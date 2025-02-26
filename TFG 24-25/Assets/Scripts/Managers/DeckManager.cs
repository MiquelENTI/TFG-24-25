using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] GameObject prefabCard;
    Queue<Cards> deck = new();

    [SerializeField] List<GameObject> cardsWarras = new();
    [SerializeField] Queue<GameObject> deckWarro = new();

    int teamTypeAlternator;

    private void Awake()
    {
        /*
        Shuffle(cardsWarras);
        /*for (int i = 0; i < cardsWarras.Count; i++)
        {
            deckWarro.Enqueue(cardsWarras[i]);
        }*/
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
            
            // DrawCardWarro();
            DrawCard();
        }

        
    }

    void CreateDeck()
    {
        GenerateInDeck(15, 10);
        ShuffleDeck();
    }

    public void DrawCard()
    {
        if (deck.Count == 0) { return; }
        GameObject instantiatedCard = PhotonNetwork.Instantiate(prefabCard.name, prefabCard.transform.localPosition, Quaternion.identity);


        GameObject cardHold = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold");
        instantiatedCard.transform.parent = cardHold.transform;
        cardHold.GetComponent<CardHold>().AddCardToHold(instantiatedCard);
        //instantiatedCard.GetComponent<DragableUIObject>().SetDragableUIObject(deck.Dequeue());
    }

    public void DrawCardWarro()
    {
        if (deckWarro.Count == 0) { return; }
        Debug.Log('1');
        GameObject temp = deckWarro.Dequeue();
        GameObject instantiatedCard = PhotonNetwork.Instantiate("Warro/"+temp.name, temp.transform.localPosition, Quaternion.identity);
        instantiatedCard.transform.parent = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHand" : "RedHand").transform;
    }

    public void TESTING_DrawCard()
    {
        if (deck.Count == 0) { return; }
        GameObject instantiatedCard = PhotonNetwork.Instantiate(prefabCard.name, prefabCard.transform.localPosition, Quaternion.identity);

        instantiatedCard.GetComponent<DragableUIObject>().TESTING_SetDragableUIObject(deck.Dequeue(), teamTypeAlternator % 2 == 0 ? TeamType.BLUE : TeamType.RED);
        teamTypeAlternator++;
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

    void GenerateInDeck(int characterId, int amount)
    {
        for (int i = 0; i < amount; i++) 
        {
            deck.Enqueue(TemporalCardDataBase.Instance.GetCharacter(characterId));
        }
    }
}
