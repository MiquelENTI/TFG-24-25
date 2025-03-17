using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : Character
{
    List<Character> enemiesAttacked = new();
    public Medusa(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
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
            if (enemy != null)
            {
                enemy.RemoveStun();
            }
        }

        base.OnDeath(attacker);
    }
  
  //Implementació del so general per a cartes no identificades 
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003010001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003010002, token.transform.position);
    }
}
