using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHold : MonoBehaviour
{
    TeamType teamType;
    List<GameObject> cards;
    float cardSize;

    void Start()
    {
        cards = new List<GameObject>();
        cardSize = 1f;
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
        card.transform.localRotation = Quaternion.Euler(0,0,0);
        ReorganizeCards();
    }

    void ReorganizeCards()
    {
        float padding = 0.1f;
        float totalLength = cards.Count * cardSize + (cards.Count-1)*padding;

        float halfLength = totalLength / 2;

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.position = new Vector3(
                transform.position.x - halfLength + (i * (cardSize + padding)),
                transform.position.y,
                transform.position.z);

        }
    }
}
