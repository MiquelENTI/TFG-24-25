using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] GameObject prefabCard;
    [SerializeField] Queue<int> deck = new();
    [SerializeField] List<int> charactersToExclude;

    int amountOfCardCopies = 2;

    int teamTypeAlternator;

    private void Awake()
    {
        charactersToExclude = new() 
        {
            6, // Turtle, Problems with its effect
            7, // Mimic, Problems with its effect
            50, // Prototype Cards
            51,// Prototype Cards
            52,// Prototype Cards
            53,// Prototype Cards
            54,// Prototype Cards
            55,// Prototype Cards
            56,// Prototype Cards
            57,// Prototype Cards
            58,// Prototype Cards
            59,// Prototype Cards
            60,// Prototype Cards
            61,// Prototype Cards
            62, // Prototype Cards
            
            -1, //Dummy
        };
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
        ShuffleDeck();
    }

    public void DrawCard()
    {
        if (deck.Count == 0) { return; }
        GameObject instantiatedCard = PhotonNetwork.Instantiate(prefabCard.name, prefabCard.transform.localPosition, Quaternion.identity);
        instantiatedCard.transform.GetChild(0).GetComponent<CardGameObject>().SetCharacterToSpawnId(deck.Dequeue());

        GameObject cardHold = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold");
        instantiatedCard.transform.parent = cardHold.transform;
        cardHold.GetComponent<CardHold>().AddCardToHold(instantiatedCard);
    }

    //public void TESTING_DrawCard()
    //{
    //    if (deck.Count == 0) { return; }
    //    GameObject instantiatedCard = PhotonNetwork.Instantiate(prefabCard.name, prefabCard.transform.localPosition, Quaternion.identity);

    //    instantiatedCard.GetComponent<DragableUIObject>().TESTING_SetDragableUIObject(deck.Dequeue(), teamTypeAlternator % 2 == 0 ? TeamType.BLUE : TeamType.RED);
    //    teamTypeAlternator++;
    //}

    public void ShuffleDeck()
    {
        List<int> characterIds = TemporalCardDataBase.Instance.GetAllCharacters(amountOfCardCopies);

        for (int i = 0; i < charactersToExclude.Count; i++)
        {
            for (int j = 0; j < amountOfCardCopies; j++)
            {
                characterIds.Remove(charactersToExclude[i]);
            }
            Debug.Log("Character To Remove: " + charactersToExclude[i]);
        }

        Shuffle(characterIds);

        for (int i = 0; i < characterIds.Count; i++)
        {
            deck.Enqueue(characterIds[i]);
            Debug.Log("CharacterId: " + characterIds[i]);
        }

        Debug.Log("Number of Cards in Deck = " + deck.Count);
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
