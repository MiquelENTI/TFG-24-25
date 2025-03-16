using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cavalier : Character
{
    // This can move and attack in the same turn.

    public Cavalier(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {

    }

    public override bool OnMovement(CellNode cellToMove)
    {
        canAttack = true;

        if (!base.OnMovement(cellToMove))
        { return false; }

        return true;
    }

    public override void OnDeath(Character attacker)
    {
        // Add this character to the hand WIP
        base.OnDeath(attacker);
        //stats.manaCost++;
    }
    /*Implementació del so en relacio al SoundManager 
    protected override void OnMovementSFX()
    {

    }
    protected override void AttackSFX()
    {
        
    }
    protected override void OnSpawnSFX()
    {
        
    }*/
}
