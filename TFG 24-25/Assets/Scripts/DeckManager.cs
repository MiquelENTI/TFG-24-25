using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] GameObject prefabCard;
    Queue<Cards> deck = new();

    private void Awake()
    {
        CreateDeck();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Pressed T");
            DrawCard();
        }
    }

    void CreateDeck()
    {
        ShuffleDeck();
    }

    public void DrawCard()
    {
        if (deck.Count == 0) { return; }
        GameObject instantiatedCard = PhotonNetwork.Instantiate(prefabCard.name, prefabCard.transform.localPosition, Quaternion.identity);
        instantiatedCard.GetComponent<DragableUIObject>().SetDragableUIObject(deck.Dequeue());
    }

    public void ShuffleDeck()
    {
        List<CharacterCard> tempList = TemporalCardDataBase.Instance.GetAllCharacters(2);
        List<Cards> cards =  new();
        foreach (Cards card in tempList) 
        {
            cards.Add(card);
        }
        Debug.Log("Number of Cards = " + cards.Count);

        Shuffle(cards);

        for (int i = 0; i < cards.Count; i++)
        {
            deck.Enqueue(cards[i]);
        }

        Debug.Log("Number of Cards in Deck = " + cards.Count);
    }

    private IList<T> Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }
}
