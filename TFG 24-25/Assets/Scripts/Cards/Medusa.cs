using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : Character
{
    List<Character> enemiesAttacked = new();
    public Medusa(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void Attack(Character enemy)
    {
        enemy.ApplyStun();
        enemiesAttacked.Add(enemy);
        base.Attack(enemy);
    }

    public override void OnDeath(Character attacker)
    {
        foreach (Character enemy in enemiesAttacked)
        {
            enemy.RemoveStun();
        }

        base.OnDeath(attacker);
    }
}
