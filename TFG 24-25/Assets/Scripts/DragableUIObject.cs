using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

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

    void Start() { }

    public void SetDragableUIObject(Cards newCard)
    {
        card = newCard;
        transform.parent = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHand" : "RedHand").transform;

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
            CreateCharacterOrActivateEffect();

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

            instantiatedToken.GetComponent<SelectToken>().SetCharacter(new Character(card.GetCharacterStats(), tokenTeam, instantiatedToken, null));
        }
        else
        {
           
        }
    }
}
