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

    public override void OnSpawn(CellNode cellToMove)
    {
        if (cellToMove.GetCharacter() == null) { return; }

        if (cellToMove.GetCharacter().GetTeamType() == teamType)
        {
            characterIdToSpawn = TemporalCardDataBase.Instance.GetTemporalStatsIdByName(cellToMove.GetCharacter().GetName());

            cellToMove.GetCharacter().DestroyCharacter();

            base.OnSpawn(cellToMove);
        }
    }

    public override void OnDeath(Character attacker)
    {
        spawnManager.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, characterIdToSpawn, PhotonNetwork.LocalPlayer.ActorNumber, onTile, true);

        base.OnDeath(attacker);
    }
}
