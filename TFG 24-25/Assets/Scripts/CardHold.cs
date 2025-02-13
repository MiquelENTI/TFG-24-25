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

    void Start()
    {
        cards = new List<GameObject>();
        cardSize = 1f;
        photonView = GetComponent<PhotonView>();
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
        //card.transform.localRotation = Quaternion.Euler(0,0,0);
        ReorganizeCards();
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
        gameObject.transform.position = newPosition;
        gameObject.transform.GetChild(0).localPosition = Vector3.zero;
        gameObject.transform.GetChild(1).localPosition = Vector3.zero;
    }
}
