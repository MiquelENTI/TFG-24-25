using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Character
{
    public Fly(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        return true;
    }
}
