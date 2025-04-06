using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mimic : Character
{
    public Mimic(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnAttacked(Character attacker)
    {
        base.OnAttacked(attacker);

        // Enemy draws a cards 
        attacker.ReceiveDamage(9999);
        ReceiveDamage(9999);
    }

   //Implementació del so 
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003007001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003007002, token.transform.position);
    }
}
