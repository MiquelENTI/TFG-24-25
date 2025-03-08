using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : Character
{
    public Hydra(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnAttack(Character attacker)
    {
        base.OnAttack(attacker);
        stats.dmg++;
    }
}
