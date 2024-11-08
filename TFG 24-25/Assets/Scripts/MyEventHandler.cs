using DilmerGames.Core.Singletons;
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

            CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
            Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

            if (character.IsToSpawn())
            {
                if (cellToMove.CanCharacterSpawn(character))
                {
                    Debug.Log("SPAWN");
                    character.DisableSpawn();
                    cellToMove.SetCharacter(character);
                    cellToMove.SetOccupied(true);
                    character.SetOnTileId(cellId);

                    cellToMove.PrintStatus();
                    return;
                }
            }


            if (cellToMove.CheckMultipleNodeForCharacter(character.GetDirections(), character.GetId()))
            {
                Debug.Log("MOOOVE");
                CellNode previousNode = CellNodeManager.Instance.GetNodeById(character.GetOnTileId());
                previousNode.RemoveCharacter();
                previousNode.SetOccupied(false);
                cellToMove.SetCharacter(character);
                character.SetOnTileId(cellId);
                character.MoveToken(cellToMove.GetPosition());
            }
            else
            {
                character.MoveToken(CellNodeManager.Instance.GetNodeById(character.GetOnTileId()).GetPosition());
                Debug.Log("NO?");
                // return to last pos
            }
        });
    }
}
