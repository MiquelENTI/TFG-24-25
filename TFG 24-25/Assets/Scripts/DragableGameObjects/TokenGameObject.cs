using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;
using System;
using TMPro;

public class TokenGameObject : DragableGameObject
{
    // TEMP?
    GameObject player;
    
    public Sprite characterSprite;

    [SerializeField] int TEMP_id;

    private Character character;

    PhotonView photonView;


    public GameObject tileCollider;

    private Character characterSelected;

    [SerializeField] Transform cardCanvas;

    protected override void Awake()
    {
        base.Awake();
        
        photonView = transform.GetComponent<PhotonView>();
    }

    protected override void Start()
    {
        base.Start();

        planeDisplacement = 0.5f;
        objectDisplacement = 0.05f;
        plane = new Plane(Vector3.up, new Vector3(0, planeDisplacement, 0));
        
        transform.parent.name = character.GetCharacterStats().name;
        UpdateCardOnBoardText();
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
        //if (Input.GetKeyUp(KeyCode.I))
        //{
        //    UpdateCardOnBoardText();
        //}
    }



    protected override void LeftMouseDownAction()
    {
        if (TurnManagerScript != null && TurnManagerScript.getIsBlue() != IsBlue)
        {
            Debug.Log("No es el turno del jugador actual.");
            return;
        }

        GetMouseWorldPos("TokenGameObject");

        if (gameObjectSelected == null)
        { return; }

        if (!gameObjectSelected.GetComponent<PhotonView>().IsMine)
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
                MyEventHandler.Instance.moveToken.Invoke(tileHovering, character.GetId());
                Debug.Log("CharacterSelectedID: " + characterSelected.GetId());
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

    public override void InitCardOnBoardText()
    {
        cob_AtkText = transform.GetChild(0).GetChild(0).GetChild(7).GetComponent<TMP_Text>();
        cob_HpText = transform.GetChild(0).GetChild(0).GetChild(6).GetComponent<TMP_Text>();
        cob_ManaText = transform.GetChild(0).GetChild(0).GetChild(8).GetComponent<TMP_Text>(); // Falta mana
        cob_NameText = transform.GetChild(0).GetChild(0).GetChild(9).GetComponent<TMP_Text>();
        cob_CardSprite = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        //cig_MovementSprite = transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<Image>();
    }

    public override void UpdateCardToDisplayText()
    {
        characterStats = character.GetCharacterStats();

        base.UpdateCardToDisplayText();
    }

    public override void UpdateCardOnBoardText()
    {
        //if (character == null)
        //{
        //    Debug.LogError("Character no ha sido inicializado.");
        //    return;
        //}

        //int attack = character.GetAttack();
        //int health = character.GetHealth();
        //int manaCost = character.getManaCost();
        //string name = character.GetName();

        //photonView.RPC("ChangeCardOnBoard_RPC", RpcTarget.All, attack, health, manaCost, name);
        photonView.RPC("ChangeCardOnBoard_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void SetCharacterRPC(int characterId, int teamTypeInt)
    {
        TeamType tokenTeam = (TeamType)teamTypeInt;
        Character characterToAssign = CharacterClassSelector(characterId, tokenTeam, gameObject);
        SetCharacter(characterToAssign);

        cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/" + characterStats.name);
        //movementSprite = Resources.Load<Sprite>("cardsprites/ilustracions/" + characterStats.name);
            
        //UpdateCardOnBoardText();
    }

    [PunRPC]
    //void ChangeCardOnBoard_RPC(int attack, int health, int manaCost, string name)
    void ChangeCardOnBoard_RPC()
    {
        cob_AtkText.text = character.GetAttack().ToString();

        cob_HpText.text = character.GetHealth().ToString();

        cob_ManaText.text = character.getManaCost().ToString();

        cob_NameText.text = character.GetName();

        cob_CardSprite.sprite = cardSprite;
    }

    public Character GetCharacter() { return character; }

    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;
        characterStats = character.GetCharacterStats();
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
