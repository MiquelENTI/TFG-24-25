using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragableGameObject : MonoBehaviour
{
    protected Plane plane;
    protected float planeDisplacement;
    protected float objectDisplacement;

    protected int tileHovering = -1;
    
    protected bool isOutsideBoard = true;

    protected CharacterStats characterStats;
    protected EffectStats effectStats;

    protected TurnManagerScript turnManagerScript;
    [SerializeField] protected bool IsBlue;

    [SerializeField] protected GameObject cardToDisplay;

    #region Card To Display (ctd)
    protected TMP_Text ctd_AtkText;
    protected TMP_Text ctd_HpText;
    protected TMP_Text ctd_NameText;
    protected TMP_Text ctd_ManaText;
    protected TMP_Text ctd_Description;
    protected Image ctd_CardSprite;
    protected Image ctd_MovementSprite;
    protected Image ctd_AbilityBackground;
    protected Image ctd_AbilitySprite;
    protected Image ctd_doubleScoreBackground;
    protected Image ctd_doubleScoreSprite;
    #endregion

    #region Card On Board (cob)
    protected TMP_Text cob_AtkText;
    protected TMP_Text cob_HpText;
    protected TMP_Text cob_NameText;
    protected TMP_Text cob_ManaText;
    protected Image cob_CardSprite;
    protected Image cob_MovementSprite;
    #endregion

    protected Sprite cardSprite;
    protected Sprite movementSprite;

    protected PhotonView photonView;
    protected virtual void Awake()
    {
        cardToDisplay = GameObject.FindGameObjectWithTag("CardToDisplay").transform.GetChild(0).gameObject;

        InitCardToDisplayText();
        InitCardOnBoardText();
        photonView = GetComponent<PhotonView>();
    }

    protected virtual void Start()
    {
        turnManagerScript = TurnManagerScript.Instance;
        

        IsBlue = PhotonNetwork.IsMasterClient ? true : false;

        if (!IsBlue)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
        }
    }

    protected virtual void InitCardOnBoardText()
    {

    }

    protected virtual void UpdateCardToDisplayText()
    {
        // 0 -> Card Sprite
        // 1 -> Name Container
        // 2 -> DMG Container
        // 3 -> Name
        // 4 -> HP Container
        // 5 -> HP
        // 6 -> DMG
        // 7 -> Mana
        // 8 -> Description

        //characterStats.PrintStats();

        try
        {
            ctd_AtkText.text = characterStats.dmg.ToString();

            ctd_HpText.text = characterStats.hp.ToString();

            ctd_ManaText.text = characterStats.manaCost.ToString();

            ctd_NameText.text = characterStats.name;

            ctd_Description.text = characterStats.description;

            ctd_AbilitySprite.sprite = Resources.Load<Sprite>("cardsprites/AbilityIcons/" + characterStats.abilityType.ToString());

            if (characterStats.abilityType.ToString() == "nothing")
            {
                ctd_AbilitySprite.gameObject.SetActive(false);
                ctd_AbilityBackground.gameObject.SetActive(false);
            }
            else
            {
                ctd_AbilitySprite.gameObject.SetActive(true);
                ctd_AbilityBackground.gameObject.SetActive(true);
            }


            if (characterStats.scoreMultiplier == 1)
            {
                ctd_doubleScoreSprite.gameObject.SetActive(false);
                ctd_doubleScoreBackground.gameObject.SetActive(false);
            }
            else if (characterStats.scoreMultiplier == 2)
            {
                ctd_doubleScoreSprite.sprite = Resources.Load<Sprite>("cardsprites/AbilityIcons/doublePoints");
                ctd_doubleScoreSprite.gameObject.SetActive(true);
                ctd_doubleScoreBackground.gameObject.SetActive(true);
            }

        }
        catch
        {
            ctd_AtkText.text = "0";

            ctd_HpText.text = "0";

            ctd_ManaText.text = effectStats.manaCost.ToString();

            ctd_NameText.text = effectStats.name;

            ctd_Description.text = effectStats.description;
        }

        // Potser fer funcio a part per canviar sprite
        if (cardSprite != null)
        {
            ctd_CardSprite.sprite = cardSprite;
        }
        else
        {
            ctd_CardSprite.sprite = null;
        }

        ctd_MovementSprite.sprite = movementSprite;
    }

    protected virtual void UpdateCardOnBoardText()
    {
        try
        {
            cob_AtkText.text = characterStats.dmg.ToString();

            cob_HpText.text = characterStats.hp.ToString();

            cob_ManaText.text = characterStats.manaCost.ToString();

            cob_NameText.text = characterStats.name;
        }
        catch
        {
            cob_AtkText.text = "0";

            cob_HpText.text = "0";

            cob_ManaText.text = effectStats.manaCost.ToString();

            cob_NameText.text = effectStats.name;
        }

        if (cardSprite != null)
        {
            cob_CardSprite.sprite = cardSprite;
        }
        else
        {
            cob_CardSprite.sprite = null;
        }
        cob_MovementSprite.sprite = movementSprite;
    }

    public virtual void CheckIsOutsideBoard()
    {

    }

    private void InitCardToDisplayText()
    {
        ctd_AtkText = cardToDisplay.transform.GetChild(7).GetComponent<TMP_Text>();
        ctd_HpText = cardToDisplay.transform.GetChild(8).GetComponent<TMP_Text>();
        ctd_NameText = cardToDisplay.transform.GetChild(6).GetComponent<TMP_Text>();
        ctd_ManaText = cardToDisplay.transform.GetChild(9).GetComponent<TMP_Text>();
        ctd_Description = cardToDisplay.transform.GetChild(10).GetComponent<TMP_Text>();
        ctd_CardSprite = cardToDisplay.transform.GetChild(0).GetComponent<Image>();
        ctd_MovementSprite = cardToDisplay.transform.GetChild(11).GetComponent<Image>();
        ctd_AbilityBackground = cardToDisplay.transform.GetChild(13).GetComponent<Image>();
        ctd_AbilitySprite = cardToDisplay.transform.GetChild(14).GetComponent<Image>();
        ctd_doubleScoreBackground = cardToDisplay.transform.GetChild(15).GetComponent<Image>();
        ctd_doubleScoreSprite = cardToDisplay.transform.GetChild(16).GetComponent<Image>();

    }
    public void SeeTokenCard()
    {
        UpdateCardToDisplayText();
    }
    public void RotateCardToDisplayVR()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            cardToDisplay.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            cardToDisplay.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
    public void ToggleCardToDisplay(bool state)
    {
        cardToDisplay.SetActive(state);
    }

    public Plane GetPlane()
    { return plane; }
    public float GetObjectDisplacement()
    { return objectDisplacement + planeDisplacement; }
    // Assign On Which Board Tile is the GameObject hovering
    public void SetOnTileId(int tileId)
    { tileHovering = tileId; }

    // Bool that Indicates if the GameObject is inside the Board Space
    public void SetOutsideBoard(bool isOutside)
    { isOutsideBoard = isOutside; }
    public bool GetIsBlue()
    { return IsBlue; }

    public void UpdateObjectPosition(Vector3 newPosition, Quaternion newRotation)
    {
        photonView.RPC("RPC_UpdateObjectPosition", RpcTarget.All, newPosition, newRotation);
    }

    [PunRPC]
    public void RPC_UpdateObjectPosition(Vector3 newPosition, Quaternion newRotation)
    {
        transform.SetPositionAndRotation(newPosition, newRotation);
    }
}
