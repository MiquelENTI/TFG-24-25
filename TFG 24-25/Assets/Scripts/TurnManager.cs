using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    bool isBlueTurn;

    void Start()
    {
        
    }
    void Update()
    {
        
    }


    bool IsBlueTurn()
    { return isBlueTurn; }

    void EndTurn()
    {
        isBlueTurn = !isBlueTurn;
    }

    void ResetTokenThings()
    {
        foreach (Character character in CharactersManager.Instance.GetCharactersByColor((TeamType)(isBlueTurn ? 1 : 0))) // 1 = BLUE, 0 = RED
        {
            character.ResetStats();
        }
    }
}
