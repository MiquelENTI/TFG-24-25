using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerracottaWarrior : Character
{
    bool hasMoved = false;
    public TerracottaWarrior(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnEndTurn()
    {
        base.OnEndTurn();
        if (!hasMoved) 
        {
            stats.hp += 3;
        }
        hasMoved = false;
    }
    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);
        hasMoved = true;
    }
}
