using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lacus : Character
{
    CellConnection direction = CellConnection.UP;

    public Lacus(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();

        CellNode currentCell = CellNodeManager.Instance.GetNodeById(onTile);

        if (stats.stun)
        { return; }

        if (!currentCell.CheckConnectionNode(direction))
        {
            DestroyCharacter();
            return;
        }

        Character infrontCharacter = currentCell.GetCellByDirection(direction).GetCharacter();
        if (infrontCharacter == null)
        {
            BypassMovement(currentCell.GetCellByDirection(direction).GetId());
            return; 
        }

        if (infrontCharacter.GetTeamType() == teamType)
        {
            return;
        }
        else
        {
            if (canAttack)
            {
                Attack(infrontCharacter);
            }
        }
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
        if (playerStats.GetCurrentMana() >= stats.manaCost && !stats.stun)
        {
            Vector2 diff = cellToMove.positionInGrid - CellNodeManager.Instance.GetNodeById(onTile).positionInGrid;

            diff.x = Mathf.Abs(diff.x);
            diff.y = Mathf.Abs(diff.y);

            if (diff.x > stats.movementRange || diff.y > stats.movementRange)
            {
                Debug.Log("ENTERED IF RANGE");
                return false;
            }

            if (diff.x == 0.0f || diff.y == 0.0f)
            {
                direction = CellNodeManager.Instance.GetNodeById(onTile).GetDirectionToCellByRange(cellToMove.GetId(), 1).Item2;
                Debug.Log("CURRENT DIRECTION: " + direction.ToString());
                playerStats.SubstractMana(stats.manaCost);
                return true;
            }
        }
        
        return false;
    }
}
