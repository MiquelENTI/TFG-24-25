using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Character
{
    public Kamikaze(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnKillEnemy(Character enemy)
    {
        base.OnKillEnemy(enemy);

        ReceiveDamageSelf(9999);
    }
}
