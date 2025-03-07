using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Character
{
    public Giant(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);

        List<CellNode> surrondingCells = cellToMove.GetSurrondingCells();

        foreach (CellNode surrondingCell in surrondingCells)
        {
            if (surrondingCell.GetCharacter() != null)
            {
                surrondingCell.GetCharacter().ReceiveDamage(2);
            }
        }
    }
}
