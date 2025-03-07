using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

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

    protected Character character;

    [SerializeField] protected GameObject cardToDisplay;
    protected virtual void Awake()
    {
        playerInputs = new PlayerInputs();
        cardToDisplay = GameObject.FindGameObjectWithTag("CardToDisplay").transform.GetChild(0).gameObject;
    }

    protected virtual void Start()
    {
        playerInputs.Gameplay.MouseLeftClick.started += _ => LeftMouseDownAction();
        playerInputs.Gameplay.MouseLeftClick.canceled += _ => LeftMouseUpAction();

        playerInputs.Gameplay.MouseRightClick.started += _ => RightClickDownAction();
        playerInputs.Gameplay.MouseRightClick.canceled += _ => RightClickUpAction();
    }

    // Detect and Calculate Mouse World Position to Move Inside a Plane
    protected virtual void GetMouseWorldPos(string colliderTag)
    {
        Ray ray = Camera.main.ScreenPointToRay(playerInputs.Gameplay.MousePosition.ReadValue<Vector2>());
        
        RaycastHit hit;

        Debug.Log("1");

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("2");
            if (hit.collider != null && hit.collider.tag == colliderTag)
            {
                Debug.Log("3");
                StartCoroutine(DragUpdate(hit.collider.gameObject));
                gameObjectSelected = hit.collider.gameObject;
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
            }
        }
    }

    protected virtual void RightClickUpAction()
    {

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

        ChangeImageCardToDisplay();
    }

    void ChangeImageCardToDisplay()
    {
        // cardToDisplay.transform.Find("ImageSprite").GetComponent<Image>().sprite = character.GetCardSprite();

        Debug.Log(cardToDisplay.name);
        character.GetCharacterStats().PrintStats();

        cardToDisplay.SetActive(true);

        

        cardToDisplay.transform.GetChild(1).GetComponent<TMP_Text>().text = character.GetHealth().ToString();

        cardToDisplay.transform.GetChild(2).GetComponent<TMP_Text>().text = character.GetAttack().ToString();

        cardToDisplay.transform.GetChild(3).GetComponent<TMP_Text>().text = character.GetDescription();

        cardToDisplay.transform.GetChild(4).GetComponent<TMP_Text>().text = character.getManaCost().ToString();

        cardToDisplay.transform.GetChild(5).GetComponent<TMP_Text>().text = character.GetCharacterStats().name;

        
    }
}
