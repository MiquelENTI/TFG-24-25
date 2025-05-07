using System;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Character
{
    private CellNode cellToMove;
    private int splashDamage;

    private Dictionary<CellConnection, Tuple<CellConnection, CellConnection>> translateDirection = new()
    {
        { CellConnection.UP, Tuple.Create(CellConnection.UPLEFT, CellConnection.UPRIGHT) },
        { CellConnection.LEFT, Tuple.Create(CellConnection.UPLEFT, CellConnection.DOWNLEFT) },
        { CellConnection.RIGHT, Tuple.Create(CellConnection.DOWNRIGHT, CellConnection.UPRIGHT) },
        { CellConnection.DOWN, Tuple.Create(CellConnection.DOWNLEFT, CellConnection.DOWNRIGHT) },
    };

    public Dragon(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        splashDamage = stats.dmg;
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        this.cellToMove = cellToMove;
        Debug.Log("CellToMove: " + cellToMove.GetId().ToString());

        if (!base.OnMovement(cellToMove))
        { return false; }


        return true;
    }

    public override void Attack(Character enemy)
    {
        base.Attack(enemy);

        CellNode onTileNode = CellNodeManager.Instance.GetNodeById(onTile);

        CellConnection attackDirection = onTileNode.GetDirectionToCellByRange(cellToMove.GetId(), stats.attackRange).Item2;

        Debug.Log("AttackDirection: " + attackDirection.ToString());

        Tuple<CellConnection, CellConnection> diagonals = translateDirection[attackDirection];

        if (onTileNode.CheckConnectionNode(diagonals.Item1))
        { 
            if (onTileNode.GetCellByDirection(diagonals.Item1).GetCharacter() != null)
            {
                onTileNode.GetCellByDirection(diagonals.Item1).GetCharacter().ReceiveDamage(this, splashDamage); 
            }
        }

        if (onTileNode.CheckConnectionNode(diagonals.Item2))
        {
            if (onTileNode.GetCellByDirection(diagonals.Item2).GetCharacter() != null)
            {
                onTileNode.GetCellByDirection(diagonals.Item2).GetCharacter().ReceiveDamage(this, splashDamage); 
            }
        }
    }
}
