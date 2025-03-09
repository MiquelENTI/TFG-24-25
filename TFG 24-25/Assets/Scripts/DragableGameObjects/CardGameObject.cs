using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CardGameObject : DragableGameObject
{
    CardHold cardHold;
    int cardId;

    public GameObject spawnTileCollider;

    int characterIdToSpawn = 1;
    public SpawnCardController gameController;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text attackText; 
    [SerializeField] private TMP_Text cardDescriptionText;

    [SerializeField] private Image illustrationImage;

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
                Debug.LogError("No se encontró el componente CardGameController en SpawnManager!");
            }
        }
        else
        {
            Debug.LogError("No se encontró GameObject con Tag 'SpawnManager' para el CardGameController!");
        }

        if (possibleCharacterNamesToAssign != null && possibleCharacterNamesToAssign.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleCharacterNamesToAssign.Count);
            randomCharacterName = possibleCharacterNamesToAssign[randomIndex];
            Debug.Log("Nombre de personaje seleccionado aleatoriamente: " + randomCharacterName);

            UpdateCardText();
        }
    }

    void UpdateCardText()
    {
        CharacterStats stats = TemporalCardDataBase.Instance.GetTemporalStats(randomCharacterName);

        nameText.text = stats.getName();
        healthText.text = stats.getHealthToString();
        attackText.text = stats.getAttackToString();
        cardDescriptionText.text = stats.getDescription();

        string imageName = "cardsprites/ilustracions/" + stats.getName();

        Sprite cardSprite = Resources.Load<Sprite>(imageName);

        if (cardSprite != null)
        {
            illustrationImage.sprite = cardSprite;
        }
        else
        {
            Debug.LogError("No se encontró la imagen: " + imageName);
        }
    }

    protected override void LeftMouseDownAction()
    {
        if (TurnManagerScript != null && TurnManagerScript.getIsBlue() != IsBlue)
        {
            Debug.Log("No es el turno del jugador actual.");
            return;
        }

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
            cardHold.ReorganizeCards();
        }
        else
        {
            if (cardId == gameObjectSelected.GetComponent<CardGameObject>().GetCardId())
            {
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
