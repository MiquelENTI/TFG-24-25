using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Necromancer : Character
{
    SpawnCardController spawnManager;

    public Necromancer(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnCardController>();
    }

    public override void OnKillEnemy(Character enemy)
    {
        int tileToSpawnZombie = enemy.GetOnTileId();
        Debug.Log("TILE: " + tileToSpawnZombie);
        base.OnKillEnemy(enemy);
        Debug.Log("TILE: " + tileToSpawnZombie);
        spawnManager.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, 28, PhotonNetwork.LocalPlayer.ActorNumber, tileToSpawnZombie, true);

    }
}
