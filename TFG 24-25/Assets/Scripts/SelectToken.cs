using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectToken : MonoBehaviour
{
    Plane plane;

    int tileHovering = -1;


    // TEMP?
    Character character;
    [SerializeField] GameObject cardToDisplay;
    
    // TEMP, en un futur fer-ho desde base dades o Resources
    public Sprite characterSprite;

    Vector3 mouseDownPos;
    float revealTimer = 0;
    float timeToReveal = 0.7f;
    bool isDragging = false;
    bool isOutsideBoard = true;

    [SerializeField] bool IsBlue = true;

    private void Awake()
    {
    }
    void Start()
    {
        CharacterStats stats = new CharacterStats(10,10,2,1000,MovementType.Omni);
        character = new(stats, (TeamType)(IsBlue ? 1 : 0), transform.gameObject, characterSprite);
        plane = new Plane(Vector3.up, Vector3.up);
        cardToDisplay = GameObject.FindGameObjectWithTag("CardToDisplay").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 mousePos = Vector3.zero;

        if (plane.Raycast(ray, out var enter))
        {
            mousePos = ray.GetPoint(enter);
            mousePos.y = 0.75f;

        }

        return mousePos;
    }

    private void OnMouseDown()
    {
        mouseDownPos = GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        Vector3 newMousePos = GetMouseWorldPos();

        SeeTokenCard();

        if (mouseDownPos != newMousePos)
        {
            isDragging = true;
        }

        if (cardToDisplay.activeSelf)
        {
            transform.position = CellNodeManager.Instance.GetNodeById(character.GetOnTileId()).GetPosition();
            return;
        }

        transform.position = newMousePos;
    }   

    private void OnMouseUp()
    {
        isDragging = false;

        if (tileHovering == -1) 
        {
            Debug.Log("TileID is -1");
            return; 
        }

        if (isOutsideBoard)
        {
            transform.position = CellNodeManager.Instance.GetNodeById(character.GetOnTileId()).GetPosition();
            return;
        }

        MyEventHandler.Instance.moveToken.Invoke(tileHovering, character.GetId());
    }


    void SeeTokenCard()
    {
        if (isDragging || cardToDisplay.activeSelf) 
        {
            return; 
        }

        if (revealTimer < timeToReveal)
        {
            //Debug.Log(revealTimer);
            revealTimer += Time.deltaTime;
            return; 
        }

        ChangeImageCardToDisplay();
        revealTimer = 0;
    }

    void ChangeImageCardToDisplay()
    {
        cardToDisplay.SetActive(true);
        cardToDisplay.transform.GetChild(0).GetComponent<Image>().sprite = character.GetCardSprite();
    }

    public void SetOnTileId(int tileId)
    {
        tileHovering = tileId;
    }

    public Character GetCharacter() 
    { return character; }

    public void SetOutsideBoard(bool isOutside)
    {
        isOutsideBoard = isOutside;
    }
}
