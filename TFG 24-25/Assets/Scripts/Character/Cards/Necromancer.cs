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

    protected override void OnKillEnemy(Character enemy)
    {
        int tileToSpawnZombie = enemy.GetOnTileId();
        base.OnKillEnemy(enemy);
        spawnManager.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, 28, PhotonNetwork.LocalPlayer.ActorNumber, tileToSpawnZombie, true);

    }
     //Implementació del so
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003029001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003029002, token.transform.position);
    }
}
