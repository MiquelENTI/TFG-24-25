using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CardHold : MonoBehaviour
{
    [SerializeField] TeamType teamType;
    List<GameObject> cards;
    float cardSize;

    public GameObject TEMP;

    PhotonView photonView;

    int currentId;

    GameObject playerCam;

    Vector3 defaultCardRotation;

    void Start()
    {
        cards = new List<GameObject>();
        cardSize = 0.1f;
        photonView = GetComponent<PhotonView>();

        currentId = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetTeamType(TeamType teamType)
    {
        this.teamType = teamType;
    }
    public TeamType GetTeamType()
    {
        return teamType;
    }

    public void AddCardToHold(GameObject card)
    {
        cards.Add(card);
        ReorganizeCards();
        card.transform.GetChild(0).GetComponent<CardGameObject>().SetCardId(currentId);
        currentId++;

        //Debug.Log("CardID: " + card.transform.GetChild(0).GetComponent<CardGameObject>().GetCardId() + " CurrentID: " + currentId);
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
    
    void MoveCard(GameObject gameObject, Vector3 newPosition)
    {
        gameObject.transform.GetChild(0).position = newPosition;
        gameObject.transform.GetChild(0).rotation = Quaternion.Euler(defaultCardRotation);
    }

    public void DestroyCard(int cardToDestroy)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (TurnManagerScript.Instance.isPCVersion)
            {
                if (cards[i].transform.GetChild(0).GetComponent<CardGameObject>().GetCardId() == cardToDestroy)
                {
                    Destroy(cards[i]);
                    cards.RemoveAt(i);
                    ReorganizeCards();
                    return;
                }
            }
            else
            {
                if (cards[i].transform.childCount == 0) // WTF PK FUNCIONA? PK NO TE CHILDS QUAN EN ESCENA TE?
                {
                    Destroy(cards[i]);
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

    public void LookToPlayerCam()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.GetChild(0).LookAt(playerCam.transform.position);
        }
    }

    public void DefaultCardRotation()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.rotation = Quaternion.Euler(defaultCardRotation);
        }
    }

    public void AssignPlayerCam(GameObject playerCam)
    {
        this.playerCam = playerCam; 
    }

    public void SetDefaultCardRotation(Vector3 defaultCardRotation)
    {
        this.defaultCardRotation = defaultCardRotation;
    }

    public Camera GetPlayerCamera()
    { return playerCam.GetComponent<Camera>(); }
}
