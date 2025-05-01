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

    int cardIdToSpawn = -10;
    public SpawnCardController spawnCardController;


    protected override void Awake()
    {
        base.Awake();
    }

        
    protected override void Start()
    {
        base.Start();
        
    }

    public void Init()
    {
        planeDisplacement = 0.5f;
        objectDisplacement = 0.1f;
        plane = new Plane(Vector3.up, new Vector3(0, planeDisplacement, 0));
        cardHold = transform.parent.parent.GetComponent<CardHold>();

        GameObject spawnManagerGO = GameObject.Find("SpawnManager");
        if (spawnManagerGO != null)
        {
            spawnCardController = spawnManagerGO.GetComponent<SpawnCardController>();
            if (spawnCardController == null)
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

    public override void CheckIsOutsideBoard()
    {
        CellNodeManager.Instance.InvokeHidePossibleSpawnTiles(IsBlue);

        if (isOutsideBoard)
        {
            if(TurnManagerScript.Instance.isPCVersion)
            {
                cardHold.ReorganizeCards();
            }
        }
        else
        {
            Debug.Log("NOT OUTSIDE BOARD");
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

                cellToSpawn.PrintStatus();

                if (!cellToSpawn.IsOccupied() && (int)cellToSpawn.GetSpawnable() == TurnManagerScript.Instance.GetIsTurnBlueInt() && PlayerStats.Instance.GetCurrentMana() >= cardStats.manaCost)
                {
                    RequestCharacterInstantiation();
                    cardHold.DestroyCard(cardId);

                    PhotonNetwork.Destroy(gameObject);
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
    void RequestCharacterInstantiation()
    {
        if (spawnCardController != null)
        {
            Debug.Log($"[DragableUIObject] Player ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber} requesting instantiation of character ID: {cardIdToSpawn}");
            spawnCardController.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, cardIdToSpawn, PhotonNetwork.LocalPlayer.ActorNumber, tileHovering, false);

            SaveData.Instance.TryBufferAction("S" + tileHovering + "/" + cardIdToSpawn);
        }
        else
        {
            Debug.LogError("SpawnCardController no asignado en CardGameObject!");
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
            Debug.Log("ENTERED TRUE");
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
            Debug.Log("ENTERED FALSE");
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
}
