using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.TextCore.Text;

public class MyEventHandler : Singleton<MyEventHandler>
{
    public UnityEvent<int, int> moveToken;
    public UnityEvent<int, int, bool> spawnToken;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        moveToken = new UnityEvent<int, int>();
        spawnToken = new UnityEvent<int, int, bool>();

        moveToken.AddListener((int cellId, int characterId) =>
        {
            photonView.RPC("RPC_MoveToken", RpcTarget.AllBuffered, cellId, characterId);
            photonView.RPC("RemoveCharacters_RPC", RpcTarget.AllBuffered);
            Debug.Log("TRIGGERED MOVE");
        });

        spawnToken.AddListener((int cellId, int characterId, bool bypassSpawn) =>
        {
            photonView.RPC("RPC_SpawnToken", RpcTarget.AllBuffered, cellId, characterId, bypassSpawn);
            photonView.RPC("RemoveCharacters_RPC", RpcTarget.AllBuffered);
        });
        
    }

    public void InvokeMoveToken(int cellId, int characterId)
    {
        Debug.Log("INVOKE");

        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
        Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

        character.OnMovement(cellToMove);

        //SaveData.Instance.SaveNewAction("M" + cellId + "/" + characterId);
    }

    public void InvokeSpawnToken(int cellId, int characterId, bool bypassSpawn)
    {
        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
        Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

        if (bypassSpawn)
        {
            character.BypassSpawn(cellToMove);
        }
        else
        {
            SaveData.Instance.BufferCheck(character.OnSpawn(cellToMove));
        } 
    }


    [PunRPC]
    public void RPC_MoveToken(int cellId, int characterId)
    {
        InvokeMoveToken(cellId, characterId);
    }

    [PunRPC]
    public void RPC_SpawnToken(int cellId, int characterId, bool bypassSpawn)
    {
        InvokeSpawnToken(cellId, characterId, bypassSpawn);
    }

    public void RemoveCharacters()
    {
        photonView.RPC("RemoveCharacters_RPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void RemoveCharacters_RPC()
    {
        CharactersManager.Instance.RemoveCharacters();
    }
}

