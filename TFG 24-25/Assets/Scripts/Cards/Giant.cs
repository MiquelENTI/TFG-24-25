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
}
