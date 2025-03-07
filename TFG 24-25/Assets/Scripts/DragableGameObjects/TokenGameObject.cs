using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using System;

public class TokenGameObject : DragableGameObject
{
    // TEMP?
    GameObject player;
    
    public Sprite characterSprite;

    [SerializeField] int TEMP_id;


    [SerializeField] bool IsBlue = true;

    PhotonView photonView;

    private TurnManagerScript TurnManagerScript;

    public GameObject tileCollider;

    private Character characterSelected;

    

    protected override void Awake()
    {
        base.Awake();
        
        photonView = transform.GetComponent<PhotonView>();
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

        // character = CharacterClassSelector(TEMP_id, IsBlue ? TeamType.BLUE : TeamType.RED, gameObject);

        planeDisplacement = 0.5f;
        objectDisplacement = 0.05f;
        plane = new Plane(Vector3.up, new Vector3(0, planeDisplacement, 0));
        cardToDisplay = GameObject.FindGameObjectWithTag("CardToDisplay").transform.GetChild(0).gameObject;
        changeCardOnBoard();
        transform.parent.name = character.GetCharacterStats().name;
    }

    void Update()
    {
        // See card Description

        //Debug.Log(tileHovering);

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
        if (!GetComponentInParent<PhotonView>().IsMine)
            return;

        if (TurnManagerScript != null && TurnManagerScript.getIsBlue() != IsBlue)
        {
            Debug.Log("No es el turno del jugador actual.");
            return;
        }

        GetMouseWorldPos("TokenGameObject");

        if (gameObjectSelected == null)
        { return; }

        characterSelected = gameObjectSelected.GetComponent<TokenGameObject>().GetCharacter();
        if (isDragging)
        {
            CellNodeManager.Instance.showPossibleMovements.Invoke(characterSelected.GetOnTileId());
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

        if (isOutsideBoard)
        {
            transform.position = CellNodeManager.Instance.GetNodeById(character.GetOnTileId()).GetPosition();
            return;
        }
        else
        {
            if (character.GetId() == characterSelected.GetId())
            {
                //Debug.Log(character.GetId() + " " + gameObjectSelected.GetComponent<TokenGameObject>().GetCharacter().GetId());

                CellNodeManager.Instance.hidePossibleMovements.Invoke(character.GetOnTileId());
                MyEventHandler.Instance.moveToken.Invoke(tileHovering, characterSelected.GetId());
            }
        }

        
        //character.GetCharacterStats().PrintStats();
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
                clickedGameObject.GetComponent<TokenGameObject>().GetTileCollider().transform.position = clickedGameObject.transform.position;

                yield return waitForFixedUpdate;
            }
        }
    }

    public GameObject GetTileCollider()
    { return tileCollider; }

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
    public void SetCharacterRPC(int characterId, int teamTypeInt)
    {
        TeamType tokenTeam = (TeamType)teamTypeInt;
        Character characterToAssign = CharacterClassSelector(characterId, tokenTeam, gameObject);
        SetCharacter(characterToAssign);
        changeCardOnBoard();
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
