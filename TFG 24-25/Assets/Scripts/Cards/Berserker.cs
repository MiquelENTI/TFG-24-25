using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : Character
{
    int originalAttack;
    int boostedAttack = 8;


    public Berserker(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
        originalAttack = newStats.dmg;
    }

    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);

        if (CellNodeManager.Instance.GetNodeById(onTile).GetScoreNode() == CellScoreType.NOPOINTS)
        { 
            stats.dmg = originalAttack;
            return; 
        }

        stats.dmg = boostedAttack;

    }
}
