using Autodesk.Fbx;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameObject : DragableGameObject
{
    private int cardId;
    private int cardIdToSpawn = -10;

    private CardHold cardHold;
    
    private SpawnCardController spawnCardController;

    protected override void Awake()
    {
        base.Awake();
    }
        
    protected override void Start()
    {
        base.Start();
    }

    protected override void InitCardOnBoardText()
    {
        cob_AtkText = transform.GetChild(0).GetChild(7).GetComponent<TMP_Text>();
        cob_HpText = transform.GetChild(0).GetChild(8).GetComponent<TMP_Text>();
        cob_NameText = transform.GetChild(0).GetChild(9).GetComponent<TMP_Text>();
        cob_ManaText = transform.GetChild(0).GetChild(10).GetComponent<TMP_Text>();
        cob_Description = transform.GetChild(0).GetChild(11).GetComponent<TMP_Text>();
        cob_CardSprite = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        cob_MovementSprite = transform.GetChild(0).GetChild(12).GetComponent<Image>();
        cob_AbilityBackground = transform.GetChild(0).GetChild(13).GetComponent<Image>();
        cob_AbilitySprite = transform.GetChild(0).GetChild(14).GetComponent<Image>();
        cob_doubleScoreBackground = transform.GetChild(0).GetChild(15).GetComponent<Image>();
        cob_doubleScoreSprite = transform.GetChild(0).GetChild(16).GetComponent<Image>();
    }
    protected override void UpdateCardOnBoardText()
    {
        try // Character
        {
            cob_AtkText.text = characterStats.dmg.ToString();
            cob_HpText.text = characterStats.hp.ToString();
            cob_ManaText.text = characterStats.manaCost.ToString();
            cob_NameText.text = characterStats.name;
            cob_Description.text = characterStats.description;
        }
        catch // Effects
        {
            cob_AtkText.text = "0";
            cob_HpText.text = "0";
            cob_ManaText.text = effectStats.manaCost.ToString();
            cob_NameText.text = effectStats.name;
            cob_Description.text = effectStats.description;
        }

        cob_CardSprite.sprite = cardSprite;
        cob_MovementSprite.sprite = movementSprite;

        if (abilitySprite != null)
        { cob_AbilitySprite.sprite = abilitySprite; }

        if (doubleScoreSprite != null)
        { cob_doubleScoreSprite.sprite = doubleScoreSprite; }
    }
    public override void CheckIsOutsideBoard()
    {
        CellNodeManager.Instance.hidePossibleSpawnTiles.Invoke(IsBlue ? TeamType.BLUE : TeamType.RED);

        if (isOutsideBoard)
        {
            if (TurnManagerScript.Instance.isPCVersion)
            { cardHold.ReorganizeCards(); }
        }
        else
        {
            CardStats cardStats = TemporalCardDataBase.Instance.GetTemporalStats(-1).Item2;
            if (TemporalCardDataBase.Instance.GetTemporalStats(cardIdToSpawn).Item1)
            { cardStats = TemporalCardDataBase.Instance.GetTemporalStats(cardIdToSpawn).Item2; }
            else
            { return; }

            if (cardStats.cardType == CardType.CHARACTER)
            {
                CellNode cellToSpawn = CellNodeManager.Instance.GetNodeById(tileHovering);

                if (!cellToSpawn.IsOccupied() && (int)cellToSpawn.GetSpawnable() == TurnManagerScript.Instance.GetIsTurnBlueInt() && PlayerStats.Instance.GetCurrentMana() >= cardStats.manaCost)
                {
                    RequestCharacterInstantiation();
                    cardHold.DestroyCard(cardId);

                    PhotonNetwork.Destroy(gameObject);
                }
                else
                { cardHold.ReorganizeCards(); }
            }
            else
            {
                if (PlayerStats.Instance.GetCurrentMana() >= cardStats.manaCost)
                {
                    RequestEffectActivation();
                    cardHold.DestroyCard(cardId);
                }
                else
                { cardHold.ReorganizeCards(); }
            }
        }
    }

    public void Init()
    {
        planeDisplacement = 0.5f;
        objectDisplacement = 0.1f;
        plane = new Plane(Vector3.up, new Vector3(0, planeDisplacement, 0));
        cardHold = transform.parent.parent.GetComponent<CardHold>();
        
        spawnCardController = SpawnCardController.Instance;
    }

    
    private void RequestCharacterInstantiation()
    {
        Debug.Log($"[DragableUIObject] Player ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber} requesting instantiation of character ID: {cardIdToSpawn}");
        spawnCardController.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, cardIdToSpawn, PhotonNetwork.LocalPlayer.ActorNumber, tileHovering, false);
    }

    private void RequestEffectActivation()
    { Debug.Log("CARTA ACTIVADA"); }

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
                abilitySprite = Resources.Load<Sprite>("cardsprites/abilityicons/" + characterStats.abilityType.ToString());
                doubleScoreSprite = Resources.Load<Sprite>("cardsprites/AbilityIcons/doublePoints");

                if (id == 19 || id == 5 || id == 30)
                { movementSprite = Resources.Load<Sprite>("cardsprites/abilityicons/omni2"); }
                else
                { movementSprite = Resources.Load<Sprite>("cardsprites/" + characterStats.movementType.ToString()); }

                if (characterStats.abilityType.ToString() == "nothing")
                {
                    ctd_AbilitySprite.gameObject.SetActive(false);
                    ctd_AbilityBackground.gameObject.SetActive(false);
                    cob_AbilitySprite.gameObject.SetActive(false);
                    cob_AbilityBackground.gameObject.SetActive(false);
                }

                if (characterStats.scoreMultiplier == 2)
                {
                    ctd_doubleScoreSprite.gameObject.SetActive(true);
                    ctd_doubleScoreBackground.gameObject.SetActive(true);
                    cob_doubleScoreSprite.gameObject.SetActive(true);
                    cob_doubleScoreBackground.gameObject.SetActive(true);
                }
            }
            else
            {
                effectStats = (EffectStats)TemporalCardDataBase.Instance.GetTemporalStats(id).Item2;
                cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/" + effectStats.name);
                movementSprite = Resources.Load<Sprite>("cardsprites/" + MovementType.Omni.ToString());
            }
        }
        else
        { return; }

        if (cardSprite == null)
        { cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/See_the_future"); }

        if (cardIdToSpawn != -10)
        { UpdateCardOnBoardText(); }
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

    public void SetCardId(int id)
    { cardId = id; }
    public int GetCardId()
    { return cardId; }
}
