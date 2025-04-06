using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Character
{
    int counterDamage = 5;
    public Chicken(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnAttacked(Character attacker)
    {
        attacker.ReceiveDamage(counterDamage);

        base.OnAttacked(attacker);
    }

    //Implementació del so 
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003022001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003022002, token.transform.position);
    }
}
