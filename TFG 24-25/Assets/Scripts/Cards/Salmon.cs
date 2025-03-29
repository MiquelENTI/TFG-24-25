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

        if (enemy.GetName() != "Charybdis")
        {
            BypassMovement(enemy.GetOnTileId());
            ReceiveDamage(1);
        }
    }
     

}
 
