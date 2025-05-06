using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Character
{
    int damageReduction = 3;


    public Turtle(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    protected override void OnAttacked(Character attacker)
    {
        int enemyDamage = attacker.GetCharacterStats().dmg;

        if (enemyDamage - damageReduction < 0)
        {
            enemyDamage = 0;
        }

        stats.hp -= enemyDamage;

        if (stats.hp <= 0)
        {
            OnDeath(attacker);
        }
    }

    //Implementació del so general per a cartes no identificades 

    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003006001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003006002, token.transform.position);
    }
}
