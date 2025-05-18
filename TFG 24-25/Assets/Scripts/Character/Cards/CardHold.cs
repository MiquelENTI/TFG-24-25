using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CardHold : MonoBehaviour
{
    int currentId;

    List<GameObject> cards;
    Vector3 defaultCardRotation;
    float cardSize;

    PhotonView photonView;

    void Start()
    {
        cards = new List<GameObject>();
        cardSize = 0.1f;
        photonView = GetComponent<PhotonView>();

        currentId = 0;
    }

    public void AddCardToHold(GameObject card)
    {
        cards.Add(card);
        ReorganizeCards();
        card.transform.GetChild(0).GetComponent<CardGameObject>().SetCardId(currentId);
        currentId++;
    }

    public void ReorganizeCards()
    {
        float padding = 0.1f;
        float totalLength = cards.Count * cardSize + (cards.Count-1)*padding;

        float halfLength = totalLength / 2;

        for (int i = 0; i < cards.Count; i++)
        {
            MoveCard(cards[i], new Vector3(
                transform.position.x - halfLength + (i * (cardSize + padding)),
                transform.position.y,
                transform.position.z));
        }
    }
    
    private void MoveCard(GameObject gameObject, Vector3 newPosition)
    {
        gameObject.transform.GetChild(0).position = newPosition;
        gameObject.transform.GetChild(0).rotation = Quaternion.Euler(defaultCardRotation);
    }

    public void DestroyCard(int cardToDestroy)
    {
        photonView.RPC("DestroyCard_RPC", RpcTarget.All, cardToDestroy);
    }

    [PunRPC]
    public void RPC_DestroyCard(int cardToDestroy)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (TurnManagerScript.Instance.isPCVersion)
            {
                if (cards[i].transform.GetChild(0).GetComponent<CardGameObject>().GetCardId() == cardToDestroy)
                {
                    PhotonNetwork.Destroy(cards[i]);
                    cards.RemoveAt(i);
                    ReorganizeCards();
                    return;
                }
            }
            else
            {
                Debug.Log("Entered Destroy Card");
                if (cards[i].transform.childCount == 0) // WTF PK FUNCIONA? PK NO TE CHILDS QUAN EN ESCENA TE?
                {
                    Debug.Log("Destroying Card");
                    //PhotonNetwork.Destroy(cards[i]);
                    cards.RemoveAt(i);
                    ReorganizeCards();
                    return;
                }

                if (cards[i].transform.GetChild(0).GetComponent<CardGameObject>().GetCardId() != cardToDestroy)
                {
                    continue;
                }
            }
        }
    }

    public void DefaultCardRotation()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.rotation = Quaternion.Euler(defaultCardRotation);
        }
    }

    public void SetDefaultCardRotation(Vector3 defaultCardRotation)
    {
        this.defaultCardRotation = defaultCardRotation;
    }
}
