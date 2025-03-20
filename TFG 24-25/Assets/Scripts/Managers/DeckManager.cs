using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;

public enum DeckMode { NONE, WHITELIST, BLACKLIST, WORKING }
public class DeckManager : Singleton<DeckManager>
{   
    [SerializeField] DeckMode deckmode = DeckMode.NONE;
    [SerializeField] int amountOfCopies = 2;
    [SerializeField] GameObject prefabCard;
    [SerializeField] Queue<int> deck = new();
    [SerializeField] List<int> characterBlackList;
    [SerializeField] List<int> characterWhiteList;
    [SerializeField] List<int> characterWorkingList;



    

    int teamTypeAlternator;

    private void Awake()
    {
        characterBlackList = new() 
        {
            //6,  // Turtle, Problems with its effect
            //7,  // Mimic, Problems with its effect
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

        //characterWhiteList = new();

        //characterWorkingList = new();
        CreateDeck();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DrawCard(true);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            DrawCard(false);
        }

    }

    void CreateDeck()
    {
        ShuffleDeck();
        SaveData.Instance.SaveDeck(deck);
    }

    [PunRPC]
    public void DrawCard(bool isBlue)
    {
        if (deck.Count == 0) { return; }

        GameObject cardHold = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold");

        GameObject instantiatedCard = PhotonNetwork.Instantiate(prefabCard.name, prefabCard.transform.localPosition, Quaternion.identity);
        instantiatedCard.transform.parent = cardHold.transform;
        instantiatedCard.transform.GetChild(0).GetComponent<CardGameObject>().Init();
        instantiatedCard.transform.GetChild(0).GetComponent<CardGameObject>().SetCardToSpawnId(deck.Dequeue());



        cardHold.GetComponent<CardHold>().AddCardToHold(instantiatedCard);
        cardHold.GetComponent<CardHold>().SetDefaultCardRotation(isBlue ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0));
    }

    public void BoardIntoHandDraw(TeamType type, string characterName)
    {
        if (type != TeamType.BLUE && PhotonNetwork.IsMasterClient)
        {
            return;
        }


        GameObject instantiatedCard = PhotonNetwork.Instantiate(prefabCard.name, prefabCard.transform.localPosition, Quaternion.identity);
        if (characterName == "Wizard" || characterName == "Cavalier")
        {
            instantiatedCard.transform.GetChild(0).GetComponent<CardGameObject>().SetCharacterToSpawnId_ManaCostIncrease(TemporalCardDataBase.Instance.GetTemporalStatsIdByName(characterName));
        }
        else // Zombie
        { 
            instantiatedCard.transform.GetChild(0).GetComponent<CardGameObject>().SetCardToSpawnId(TemporalCardDataBase.Instance.GetTemporalStatsIdByName(characterName));
        }

        GameObject cardHold = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold");
        instantiatedCard.transform.parent = cardHold.transform;
        cardHold.GetComponent<CardHold>().AddCardToHold(instantiatedCard);
        cardHold.GetComponent<CardHold>().SetDefaultCardRotation(PhotonNetwork.IsMasterClient ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0));
    }

    public void ShuffleDeck()
    {
        List<int> characterIds = TemporalCardDataBase.Instance.GetAllCharacters(amountOfCopies);

        List<int> characterIdsBlackList = new();
        List<int> characterIdsWhiteList = new();
        List<int> characterIdsWorkingList = new();

        for (int i = 0; i < characterIds.Count; i++)
        {
            characterIdsBlackList.Add(characterIds[i]);
        }

        if (deckmode != DeckMode.NONE)
        {
            for (int i = 0; i < amountOfCopies; i++)
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

    public void SetReplayDeck(Queue<int> replayDeck)
    {
        deck = replayDeck;
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
