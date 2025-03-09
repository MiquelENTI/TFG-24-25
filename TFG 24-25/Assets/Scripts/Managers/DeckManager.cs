using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public enum DeckMode { NONE, WHITELIST, BLACKLIST, WORKING }
public class DeckManager : Singleton<DeckManager>
{   
    [SerializeField] DeckMode deckmode = DeckMode.NONE;

    [SerializeField] GameObject prefabCard;
    [SerializeField] Queue<int> deck = new();
    [SerializeField] List<int> characterBlackList;
    [SerializeField] List<int> characterWhiteList;
    [SerializeField] List<int> characterWorkingList;



    int amountOfCardCopies = 2;

    int teamTypeAlternator;

    private void Awake()
    {
        characterBlackList = new() 
        {
            6,  // Turtle, Problems with its effect
            7,  // Mimic, Problems with its effect
            50, // Prototype Cards
            51, // Prototype Cards
            52, // Prototype Cards
            53, // Prototype Cards
            54, // Prototype Cards
            55, // Prototype Cards
            56, // Prototype Cards
            57, // Prototype Cards
            58, // Prototype Cards
            59, // Prototype Cards
            60, // Prototype Cards
            61, // Prototype Cards
            62, // Prototype Cards
            
            -1, //Dummy
        };

        characterWhiteList = new()
        {
            39
        };

        characterWorkingList = new()
        {
            16,
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

        List<int> characterIdsBlackList = characterIds;
        List<int> characterIdsWhiteList = new();
        List<int> characterIdsWorkingList = new();

        if (deckmode != DeckMode.NONE)
        {
            for (int i = 0; i < amountOfCardCopies; i++)
            {
                if (deckmode == DeckMode.BLACKLIST)
                {
                    for (int j = 0; j < characterBlackList.Count; j++)
                    {
                        characterIdsBlackList.Remove(characterBlackList[j]);
                        //Debug.Log("Character To Remove: " + characterBlackList[i]);
                    }
                }
                else if (deckmode == DeckMode.WHITELIST)
                {
                    for (int j = 0; j < characterWhiteList.Count; j++)
                    {
                        characterIdsWhiteList.Add(characterWhiteList[j]);
                        //Debug.Log("Character To Add: " + characterWhiteList[i]);
                    }
                }
                else if (deckmode == DeckMode.WORKING)
                {
                    for (int j = 0; j < characterWorkingList.Count; j++)
                    {
                        characterIdsWorkingList.Add(characterWorkingList[j]);
                        //Debug.Log("Character To Add: " + characterWorkingList[i]);
                    }
                }
            }

            switch (deckmode)
            {
                case DeckMode.WHITELIST:
                    {
                        characterIds.Clear();
                        characterIds = characterIdsWhiteList;
                    }
                    break;
                case DeckMode.BLACKLIST:
                    {
                        characterIds.Clear();
                        characterIds = characterIdsBlackList;
                    }
                    break;
                case DeckMode.WORKING:
                    {
                        characterIds.Clear();
                        characterIds = characterIdsWorkingList;
                    }
                    break;
            }
        }

        Shuffle(characterIds);

        for (int i = 0; i < characterIds.Count; i++)
        {
            deck.Enqueue(characterIds[i]);
            //Debug.Log("CharacterId: " + characterIds[i]);
        }

        //Debug.Log("Number of Cards in Deck = " + deck.Count);
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
