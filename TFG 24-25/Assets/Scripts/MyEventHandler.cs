using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class MyEventHandler : Singleton<MyEventHandler>
{
    public UnityEvent<int, int> moveToken;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        moveToken = new UnityEvent<int, int>();

        moveToken.AddListener((int cellId, int characterId) =>
        {
            InvokeMoveToken(cellId, characterId);

            photonView.RPC("RPC_MoveToken", RpcTarget.Others, cellId, characterId);
        });
    }

    private void InvokeMoveToken(int cellId, int characterId)
    {
        Debug.Log("INVOKE");

        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
        cellToMove.PrintStatus();
        Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

        if (character.IsToSpawn())
        {
            character.OnSpawn(cellToMove);
            return;
        }

        character.OnMovement(cellToMove);
    }

    [PunRPC]
    public void RPC_MoveToken(int cellId, int characterId)
    {
        InvokeMoveToken(cellId, characterId);
    }
}

