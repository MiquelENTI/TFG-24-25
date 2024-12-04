using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : Singleton<CharactersManager>
{
    Dictionary<int, Character> charactersOnBoard;
    int currentId = 0;

    // Fa falta?
    Dictionary<int, Sprite> charactersCardSprite;

    private void Awake()
    {
        charactersOnBoard = new();
        charactersCardSprite = new Dictionary<int, Sprite>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCharacter(Character character, Sprite cardSprite)
    {
        charactersOnBoard.Add(currentId,character);
        charactersCardSprite.Add(currentId, cardSprite);
        character.SetId(currentId);
        currentId++;
    }
    public void RemoveCharacter(int id)
    {
        Destroy(charactersOnBoard[id].GetToken());
        charactersOnBoard.Remove(id);
    }

    public Character GetCharacterInBoardById(int id)
    {
        return charactersOnBoard[id];
    }

    public List<Character> GetCharactersByColor(TeamType type)
    {
        List<Character> list = new List<Character>();

        foreach (Character character in charactersOnBoard.Values)
        {
            if (character.GetTeamType() == type)
            {
                list.Add(character);
            }
        }
        return list;
    }

    public void TriggerOnStartTurn()
    {
        foreach(Character character in charactersOnBoard.Values)
        {
            character.OnStartTurn();
        }
    }
    public void TriggerOnEndTurn()
    {
        Debug.Log("Trigger");
        foreach (Character character in charactersOnBoard.Values)
        {
            character.OnEndTurn();
        }
    }
}
