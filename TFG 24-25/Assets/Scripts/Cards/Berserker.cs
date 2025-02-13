using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker : Character
{
    int originalAttack = 6;
    int boostedAttack = 8;


    public Berserker(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
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
