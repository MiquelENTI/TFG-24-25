using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharactersManager : Singleton<CharactersManager>
{
    Dictionary<int, Character> charactersOnBoard;
    int currentId = 0;

    List<Character> charactersToRemove; 

    [SerializeField] bool bypassMana;

    private void Awake()
    {
        charactersOnBoard = new();
        charactersToRemove = new List<Character>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddCharacter(Character character)
    {
        charactersOnBoard.Add(currentId,character);
        character.SetId(currentId);

        currentId++;
    }
    public void RemoveCharacters()
    {
        if (charactersToRemove.Count == 0) return;

        foreach (Character character in charactersToRemove)
        {
            int id = character.GetId();
            CellNodeManager.Instance.GetNodeById(charactersOnBoard[id].GetOnTileId()).RemoveCharacter();
            Destroy(charactersOnBoard[id].GetToken());
            charactersOnBoard.Remove(id);
        }

        charactersToRemove.Clear();
        
        //ClearMissingCharacterBoard();
    }

    public void AddToRemoveList(Character characterToRemove)
    {
        foreach (Character character in charactersToRemove)
        {
            if (character.GetId() == characterToRemove.GetId())
            {
                Debug.Log("FOUND A CHARACTER WITH SAME ID THAT HAS TO BE REMOVED");
                return;
            }
        }
        charactersToRemove.Add(characterToRemove);
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

    public void ActivateEndTurnCharactersByColor(bool isBlue)
    {
        TeamType type = isBlue ? TeamType.BLUE : TeamType.RED;

        var temp = GetCharactersByColor(type);

        foreach (Character character in temp)
        {
            character.OnEndTurn();
        }
    }

    public void ActivateStartTurnCharactersByColor(bool isBlue)
    {
        TeamType type = isBlue ? TeamType.BLUE : TeamType.RED;

        var temp = GetCharactersByColor(type);

        foreach (Character character in temp)
        {
            character.OnStartTurn();
        }
    }

    public bool GetBypassMana()
    { return bypassMana; }
}
