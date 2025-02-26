using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TokenGameObject : DragableGameObject
{
    

    // TEMP?
    Character character;
    GameObject player;
    [SerializeField] GameObject cardToDisplay;
    public Sprite characterSprite;

    [SerializeField] int TEMP_id;


    [SerializeField] bool IsBlue = true;

    PhotonView photonView;

    private TurnManagerScript TurnManagerScript;

    protected override void Awake()
    {
        base.Awake();

        if (!TryGetComponent<PhotonView>(out photonView))
        {
            gameObject.AddComponent<PhotonView>();
            photonView = GetComponent<PhotonView>();
        }
    }

    protected override void Start()
    {
        base.Start();

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

        character = CharacterClassSelector(TEMP_id, IsBlue ? TeamType.BLUE : TeamType.RED, gameObject);


        plane = new Plane(Vector3.up, Vector3.up);
        cardToDisplay = GameObject.FindGameObjectWithTag("CardToDisplay").transform.GetChild(0).gameObject;
        changeCardOnBoard();
    }

    void Update()
    {
        // See card Description
        if (Input.GetMouseButtonDown(1))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    SeeTokenCard();
                }
            }
        }

        //if (Input.GetKeyUp(KeyCode.M))
        //{
        //    CharactersManager.Instance.TriggerOnStartTurn();
        //}
        //if (Input.GetKeyUp(KeyCode.N))
        //{
        //    CharactersManager.Instance.TriggerOnEndTurn();
        //}
    }



    protected override void LeftMouseDownAction()
    {
        base.LeftMouseDownAction();

        CellNodeManager.Instance.togglePossibleMovements.Invoke(character.GetOnTileId());

        GetMouseWorldPos("TokenGameObject");

        if (TurnManagerScript != null && TurnManagerScript.getIsBlue() != IsBlue)
        {
            Debug.Log("No es el turno del jugador actual.");
            return;
        }

        if (cardToDisplay.activeSelf)
        {
            transform.position = CellNodeManager.Instance.GetNodeById(character.GetOnTileId()).GetPosition();
            return;
        }
    }

    protected override void LeftMouseUpAction()
    {
        base.LeftMouseUpAction();
    
        if (TurnManagerScript != null && TurnManagerScript.getIsBlue() != IsBlue)
        {
            Debug.Log("No es el turno del jugador actual.");
            return;
        }

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
        //character.GetCharacterStats().PrintStats();
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

        int attack = character.GetAttack();
        int health = character.GetHealth();
        int manaCost = character.getManaCost();

        photonView.RPC("ChangeCardOnBoard_RPC", RpcTarget.All, attack, health, manaCost);
    }

    [PunRPC]
    void ChangeCardOnBoard_RPC(int attack, int health, int manaCost)
    {
        if (character == null)
        {
            Debug.LogError("Character no ha sido inicializado.");
            return;
        }

        Transform cardCanvas = transform.Find("Canvas");

        if (cardCanvas != null)
        {
            Transform cardInBoard = cardCanvas.Find("CardInBoard");

            if (cardInBoard != null)
            {
                // Image cardImage = cardInBoard.Find("ImageSprite").GetComponent<Image>();
                Text attackText = cardInBoard.Find("AttackText").GetComponent<Text>();
                Text healthText = cardInBoard.Find("HealthText").GetComponent<Text>();
                Text manaText = cardInBoard.Find("ManaCostText").GetComponent<Text>();

                // cardImage.sprite = character.GetCardSprite();
                attackText.text = attack.ToString();
                healthText.text = health.ToString();
                manaText.text = manaCost.ToString();
            }
            else
            {
                Debug.LogError("No se encontró el objeto 'CardInBoard' dentro del canvas.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el canvas.");
        }
    }

    public Character GetCharacter() { return character; }

    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;
    }

    Character CharacterClassSelector(int id, TeamType tokenTeam, GameObject instantiatedToken)
    {
        CharacterStats stats = TemporalCardDataBase.Instance.GetTemporalStats(id);
        switch (id)
        {
            case 1:
                return new Cavalier(stats, tokenTeam, instantiatedToken, null);
            case 3:
                return new Salmon(stats, tokenTeam, instantiatedToken, null);
            case 5:
                return new Fly(stats, tokenTeam, instantiatedToken, null);
            case 6:
                return new Turtle(stats, tokenTeam, instantiatedToken, null);
            case 7:
                return new Mimic(stats, tokenTeam, instantiatedToken, null);
            case 10:
                return new Medusa(stats, tokenTeam, instantiatedToken, null);
            case 13:
                return new Mummy(stats, tokenTeam, instantiatedToken, null);
            case 16:
                return new DeathHorseman(stats, tokenTeam, instantiatedToken, null);
            case 17:
                return new TerracottaWarrior(stats, tokenTeam, instantiatedToken, null);
            case 20:
                return new MagicKarp(stats, tokenTeam, instantiatedToken, null);
            case 22:
                return new Chicken(stats, tokenTeam, instantiatedToken, null);
            case -1:
                return new Dummy(stats, tokenTeam);
            case 14:
                return new TheGun(stats, tokenTeam, instantiatedToken, null);
            case 15:
                return new HumanWerewolf(stats, tokenTeam, instantiatedToken, null);
            default:
                Debug.LogError("NO CHARACTER RECOGNISED");
                return new Character(stats, tokenTeam, instantiatedToken, null);
        }

    }
}
