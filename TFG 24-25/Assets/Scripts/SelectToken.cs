using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SelectToken : MonoBehaviour
{
    Plane plane;
    int tileHovering = -1;

    // TEMP?
    Character character;
    [SerializeField] GameObject cardToDisplay;
    public Sprite characterSprite;

    Vector3 mouseDownPos;
    bool isDragging = false;
    bool isOutsideBoard = true;

    [SerializeField] bool IsBlue = true;

    PhotonView photonView;

    private void Awake()
    {
        if (!TryGetComponent<PhotonView>(out photonView))
        {
            gameObject.AddComponent<PhotonView>();
            photonView = GetComponent<PhotonView>();
        }
    }

    void Start()
    {
        /*
        CharacterStats stats = new CharacterStats(0,5,10,1000,MovementType.Omni);

        if (IsBlue)
        {
            character = new Cavalier(stats, (TeamType)(IsBlue ? 1 : 0), transform.gameObject, characterSprite);
        }
        else
        {
            character = new Medusa(stats, (TeamType)(IsBlue ? 1 : 0), transform.gameObject, characterSprite);
        }
        */
        plane = new Plane(Vector3.up, Vector3.up);
        cardToDisplay = GameObject.FindGameObjectWithTag("CardToDisplay").transform.GetChild(0).gameObject;
        changeCardOnBoard();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Clic derecho
        {
            if (!GetComponent<PhotonView>().IsMine)
                return;

            // Realizar Raycast desde la c�mara hacia la posici�n del rat�n
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // Si el raycast colisiona con algo
            {
                if (hit.transform == transform) // Verifica si el objeto clickeado es este token
                {
                    SeeTokenCard();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            CharactersManager.Instance.TriggerOnStartTurn();
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            CharactersManager.Instance.TriggerOnEndTurn();
        }
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
        if (!GetComponent<PhotonView>().IsMine)
            return;

        mouseDownPos = GetMouseWorldPos();

        CellNodeManager.Instance.togglePossibleMovements.Invoke(character.GetOnTileId());
    }

    private void OnMouseDrag()
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        Vector3 newMousePos = GetMouseWorldPos();

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
        if (!GetComponent<PhotonView>().IsMine)
            return;

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

        CellNodeManager.Instance.togglePossibleMovements.Invoke(character.GetOnTileId());
        MyEventHandler.Instance.moveToken.Invoke(tileHovering, character.GetId());
        character.GetCharacterStats().PrintStats();
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
        cardToDisplay.transform.Find("ImageSprite").GetComponent<Image>().sprite = character.GetCardSprite();

        cardToDisplay.transform.Find("AttackText").GetComponent<Text>().text = character.GetAttack().ToString();

        cardToDisplay.transform.Find("HealthText").GetComponent<Text>().text = character.GetHealth().ToString();

        cardToDisplay.transform.Find("ManaCostText").GetComponent<Text>().text = character.getManaCost().ToString();

        cardToDisplay.transform.Find("DescriptionText").GetComponent<Text>().text = character.GetDescription().ToString();

        cardToDisplay.SetActive(true);
    }

    void changeCardOnBoard()
    {
        if (character == null)
        {
            Debug.LogError("Character no ha sido inicializado.");
            return;
        }

        Transform cardCanvas = transform.Find("Canvas");

        if (cardCanvas != null)
        {
            Transform CardInBoard = cardCanvas.Find("CardInBoard");

            if (CardInBoard != null)
            {
                Image cardImage = CardInBoard.Find("ImageSprite").GetComponent<Image>();
                Text attackText = CardInBoard.Find("AttackText").GetComponent<Text>();
                Text healthText = CardInBoard.Find("HealthText").GetComponent<Text>();
                Text manaText = CardInBoard.Find("ManaCostText").GetComponent<Text>();

                cardImage.sprite = character.GetCardSprite();
                attackText.text = character.GetAttack().ToString();
                healthText.text = character.GetHealth().ToString();
                manaText.text = character.getManaCost().ToString();
            }
            else
            {
                Debug.LogError("No se encontr� el objeto 'CardInBoard' dentro del canvas.");
            }
        }
        else
        {
            Debug.LogError("No se encontr� el canvas.");
        }
    }


    public void SetOnTileId(int tileId)
    {
        tileHovering = tileId;
    }

    public Character GetCharacter() { return character; }

    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;
    }
    public void SetOutsideBoard(bool isOutside)
    {
        isOutsideBoard = isOutside;
    }
}
