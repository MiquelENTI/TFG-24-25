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

    int cardIdToSpawn = -10;
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
                Debug.LogError("No se encontró el componente CardGameController en SpawnManager!");
            }
        }
        else
        {
            Debug.LogError("No se encontró GameObject con Tag 'SpawnManager' para el CardGameController!");
        }

        if (cardIdToSpawn != -10)
        {
            UpdateCardOnBoardText();
        }
    }

    private void Update()
    {
        if (playerInputs.Gameplay.MousePosition.ReadValue<Vector2>().y < 460.0f)
        {
            cardHold.LookToPlayerCam();
        }
        else
        {
            cardHold.DefaultCardRotation();
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
                CardStats cardStats = TemporalCardDataBase.Instance.GetTemporalStats(-1).Item2;
                if (TemporalCardDataBase.Instance.GetTemporalStats(cardIdToSpawn).Item1)
                {
                    cardStats = TemporalCardDataBase.Instance.GetTemporalStats(cardIdToSpawn).Item2;
                }
                else
                {
                    Debug.LogError("STATS DE CARTA NO ENCONTRADAS");
                    return;
                }

                if (cardStats.cardType == CardType.CHARACTER)
                {
                    CellNode cellToSpawn = CellNodeManager.Instance.GetNodeById(tileHovering);

                    if (!cellToSpawn.IsOccupied() && (int)cellToSpawn.GetSpawnable() == TurnManagerScript.Instance.GetIsBlueInt() && PlayerStats.Instance.GetCurrentMana() >= cardStats.manaCost)
                    {
                        RequestCharacterInstantiation();
                        cardHold.DestroyCard(cardId);
                    }
                    else
                    {
                        cardHold.ReorganizeCards();
                    }
                }
                else
                {
                    if (PlayerStats.Instance.GetCurrentMana() >= cardStats.manaCost)
                    {
                        RequestEffectActivation();
                        cardHold.DestroyCard(cardId);
                    }
                    else
                    {
                        cardHold.ReorganizeCards();
                    }
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
            Debug.Log($"[DragableUIObject] Player ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber} requesting instantiation of character ID: {cardIdToSpawn}");
            gameController.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, cardIdToSpawn, PhotonNetwork.LocalPlayer.ActorNumber, tileHovering);
        }
        else
        {
            Debug.LogError("GameController no asignado en DragableUIObject!");
        }
    }

    void RequestEffectActivation()
    {
        Debug.Log("CARTA ACTIVADA");
    }

    public void SetCardId(int id)
    { cardId = id; }
    public int GetCardId()
    { return cardId; }

    public void SetCardToSpawnId(int id)
    { 
        cardIdToSpawn = id;
        // Info To Display it on Card


        if (TemporalCardDataBase.Instance.GetTemporalStats(id).Item1)
        {
            if (TemporalCardDataBase.Instance.GetTemporalStats(id).Item2.cardType == CardType.CHARACTER)
            {
                characterStats = (CharacterStats)TemporalCardDataBase.Instance.GetTemporalStats(id).Item2;
                cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/" + characterStats.name);
                movementSprite = Resources.Load<Sprite>("cardsprites/" + characterStats.movementType.ToString());
            }
            else
            {
                effectStats = (EffectStats)TemporalCardDataBase.Instance.GetTemporalStats(id).Item2;
                cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/" + effectStats.name);
                movementSprite = Resources.Load<Sprite>("cardsprites/" + MovementType.Omni.ToString());
            }
        }
        else
        {
            return;
        }

        if (cardSprite == null)
        {
            cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/See_the_future");
        }
    }

    public void SetCharacterToSpawnId_ManaCostIncrease(int id)
    {
        cardIdToSpawn = id;
        // Info To Display it on Card
        characterStats = (CharacterStats)TemporalCardDataBase.Instance.GetTemporalStats(id).Item2;
        characterStats.manaCost++;
        cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/" + characterStats.name);
        //movementSprite = Resources.Load<Sprite>("cardsprites/ilustracions/" + characterStats.name);
        if (cardSprite == null)
        {
            cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/See_the_future");
        }
        movementSprite = Resources.Load<Sprite>("cardsprites/" + characterStats.movementType.ToString());
    }

    public int GetCharacterToSpawnId()
    { return cardIdToSpawn; }

    public override void InitCardOnBoardText()
    {
        cob_AtkText = transform.GetChild(0).GetChild(7).GetComponent<TMP_Text>();
        cob_HpText = transform.GetChild(0).GetChild(8).GetComponent<TMP_Text>();
        cob_ManaText = transform.GetChild(0).GetChild(10).GetComponent<TMP_Text>();
        cob_NameText = transform.GetChild(0).GetChild(9).GetComponent<TMP_Text>();
        cob_CardSprite = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        cob_MovementSprite = transform.GetChild(0).GetChild(12).GetComponent<Image>();
    }

    void SpawnCharacter()
    {
        CharacterStats characterStats = TemporalCardDataBase.Instance.GetTemporalStats(characterIdToSpawn);
        CellNode cellToSpawn = CellNodeManager.Instance.GetNodeById(tileHovering);


        if ((int)cellToSpawn.GetSpawnable() == TurnManagerScript.Instance.GetIsBlueInt() && PlayerStats.Instance.GetCurrentMana() >= characterStats.manaCost)
        {
            if (!cellToSpawn.IsOccupied() && characterIdToSpawn != 31)
            {
                RequestCharacterInstantiation();
                cardHold.DestroyCard(cardId);
            }
            else if (characterIdToSpawn == 31 && cellToSpawn.IsOccupied())
            {
                RequestCharacterInstantiation();
                cardHold.DestroyCard(cardId);
            }
        }
        else
        {
            cardHold.ReorganizeCards();
        }
    }

    void SpawnEffect()
    {

    }
}
