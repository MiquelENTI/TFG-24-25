using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Character
{
    public Kamikaze(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnKillEnemy(Character enemy)
    {
        base.OnKillEnemy(enemy);

        ReceiveDamage(9999);
    }
}
