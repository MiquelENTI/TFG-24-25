using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Charybdis : Character
{
    SpawnCardController spawnManager;
    int characterIdToSpawn;

    public Charybdis(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnCardController>();
    }

    public override bool OnSpawn(CellNode cellToMove)
    {
        if (cellToMove.GetCharacter() == null) { return false; }

        if (cellToMove.GetCharacter().GetTeamType() == teamType)
        {
            characterIdToSpawn = TemporalCardDataBase.Instance.GetTemporalStatsIdByName(cellToMove.GetCharacter().GetName());

            cellToMove.GetCharacter().DestroyCharacter();

            base.OnSpawn(cellToMove);

        }

        return true;
    }

    public override void OnDeath(Character attacker)
    {
        spawnManager.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, characterIdToSpawn, PhotonNetwork.LocalPlayer.ActorNumber, onTile, true);

        base.OnDeath(attacker);
    }
    
    //Implementació del so 
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003031001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003031002, token.transform.position);
    }

}
