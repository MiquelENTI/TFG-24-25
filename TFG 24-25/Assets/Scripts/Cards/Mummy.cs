using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : Character
{
    public Mummy(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        DecreaseDamage(1);
        ReceiveDamage(1);

        return true;
    }
}
