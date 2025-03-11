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

    public UnityEvent samuraiEffectRed;
    public UnityEvent samuraiEffectBlue;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        moveToken = new UnityEvent<int, int>();

        samuraiEffectRed = new();
        samuraiEffectBlue = new();

        moveToken.AddListener((int cellId, int characterId) =>
        {
            photonView.RPC("RPC_MoveToken", RpcTarget.AllBuffered, cellId, characterId);
            Debug.Log("TRIGGERED MOVE");
        });

        
    }

    private void InvokeMoveToken(int cellId, int characterId)
    {
        Debug.Log("INVOKE");

        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(cellId);
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

