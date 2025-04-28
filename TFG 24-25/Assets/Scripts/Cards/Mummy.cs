using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : Character
{
    public Mummy(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        DecreaseDamage(1);
        ReceiveDamageSelf(1);

        return true;
    }
   
    //Implementació del so general per a cartes no identificades 
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003013001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003013002, token.transform.position);
    }
}
