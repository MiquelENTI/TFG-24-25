using DilmerGames.Core.Singletons;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : Singleton<CharactersManager>
{
    Dictionary<int, Character> charactersOnBoard;
    int currentId = 0;

    private void Awake()
    {
        charactersOnBoard = new();
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var character in charactersOnBoard.Values)
        //{
        //    Debug.Log(character.GetId().ToString());
        //}
    }

    public void AddCharacter(Character character)
    {
        charactersOnBoard.Add(currentId,character);
        character.SetId(currentId);
        currentId++;
    }
    public void RemoveCharacter(int id)
    {
        charactersOnBoard.Remove(currentId);
    }

    public Character GetCharacterInBoardById(int id)
    {
        return charactersOnBoard[id];
    }
}
