using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Character
{
    public Giant(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        List<CellNode> surrondingCells = cellToMove.GetSurrondingCells();

        foreach (CellNode surrondingCell in surrondingCells)
        {
            if (surrondingCell.GetCharacter() != null)
            {
                surrondingCell.GetCharacter().ReceiveDamage(this, 2);
            }
        }

        return true;
    }
     
     //Implementació del so
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003037001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003037002, token.transform.position);
    }
}
