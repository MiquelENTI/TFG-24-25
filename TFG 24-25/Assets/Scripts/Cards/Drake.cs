using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Drake : Character
{
    CellNode cellToMove;
    int splashDamage;

    Dictionary<CellConnection, Tuple<CellConnection, CellConnection>> translateDirection = new()
    {
        { CellConnection.UP, Tuple.Create(CellConnection.UPLEFT, CellConnection.UPRIGHT) },
        { CellConnection.LEFT, Tuple.Create(CellConnection.UPLEFT, CellConnection.DOWNLEFT) },
        { CellConnection.RIGHT, Tuple.Create(CellConnection.DOWNRIGHT, CellConnection.UPRIGHT) },
        { CellConnection.DOWN, Tuple.Create(CellConnection.DOWNLEFT, CellConnection.DOWNRIGHT) },
    };

    public Drake(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        splashDamage = stats.dmg;
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        this.cellToMove = cellToMove;

        return true;
    }

    public override void Attack(Character enemy)
    {
        base.Attack(enemy);

        CellNode onTileNode = CellNodeManager.Instance.GetNodeById(onTile);

        CellConnection attackDirection = onTileNode.GetDirectionToCellByRange(cellToMove.GetId(), stats.range).Item2;

        Tuple<CellConnection, CellConnection> diagonals = translateDirection[attackDirection];

        onTileNode.GetCellByDirection(diagonals.Item1).GetCharacter().ReceiveDamage(splashDamage);
        onTileNode.GetCellByDirection(diagonals.Item2).GetCharacter().ReceiveDamage(splashDamage);
    }
}
