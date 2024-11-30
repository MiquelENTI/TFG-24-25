using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salmon : Character
{
    public Salmon(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnKillEnemy(Character enemy)
    {
        base.OnKillEnemy(enemy);

        BypassMovement(enemy.GetOnTileId());
        ReceiveDamage(3);
    }
}
