using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Character
{
    public Fly(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);
    }
}
