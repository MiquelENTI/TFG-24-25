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
        DeckManager.Instance.BoardIntoHandDraw(stats.name);
        base.OnDeath(attacker);
    }
    //Implementació del so 
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003002001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003002002, token.transform.position);
    }
}
