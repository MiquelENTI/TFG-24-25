using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Character
{
    bool canMove = true;
    public Turtle(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnStartTurn()
    {
        canMove = !canMove;
        if (canMove)
        {
            stats.movementsLeft = stats.numOfMovements;
        }
        else
        {
            stats.movementsLeft = 0;
        }
    }
}
