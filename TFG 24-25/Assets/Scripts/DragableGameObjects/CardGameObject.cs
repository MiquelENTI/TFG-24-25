using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CardGameObject : DragableGameObject
{
    //Plane plane = new Plane(Vector3.up, Vector3.up);

    CardHold cardHold;
    int cardId;

    public GameObject spawnTileCollider;

    int characterIdToSpawn = 1;
    public SpawnCardController gameController;

    protected override void Awake()
    {
        base.Awake();

        
    }
    protected override void Start()
    {
        base.Start();
        planeDisplacement = 0.5f;
        objectDisplacement = 0.1f;
        plane = new Plane(Vector3.up, new Vector3(0, planeDisplacement, 0));
        cardHold = transform.parent.parent.GetComponent<CardHold>();
        spawnTileCollider = transform.parent.GetChild(1).gameObject;



        GameObject spawnManagerGO = GameObject.Find("SpawnManager");
        if (spawnManagerGO != null)
        {
            gameController = spawnManagerGO.GetComponent<SpawnCardController>();
            if (gameController == null)
            {
                Debug.LogError("No se encontr el componente CardGameController en SpawnManager!");
            }
        }
        else
        {
            Debug.LogError("No se encontr GameObject con Tag 'SpawnManager' para el CardGameController!");
        }
    }


    protected override void LeftMouseDownAction()
    {
        if (TurnManagerScript != null && TurnManagerScript.getIsBlue() != IsBlue)
        {
            Debug.Log("No es el turno del jugador actual.");
            return;
        }

        // When Dragging, the GameObject (Visible Card) Updates its Position and its Sibling, the Collider that Detects on Which Tile is it Hovering On.
        GetMouseWorldPos("CardGameObject");

        if (gameObjectSelected == null)
        { return; }

        if (!gameObjectSelected.GetComponent<PhotonView>().IsMine)
        { return; }

        if (isDragging)
        {
            CellNodeManager.Instance.showPossibleSpawnTiles.Invoke(cardHold.GetTeamType());
        }
    }

    protected override void LeftMouseUpAction()
    {
        base.LeftMouseUpAction();

        if (gameObjectSelected == null)
        { return; }

        if (isOutsideBoard)
        {
            // Returns to The Hand
            cardHold.ReorganizeCards();
        }
        else
        {
            // Verify If its the card selected
            if (cardId == gameObjectSelected.GetComponent<CardGameObject>().GetCardId())
            {
                // Spawn Token in tileHovering if is empty
                if (!CellNodeManager.Instance.GetNodeById(tileHovering).IsOccupied())
                {
                    RequestCharacterInstantiation();
                    cardHold.DestroyCard(cardId);
                }
                else
                {
                    cardHold.ReorganizeCards();
                }    
            }
        }
        
        CellNodeManager.Instance.hidePossibleSpawnTiles.Invoke(cardHold.GetTeamType());
    }

    protected override IEnumerator DragUpdate(GameObject clickedGameObject)
    {
        Vector3 mousePos = Vector3.zero;

        isDragging = true;

        while (playerInputs.Gameplay.MouseLeftClick.ReadValue<float>() != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(playerInputs.Gameplay.MousePosition.ReadValue<Vector2>());

            if (plane.Raycast(ray, out var enter))
            {
                mousePos = ray.GetPoint(enter);
                mousePos.y = planeDisplacement + objectDisplacement;

                clickedGameObject.transform.position = mousePos;
                clickedGameObject.GetComponent<CardGameObject>().GetSpawnTileCollider().transform.position = mousePos;

                yield return waitForFixedUpdate;
            }
        }
    }

    public GameObject GetSpawnTileCollider()
    { return spawnTileCollider; }

    void RequestCharacterInstantiation()
    {
        if (gameController != null)
        {
            Debug.Log($"[DragableUIObject] Player ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber} requesting instantiation of character ID: {characterIdToSpawn}");
            gameController.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, characterIdToSpawn, PhotonNetwork.LocalPlayer.ActorNumber, tileHovering);
        }
        else
        {
            Debug.LogError("GameController no asignado en DragableUIObject!");
        }
    }

    public void SetCardId(int id)
    { cardId = id; }
    public int GetCardId() 
    { return cardId; }

    public void SetCharacterToSpawnId(int id)
    { 
        characterIdToSpawn = id;
        // Info To Display it on Card
        //character = new Character(TemporalCardDataBase.Instance.GetTemporalStats(id), TeamType.BLUE, null, null);
    }
    public int GetCharacterToSpawnId()
    { return characterIdToSpawn; }
}
