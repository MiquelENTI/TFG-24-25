using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DragableGameObject : MonoBehaviour
{
    protected Plane plane;
    protected float planeDisplacement;
    protected float objectDisplacement;

    protected Vector3 mouseDownPos;
    protected bool isDragging = false;
    protected int tileHovering = -1;
    
    protected bool isOutsideBoard = true;

    protected PlayerInputs playerInputs;
    protected WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected GameObject gameObjectSelected; // mirar de ferho millor

    protected CharacterStats characterStats;

    protected TurnManagerScript TurnManagerScript;
    [SerializeField] protected bool IsBlue;

    [SerializeField] protected GameObject cardToDisplay;
    protected TMP_Text ctd_AtkText;
    protected TMP_Text ctd_HpText;
    protected TMP_Text ctd_NameText;
    protected TMP_Text ctd_ManaText;
    protected TMP_Text ctd_Description;
    protected Image ctd_CardSprite;
    protected Image ctd_MovementSprite;

    protected TMP_Text cob_AtkText;
    protected TMP_Text cob_HpText;
    protected TMP_Text cob_NameText;
    protected TMP_Text cob_ManaText;
    protected Image cob_CardSprite;
    protected Image cob_MovementSprite;

    protected Sprite cardSprite;
    protected Sprite movementSprite;

    protected virtual void Awake()
    {
        playerInputs = new PlayerInputs();
        cardToDisplay = GameObject.FindGameObjectWithTag("CardToDisplay").transform.GetChild(0).gameObject;
        InitCardToDisplayText();
        InitCardOnBoardText();
    }

    protected virtual void Start()
    {
        playerInputs.Gameplay.MouseLeftClick.started += _ => LeftMouseDownAction();
        playerInputs.Gameplay.MouseLeftClick.canceled += _ => LeftMouseUpAction();

        playerInputs.Gameplay.MouseRightClick.started += _ => RightClickDownAction();
        playerInputs.Gameplay.MouseRightClick.canceled += _ => RightClickUpAction();

        GameObject turnManagerObject = GameObject.Find("TurnManager");
        if (turnManagerObject != null)
        {
            TurnManagerScript = turnManagerObject.GetComponent<TurnManagerScript>();
            if (TurnManagerScript == null)
            {
                Debug.LogError("TurnManagerScript no encontrado en el GameObject 'turnmanager'.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el GameObject 'turnmanager' en la escena.");
        }

        IsBlue = PhotonNetwork.IsMasterClient ? true : false;

        if (!IsBlue)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180.0f, 0));
        }
    }

    // Detect and Calculate Mouse World Position to Move Inside a Plane
    protected virtual void GetMouseWorldPos(string colliderTag)
    {
        Ray ray = Camera.main.ScreenPointToRay(playerInputs.Gameplay.MousePosition.ReadValue<Vector2>());
        
        RaycastHit hit;

       // Debug.Log("1");

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("2");
            if (hit.collider != null && hit.collider.tag == colliderTag)
            {
                //Debug.Log("3");
                gameObjectSelected = hit.collider.gameObject;

                if ((gameObjectSelected.GetComponent<DragableGameObject>().IsBlue == gameObjectSelected.GetComponent<DragableGameObject>().TurnManagerScript.getIsBlue()) && gameObjectSelected.GetComponent<PhotonView>().IsMine)
                {
                    //Debug.Log("4");
                    StartCoroutine(DragUpdate(hit.collider.gameObject));
                }
            }
        }
    }

    // When mouse is pressed down get an initial Mouse World Pos
    protected virtual void LeftMouseDownAction()
    {
    }

    // Cancel Dragging
    protected virtual void LeftMouseUpAction()
    {
        isDragging = false;
    }

    protected virtual void RightClickDownAction()
    {
        Ray ray = Camera.main.ScreenPointToRay(playerInputs.Gameplay.MousePosition.ReadValue<Vector2>());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                SeeTokenCard();
                ToggleCardToDisplay(true);
            }
        }
    }

    protected virtual void RightClickUpAction()
    {
        ToggleCardToDisplay(false);
    }

    // Assign On Which Board Tile is the GameObject hovering
    public void SetOnTileId(int tileId)
    {
        tileHovering = tileId;
    }

    // Bool that Indicates if the GameObject is inside the Board Space
    public void SetOutsideBoard(bool isOutside)
    {
        isOutsideBoard = isOutside;
    }

    public int GetTileHovering()
    { return tileHovering; }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    protected virtual IEnumerator DragUpdate(GameObject clickedGameObject)
    {
        Vector3 mousePos = Vector3.zero;

        isDragging = true;

        while (playerInputs.Gameplay.MouseLeftClick.ReadValue<float>() != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(playerInputs.Gameplay.MousePosition.ReadValue<Vector2>());

            if (plane.Raycast(ray, out var enter))
            {
                mousePos = ray.GetPoint(enter);
                mousePos.y = -1.5f;
                clickedGameObject.transform.position = mousePos;
                yield return waitForFixedUpdate;
            }
        }
    }

    void SeeTokenCard()
    {
        if (isDragging || cardToDisplay.activeSelf)
        {
            return;
        }

        UpdateCardToDisplayText();
    }

    void InitCardToDisplayText()
    {
        ctd_AtkText = cardToDisplay.transform.GetChild(7).GetComponent<TMP_Text>();
        ctd_HpText = cardToDisplay.transform.GetChild(8).GetComponent<TMP_Text>();
        ctd_NameText = cardToDisplay.transform.GetChild(6).GetComponent<TMP_Text>();
        ctd_ManaText = cardToDisplay.transform.GetChild(9).GetComponent<TMP_Text>();
        ctd_Description = cardToDisplay.transform.GetChild(10).GetComponent<TMP_Text>();
        ctd_CardSprite = cardToDisplay.transform.GetChild(0).GetComponent<Image>();
        ctd_MovementSprite = cardToDisplay.transform.GetChild(11).GetComponent<Image>();
        
    }

    public virtual void InitCardOnBoardText()
    {

    }

    public virtual void UpdateCardToDisplayText()
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

        ctd_AtkText.text = characterStats.dmg.ToString();

        ctd_HpText.text = characterStats.hp.ToString();

        ctd_ManaText.text = characterStats.manaCost.ToString();

        ctd_NameText.text = characterStats.name;

        ctd_Description.text = characterStats.description;

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

    public virtual void UpdateCardOnBoardText()
    {
        cob_AtkText.text = characterStats.dmg.ToString();

        cob_HpText.text = characterStats.hp.ToString();

        cob_ManaText.text = characterStats.manaCost.ToString();

        cob_NameText.text = characterStats.name;

        if (cardSprite != null)
        {
            cob_CardSprite.sprite = cardSprite;
        }
        else
        {
            cob_CardSprite.sprite = null;
        }
    }

    public void ToggleCardToDisplay(bool state)
    {
        cardToDisplay.SetActive(state);
    }
}
