using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Character
{
    bool canMove = true;
    public Turtle(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnStartTurn()
    {
        canMove = !canMove;
    }

    public override void OnMovement(CellNode cellToMove)
    {
        if (canMove)
        {
            base.OnMovement(cellToMove);
        }
    }
}
