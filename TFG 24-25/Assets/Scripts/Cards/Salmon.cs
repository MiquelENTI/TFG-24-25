using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salmon : Character
{
    public Salmon(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnKillEnemy(Character enemy)
    {
        base.OnKillEnemy(enemy);

        BypassMovement(enemy.GetOnTileId());
        ReceiveDamage(1);
    }
     
     
     //Implementació del so en relacio al SoundManager 
    protected override void OnMovementSFX()
    {

    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(000, token.transform.position);
    }
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(000, token.transform.position);
    }

}
 
