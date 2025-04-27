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
        if (!base.OnMovement(cellToMove))
        {
            canAttack = true;
            return false;
        }

        return true;
    }

    public override void Attack(Character enemy)
    {
        base.Attack(enemy);
        canMove = true;
    }

    public override void OnDeath(Character attacker)
    {
        Debug.Log("ENTERED ON DEATH");
        //DeckManager.Instance.BoardIntoHandDraw(teamType, stats.name);
        base.OnDeath(attacker);
    }
     //Implementació del so general per a cartes no identificades 
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003016002, token.transform.position);
    }
}
