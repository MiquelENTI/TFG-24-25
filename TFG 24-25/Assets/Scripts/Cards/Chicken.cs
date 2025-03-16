using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Character
{
    int counterDamage = 5;
    public Chicken(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnAttack(Character attacker)
    {
        attacker.ReceiveDamage(counterDamage);
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
