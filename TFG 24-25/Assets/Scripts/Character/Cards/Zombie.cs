using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Character
{
    public Zombie(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnDeath(Character attacker)
    {
        //DeckManager.Instance.BoardIntoHandDraw(teamType, stats.name);
        base.OnDeath(attacker);
    }
}
