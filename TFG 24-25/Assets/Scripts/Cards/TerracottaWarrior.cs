using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerracottaWarrior : Character
{
    int maxHp;
    bool hasMoved = false;
    public TerracottaWarrior(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
        maxHp = newStats.hp;
    }

    public override void OnStartTurn()
    {
        Debug.Log("AAAAAAAAAAAAASDASDASDASDASDASD");
        base.OnStartTurn();
        if (!hasMoved) 
        {
            stats.hp += 1;
            if (stats.hp > maxHp)
            {
                stats.hp = maxHp;
            }
        }
        hasMoved = false;
    }
    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);
        hasMoved = true;
    }
}
