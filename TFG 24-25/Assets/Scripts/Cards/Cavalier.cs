using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cavalier : Character
{
    // This can move and attack in the same turn.

    public Cavalier(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnMovement(CellNode cellToMove)
    {
        canAttack = true;
        base.OnMovement(cellToMove);
    }

    public override void OnDeath(Character attacker)
    {
        // Add this character to the hand WIP
        base.OnDeath(attacker);
        stats.manaCost++;
    }
}
