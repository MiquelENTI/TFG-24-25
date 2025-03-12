using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : Character
{
    public Hydra(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnAttack(Character attacker)
    {
        base.OnAttack(attacker);
        stats.dmg++;
    }
}
