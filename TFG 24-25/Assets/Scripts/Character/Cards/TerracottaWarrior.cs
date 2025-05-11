using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerracottaWarrior : Character
{
    public TerracottaWarrior(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }
    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        return true;
    }

    public override void OnPointsScoring(int pointsScored)
    {
        base.OnPointsScoring(pointsScored);

        Heal(pointsScored);
    }

    //Implementació del so
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003017001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003017002, token.transform.position);
    }
}
