using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mimic : Character
{
    public Mimic(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnAttack(Character attacker)
    {
        base.OnAttack(attacker);

        // Enemy draws a cards 
        attacker.ReceiveDamage(9999);
        ReceiveDamage(9999);
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
