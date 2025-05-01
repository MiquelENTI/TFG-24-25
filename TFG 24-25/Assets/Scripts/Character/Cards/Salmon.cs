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
        Debug.Log("ENTERING EATING ENEMY");
        base.OnKillEnemy(enemy);
        if (enemy.GetName() != "Charybdis")
        {
            Debug.Log("EATING ENEMY");
            BypassMovement(enemy.GetOnTileId());
            ReceiveDamageSelf(1);
        }
    }
     

}
 
