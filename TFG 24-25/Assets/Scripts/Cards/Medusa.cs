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
  
  /*Implementació del so en relacio al SoundManager 
    protected override void OnMovementSFX()
    {

    }
    protected override void AttackSFX()
    {
        
    }
    protected override void OnSpawnSFX()
    {
        
    }*/

}
