using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Character
{
    int counterDamage = 16;
    public Chicken(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnAttack(Character attacker)
    {
        attacker.ReceiveDamage(counterDamage);
    }
}
