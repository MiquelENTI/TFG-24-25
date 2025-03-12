using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGun : Character
{
    CellNode currentCellToMove;

    CellNode cellToMoveAfterAttack;
    CellConnection directionToMove;
    bool moveAfterAttack = false;
    public TheGun(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void Attack(Character enemy)
    {
        base.Attack(enemy);
        foreach (CellConnection direction in directions)
        {
            CellNode cellOnTile = CellNodeManager.Instance.GetNodeById(onTile);
            if (!cellOnTile.CheckConnectionNode(direction))
            {
                continue;
            }

            if (cellOnTile.GetCellByDirection(direction).GetId() == currentCellToMove.GetId())
            {
                CellNode cellToMove = cellOnTile.GetCellByInverseDirection((int)direction);
                
                if (cellToMove == null) { return; }

                if (!cellToMove.IsOccupied())
                {
                    moveAfterAttack = true;
                    directionToMove = direction;
                }
            }
        }
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        currentCellToMove = cellToMove;
        
        if (moveAfterAttack)
        {
            BypassMovement(CellNodeManager.Instance.GetNodeById(onTile).GetCellByInverseDirection((int)directionToMove).GetId());
            moveAfterAttack = false;
        }

        return true;
    }
}
