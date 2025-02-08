using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using System.Xml.Serialization;

public class DragableUIObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 startPoint;
    Vector2 endPoint;

    Cards card;

    [SerializeField] GameObject token;
    [SerializeField] GameObject token2;

    bool tokenSpawn = false;

    Vector3 initialPosition;

    [SerializeField] Material blueMaterial;
    [SerializeField] Material redMaterial;

    TeamType TESTING_teamType;

    void Start() { }

    public void SetDragableUIObject(Cards newCard)
    {
        //card = newCard;
        transform.parent = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHand" : "RedHand").transform;

    }

    public void TESTING_SetDragableUIObject(Cards newCard, TeamType teamType)
    {
        //card = newCard;
        transform.parent = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHand" : "RedHand").transform;
        TESTING_teamType = teamType;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPoint = eventData.pressPosition;
        initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        endPoint = eventData.position;

        transform.position = new Vector3(transform.position.x, endPoint.y, transform.position.z);

        if (transform.localPosition.y > -175.0f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -174.0f, transform.localPosition.z);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.localPosition.y > -175.0f)
        {
            //if (TESTING_teamType == TeamType.RED || TESTING_teamType == TeamType.BLUE)
            //{
            //    TESTING_CreateCharacterOrActivateEffect();
            //}
            //else
            //{
            Debug.Log("WARRO?");
            CreateCharacterOrActivateEffectWarro();
            //}    

            Destroy(gameObject);
        }
        else
        {
            transform.position = initialPosition;
        }
    }

    void CreateCharacterOrActivateEffect()
    {
        
        TeamType tokenTeam = PhotonNetwork.IsMasterClient ? TeamType.BLUE : TeamType.RED;
        if (card.IsCharacter())
        {
            Vector3 spawnPosition = new Vector3(2.16f, 0.75f, PhotonNetwork.IsMasterClient ? -3.95f : -0.5f);
            GameObject instantiatedToken = PhotonNetwork.Instantiate(PhotonNetwork.IsMasterClient ? token.name : token2.name, spawnPosition, Quaternion.identity);

            Renderer tokenRenderer = instantiatedToken.GetComponent<Renderer>();
            if (PhotonNetwork.IsMasterClient)
            {
                tokenRenderer.material = blueMaterial;
                instantiatedToken.GetComponent<PhotonView>().RPC("SyncMaterial", RpcTarget.OthersBuffered, "blue");
            }
            else
            {
                tokenRenderer.material = redMaterial;
                instantiatedToken.GetComponent<PhotonView>().RPC("SyncMaterial", RpcTarget.OthersBuffered, "red");
            }

            //instantiatedToken.GetComponent<SelectToken>().SetCharacter(CharacterClassSelector(card.GetCharacterStats().name, tokenTeam, instantiatedToken));
        }
        else
        {
           
        }
        
    }

    void CreateCharacterOrActivateEffectWarro()
    {

        TeamType tokenTeam = PhotonNetwork.IsMasterClient ? TeamType.BLUE : TeamType.RED;
        
        Vector3 spawnPosition = new Vector3(2.16f, 0.75f, PhotonNetwork.IsMasterClient ? -3.95f : -0.5f);
        GameObject instantiatedToken = PhotonNetwork.Instantiate(PhotonNetwork.IsMasterClient ? "Warro/"+token.name : "Warro/" + token2.name, spawnPosition, Quaternion.identity);

            //instantiatedToken.GetComponent<SelectToken>().SetCharacter(CharacterClassSelector(card.GetCharacterStats().name, tokenTeam, instantiatedToken));
    }

    public void TESTING_CreateCharacterOrActivateEffect()
    {
        /*
        if (card.IsCharacter())
        {
            Vector3 spawnPosition = new Vector3(2.16f, 0.75f, PhotonNetwork.IsMasterClient ? -3.95f : -0.5f);
            GameObject instantiatedToken = PhotonNetwork.Instantiate(PhotonNetwork.IsMasterClient ? token.name : token2.name, spawnPosition, Quaternion.identity);

            Renderer tokenRenderer = instantiatedToken.GetComponent<Renderer>();
            if (PhotonNetwork.IsMasterClient)
            {
                tokenRenderer.material = blueMaterial;
                instantiatedToken.GetComponent<PhotonView>().RPC("SyncMaterial", RpcTarget.OthersBuffered, "blue");
            }
            else
            {
                tokenRenderer.material = redMaterial;
                instantiatedToken.GetComponent<PhotonView>().RPC("SyncMaterial", RpcTarget.OthersBuffered, "red");
            }

            //instantiatedToken.GetComponent<SelectToken>().SetCharacter(CharacterClassSelector(card.GetCharacterStats().name, TESTING_teamType, instantiatedToken));
        }
        else
        {

        }
        */
    }

    Character CharacterClassSelector(string characterName, TeamType tokenTeam, GameObject instantiatedToken)
    {
        return null;
        /*
        switch (characterName)
        {
            case "Cavalier":
                return new Cavalier(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Salmon":
                return new Salmon(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Fly":
                return new Fly(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Turtle":
                return new Turtle(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Mimic":
                return new Mimic(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Medusa":
                return new Medusa(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Mummy":
                return new Mummy(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Death Horseman":
                return new DeathHorseman(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Terracotta Warrior":
                return new TerracottaWarrior(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Magic Karp":
                return new MagicKarp(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Chicken":
                return new Chicken(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Dummy":
                return new Dummy(card.GetCharacterStats(), tokenTeam);
            case "The Gun":
                return new TheGun(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            case "Human Werewolf":
                return new HumanWerewolf(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
            default:
                Debug.LogError("NO CHARACTER RECOGNISED");
                return new Character(card.GetCharacterStats(), tokenTeam, instantiatedToken, null);
        }
        */

    }
}
