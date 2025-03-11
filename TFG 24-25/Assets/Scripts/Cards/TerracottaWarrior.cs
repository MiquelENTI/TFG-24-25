using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerracottaWarrior : Character
{
    public TerracottaWarrior(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }
    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);
    }

    public override void OnPointsScoring(int pointsScored)
    {
        base.OnPointsScoring(pointsScored);

        Heal(pointsScored);
    }
}
