using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : Character
{
    int originalAttack;
    int boostedAttack = 8;


    public Berserker(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        originalAttack = newStats.dmg;
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        if (CellNodeManager.Instance.GetNodeById(onTile).GetScoreNode() == CellScoreType.NOPOINTS)
        { 
            stats.dmg = originalAttack;
            return true; 
        }

        stats.dmg = boostedAttack;

        return true;
    }
}
