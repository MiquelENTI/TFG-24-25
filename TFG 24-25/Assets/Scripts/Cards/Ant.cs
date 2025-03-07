using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : Character
{
    int originalAttack;
    int currentAttack;
    public Ant(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
        originalAttack = newStats.dmg;
    }

    public override void OnSpawn(CellNode cellToMove)
    {
        base.OnSpawn(cellToMove);

        CalculateCurrentAttack(cellToMove);
    }

    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);

        CalculateCurrentAttack(cellToMove);
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();

        CalculateCurrentAttack(CellNodeManager.Instance.GetNodeById(onTile));
    }

    void CalculateCurrentAttack(CellNode cellToMove)
    {
        List<CellNode> surrondingCells = cellToMove.GetSurrondingCells();

        currentAttack = originalAttack;
        foreach (CellNode surrondingCell in surrondingCells)
        {
            if (surrondingCell.GetCharacter() != null)
            {
                currentAttack++;
            }
        }
    }
}
