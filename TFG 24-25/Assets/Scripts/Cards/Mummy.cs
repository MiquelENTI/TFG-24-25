using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : Character
{
    public Mummy(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);
        DecreaseDamage(1);
        ReceiveDamage(1);
    }
}
