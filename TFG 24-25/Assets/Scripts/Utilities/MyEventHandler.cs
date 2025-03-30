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

    public UnityEvent samuraiEffectRed;
    public UnityEvent samuraiEffectBlue;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        moveToken = new UnityEvent<int, int>();
        spawnToken = new UnityEvent<int, int, bool>();

        samuraiEffectRed = new();
        samuraiEffectBlue = new();

        moveToken.AddListener((int cellId, int characterId) =>
        {
            photonView.RPC("RPC_MoveToken", RpcTarget.AllBuffered, cellId, characterId);
            Debug.Log("TRIGGERED MOVE");
        });

        spawnToken.AddListener((int cellId, int characterId, bool bypassSpawn) =>
        {
            photonView.RPC("RPC_SpawnToken", RpcTarget.AllBuffered, cellId, characterId, bypassSpawn);
        });
        
    }

    private void InvokeMoveToken(int cellId, int characterId)
    {
        Debug.Log("INVOKE");

        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
        Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

        character.OnMovement(cellToMove);
    }

    private void InvokeSpawnToken(int cellId, int characterId, bool bypassSpawn)
    {
        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
        Character character = CharactersManager.Instance.GetCharacterInBoardById(characterId);

        if (bypassSpawn)
        {
            character.BypassSpawn(cellToMove);
        }
        else
        {
            character.OnSpawn(cellToMove);
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

    public void StartSamuraiEffect(TeamType teamType)
    {
        switch (teamType)
        {
            case TeamType.RED:
                {
                    samuraiEffectRed.AddListener(() =>
                    {
                        photonView.RPC("RPC_ActivateSamuraiEffect", RpcTarget.All, teamType);
                        Debug.Log("SAMURAI RED ACTIVATED");
                    });
                    break;
                }
            case TeamType.BLUE:
                samuraiEffectBlue.AddListener(() =>
                {
                    photonView.RPC("RPC_ActivateSamuraiEffect", RpcTarget.All, teamType);
                    Debug.Log("SAMURAI RED ACTIVATED");
                });
                break;
            default:
                break;
        }
    }

    public void DeactivateSamuraiEffect(TeamType teamType)
    {
        switch (teamType)
        {
            case TeamType.RED:
                samuraiEffectRed.RemoveAllListeners();
                break;
            case TeamType.BLUE:
                samuraiEffectBlue.RemoveAllListeners();
                break;
        }
    }    

    [PunRPC]
    public void RPC_ActivateSamuraiEffect(TeamType teamType)
    {
        CharactersManager.Instance.GetCharactersByColor(teamType).ForEach(character =>
        {
            if (character.GetCharacterStats().getName() == "Samurai")
            {
                character.Effect();
            }
        });
    }

    public void InvokeSamuraiEffect(TeamType teamType)
    {
        switch (teamType)
        {
            case TeamType.RED:
                samuraiEffectRed.Invoke();
                break;
            case TeamType.BLUE:
                samuraiEffectBlue.Invoke();
                break;
            default:
                break;
        }
    }
}

