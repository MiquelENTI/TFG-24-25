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

    [SerializeField] List<int> possibleCharacterNamesToAssign = new List<int>();
    int randomCharacterName = 1;

    Vector3 initialPosition;

    public SpawnCardController gameController;

    TeamType TESTING_teamType;

    void Start()
    {
        GameObject spawnManagerGO = GameObject.Find("SpawnManager");
        if (spawnManagerGO != null)
        {
            gameController = spawnManagerGO.GetComponent<SpawnCardController>();
            if (gameController == null)
            {
                Debug.LogError("No se encontr� el componente CardGameController en SpawnManager!");
            }
        }
        else
        {
            Debug.LogError("No se encontr� GameObject con Tag 'SpawnManager' para el CardGameController!");
        }
        if (possibleCharacterNamesToAssign != null && possibleCharacterNamesToAssign.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleCharacterNamesToAssign.Count);
            randomCharacterName = possibleCharacterNamesToAssign[randomIndex];
            Debug.Log("Nombre de personaje seleccionado aleatoriamente: " + randomCharacterName);
        }
    }

    public void SetDragableUIObject(Cards newCard)
    {
        card = newCard;
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
            Debug.Log("Carta jugada, solicitando instanciar personaje: " + randomCharacterName);
            RequestCharacterInstantiation();
            Destroy(gameObject);
        }
        else
        {
            transform.position = initialPosition;
        }
    }


    void RequestCharacterInstantiation()
    {
        if (gameController != null)
        {
            Debug.Log($"[DragableUIObject] Player ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber} requesting instantiation of character ID: {randomCharacterName}");
            gameController.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, "TokenPrefabName", randomCharacterName, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            Debug.LogError("GameController no asignado en DragableUIObject!");
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

            instantiatedToken.GetComponent<TokenGameObject>().SetCharacter(CharacterClassSelector(card.GetCharacterStats().name, tokenTeam, instantiatedToken));
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

        instantiatedToken.GetComponent<TokenGameObject>().SetCharacter(CharacterClassSelector(card.GetCharacterStats().name, tokenTeam, instantiatedToken));
    }

    public void TESTING_CreateCharacterOrActivateEffect()
    {
        
        if (card.IsCharacter())
        {
            Vector3 spawnPosition = new Vector3(2.16f, 0.75f, PhotonNetwork.IsMasterClient ? -3.95f : -0.5f);
            GameObject instantiatedToken = PhotonNetwork.Instantiate(PhotonNetwork.IsMasterClient ? token.name : token2.name, spawnPosition, Quaternion.identity);

            Renderer tokenRenderer = instantiatedToken.GetComponent<Renderer>();

            //instantiatedToken.GetComponent<SelectToken>().SetCharacter(CharacterClassSelector(card.GetCharacterStats().name, TESTING_teamType, instantiatedToken));
        }
        else
        {

        }
        
    }
}
