using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Character
{
    bool canMove = true;
    public Turtle(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnStartTurn()
    {
        canMove = !canMove;
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        Debug.Log("CANMOVE: " + canMove);

        stats.PrintStats();

        if (canMove)
        {
            if (!base.OnMovement(cellToMove))
            { return false; }
        }
        else
        {
            MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
        }

        return true;
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
