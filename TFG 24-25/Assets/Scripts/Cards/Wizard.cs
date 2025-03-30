using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
    // This has infinite attack range. WIP


    public Wizard(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void Effect() 
    {
        
    }

    public override void OnDeath(Character attacker)
    {
        DeckManager.Instance.BoardIntoHandDraw(stats.name);
        base.OnDeath(attacker);
    }

    //Implementació del so general per a cartes no identificades 

    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003001001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003001002, token.transform.position);
    }

}
