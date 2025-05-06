using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cavalier : Character
{
    bool hasAttacked = false;
    bool hasMoved = false;

    public Cavalier(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {

    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (hasMoved && !hasAttacked)
        {
            canAttack = true;
        }
        else if (hasAttacked && !hasMoved)
        {
            canMove = true;
        }

        if (!base.OnMovement(cellToMove)) // True - Movement is availabe and Character Does Not Attack. False - Movement is blocked or Character Attacks
        {
            return false;
        }
        hasMoved = true;
        return true;
    }

    public override void Attack(Character enemy)
    {
        base.Attack(enemy);
        hasAttacked = true;
    }
    public override void OnStartTurn()
    {
        base.OnStartTurn();

        hasAttacked = false;
        hasMoved = false;
    }

    protected override void OnDeath(Character attacker)
    {
        //DeckManager.Instance.BoardIntoHandDraw(teamType, stats.name);
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
