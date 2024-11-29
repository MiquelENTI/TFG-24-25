using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class MyEventHandler : Singleton<MyEventHandler>
{
    public UnityEvent<int, int> moveToken;

    private void Awake()
    {
        moveToken = new UnityEvent<int, int>();
        moveToken.AddListener((int cellId, int characterId) =>
        {
            // cellId --> Cell to Move
            // characterId --> Character thats moving

            Debug.Log("INVOKE");

            // IF NOT MY TURN -> RETURN;

            CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
            Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

            

            if (character.IsToSpawn())
            {
                character.OnSpawn(cellToMove);
                
                return;
            }
            
            character.OnMovement(cellToMove);
        });
    }
}
