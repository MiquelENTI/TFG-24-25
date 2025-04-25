using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : Character
{
    public Samurai(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnPointsScoring(int pointsScored)
    {
        base.OnPointsScoring(pointsScored);

        stats.dmg += pointsScored;
    }

    public override void OnSpawn(CellNode cellToMove)
    {
        base.OnSpawn(cellToMove);
    }

    public override void OnDeath(Character attacker)
    {
        base.OnDeath(attacker);
    }
}
