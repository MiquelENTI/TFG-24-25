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
      protected override void OnSpawnSFX()
    {
        base.OnSpawnSFX();
        SoundManager.Instance.PlaySFX(013, token.transform.position);
        Debug.Log("Distancia: " + Vector3.Distance(Camera.main.transform.localPosition, token.transform.position));
        Debug.Log(token.transform.position);
    }
}
