using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Character
{
    public Fly(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        cellToMove.PrintStatus();
        if (!base.OnMovement(cellToMove))
        { return false; }

        return true;
    }
    //Implementació del so general per a cartes no identificades 
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003005001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003005001, token.transform.position);
    }
}
