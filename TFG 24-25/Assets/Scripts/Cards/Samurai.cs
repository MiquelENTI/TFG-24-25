using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : Character
{
    int bonusDmg = 0;
    int maxDmg = 8;

    public Samurai(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnSpawn(CellNode cellToMove)
    {
        base.OnSpawn(cellToMove);
        MyEventHandler.Instance.StartSamuraiEffect(teamType);
    }

    public override void OnDeath(Character attacker)
    {
        base.OnDeath(attacker);

        MyEventHandler.Instance.DeactivateSamuraiEffect(teamType);
    }

    public override void Effect()
    {
        base.Effect();

        if (bonusDmg >= maxDmg)
        { return; }

        bonusDmg++;

        stats.dmg++;
    }
}
