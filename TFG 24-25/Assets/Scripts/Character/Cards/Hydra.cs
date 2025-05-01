using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : Character
{
    public Hydra(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnAttacked(Character attacker)
    {
        base.OnAttacked(attacker);
        stats.dmg++;
        DisplayActionsManager.Instance.CreateBuffText(stats.dmg, true, token.transform.position);
    }
     
     //Implementació del so
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003027001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003027002, token.transform.position);
    }
}
