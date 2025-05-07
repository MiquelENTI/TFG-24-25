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
    private Character character;

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
    protected override void InitCardOnBoardText()
    {
        cob_AtkText = transform.GetChild(0).GetChild(0).GetChild(7).GetComponent<TMP_Text>();
        cob_HpText = transform.GetChild(0).GetChild(0).GetChild(6).GetComponent<TMP_Text>();
        cob_ManaText = transform.GetChild(0).GetChild(0).GetChild(8).GetComponent<TMP_Text>();
        cob_NameText = transform.GetChild(0).GetChild(0).GetChild(9).GetComponent<TMP_Text>();
        cob_CardSprite = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
    }

    protected override void UpdateCardToDisplayText()
    {
        characterStats = character.GetCharacterStats();

        base.UpdateCardToDisplayText();
    }

    protected override void UpdateCardOnBoardText()
    {
        photonView.RPC("ChangeCardOnBoard_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void SetCharacterRPC(int characterId, int teamTypeInt)
    {
        TeamType tokenTeam = (TeamType)teamTypeInt;
        Character characterToAssign = CharacterClassSelector(characterId, tokenTeam, gameObject);
        SetCharacter(characterToAssign);

        cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/" + characterStats.name);
        if (cardSprite == null)
        {
            cardSprite = Resources.Load<Sprite>("cardsprites/ilustracions/See_the_future");
        }

        movementSprite = Resources.Load<Sprite>("cardsprites/" + characterStats.movementType.ToString());
    }

    [PunRPC]
    void ChangeCardOnBoard_RPC()
    {
        cob_AtkText.text = character.GetCharacterStats().dmg.ToString();

        cob_HpText.text = character.GetCharacterStats().hp.ToString();

        cob_ManaText.text = character.GetCharacterStats().manaCost.ToString();

        cob_NameText.text = character.GetCharacterStats().name;

        cob_CardSprite.sprite = cardSprite;
    }

    public Character GetCharacter() { return character; }

    public void SetCharacter(Character newCharacter)
    {
        character = newCharacter;
        characterStats = character.GetCharacterStats();
    }

    public override void CheckIsOutsideBoard()
    {
        CellNodeManager.Instance.hidePossibleMovements.Invoke(character.GetOnTileId());
        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (isOutsideBoard)
        {
            transform.position = CellNodeManager.Instance.GetNodeById(character.GetOnTileId()).GetPosition();
        }
        else
        {
            MyEventHandler.Instance.moveToken.Invoke(tileHovering, character.GetId());
        }
    }



    private Character CharacterClassSelector(int id, TeamType tokenTeam, GameObject instantiatedToken)
    {
        CharacterStats stats = (CharacterStats)TemporalCardDataBase.Instance.GetTemporalStats(id).Item2;
        switch (id)
        {
            case 1:
                return new Wizard(stats, tokenTeam, instantiatedToken);
            case 2:
                return new Cavalier(stats, tokenTeam, instantiatedToken);
            case 3:
                return new Salmon(stats, tokenTeam, instantiatedToken);
            case 5:
                return new Fly(stats, tokenTeam, instantiatedToken);
            case 6:
                return new Turtle(stats, tokenTeam, instantiatedToken);
            case 7:
                return new Mimic(stats, tokenTeam, instantiatedToken);
            case 8:
                return new Pichi(stats, tokenTeam, instantiatedToken);
            case 10:
                return new Medusa(stats, tokenTeam, instantiatedToken);
            case 13:
                return new Mummy(stats, tokenTeam, instantiatedToken);
            case 14:
                return new TheGun(stats, tokenTeam, instantiatedToken);
            case 15:
                return new HumanWerewolf(stats, tokenTeam, instantiatedToken);
            case 16:
                return new DeathHorseman(stats, tokenTeam, instantiatedToken);
            case 17:
                return new TerracottaWarrior(stats, tokenTeam, instantiatedToken);
            case 18:
                return new Ghost(stats, tokenTeam, instantiatedToken);
            case 19:
                return new Grasshopper(stats, tokenTeam, instantiatedToken);
            case 20:
                return new MagicKarp(stats, tokenTeam, instantiatedToken);
            case 21:
                return new Archer(stats, tokenTeam, instantiatedToken);
            case 22:
                return new Chicken(stats, tokenTeam, instantiatedToken);
            case 25:
                return new Leech(stats, tokenTeam, instantiatedToken);
            case 26:
                return new Berserker(stats, tokenTeam, instantiatedToken);
            case 27:
                return new Hydra(stats, tokenTeam, instantiatedToken);
            case 28:
                return new Zombie(stats, tokenTeam, instantiatedToken);
            case 29:
                return new Necromancer(stats, tokenTeam, instantiatedToken);
            case 30:
                return new Pocholo(stats, tokenTeam, instantiatedToken);
            case 31:
                return new Charybdis(stats, tokenTeam, instantiatedToken);
            case 32:
                return new Kamikaze(stats, tokenTeam, instantiatedToken);
            case 35:
                return new Dragon(stats, tokenTeam, instantiatedToken);
            case 36:
                return new Lacus(stats, tokenTeam, instantiatedToken);
            case 37:
                return new Samurai(stats, tokenTeam, instantiatedToken);
            case 38:
                return new Giant(stats, tokenTeam, instantiatedToken);
            case 39:
                return new Ant(stats, tokenTeam, instantiatedToken);
            case 40:
                return new Wyvern(stats, tokenTeam, instantiatedToken);






            case -1:
                return new Dummy(stats, tokenTeam);
            default:
                Debug.LogError("NO CHARACTER RECOGNISED");
                return new Character(stats, tokenTeam, instantiatedToken);
        }

    }
}
