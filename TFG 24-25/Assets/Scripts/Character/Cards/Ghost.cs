using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ghost : Character
{
    Dictionary<int, int> moveAcrossBoard = new Dictionary<int, int>();

    public Ghost(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        InitAdditionalMoves();
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (CheckIfMoveAcrossBoard(cellToMove) && playerStats.GetCurrentMana() >= stats.manaCost && !stats.stun && canMove)
        {
            playerStats.SubstractMana(stats.manaCost);
            BypassMovement(cellToMove.GetId());

            return true;
        }
        return base.OnMovement(cellToMove);
    }

    bool CheckIfMoveAcrossBoard(CellNode cellToMove)
    {
        if (!moveAcrossBoard.ContainsKey(onTile))
        { return false; }

        if (moveAcrossBoard[onTile] != cellToMove.GetId())
        { return false; }

        if (CellNodeManager.Instance.GetNodeById(moveAcrossBoard[onTile]).GetCharacter() != null)
        { return false; }

        return true;
    }


    void InitAdditionalMoves()
    {
        // Left to Right
        moveAcrossBoard.Add(0,5);
        moveAcrossBoard.Add(6,11);
        moveAcrossBoard.Add(12,17);
        moveAcrossBoard.Add(18,23);
        moveAcrossBoard.Add(24,29);
        moveAcrossBoard.Add(30,35);


        //Right to Left
        moveAcrossBoard.Add(5,0);
        moveAcrossBoard.Add(11,6);
        moveAcrossBoard.Add(17,12);
        moveAcrossBoard.Add(23,18);
        moveAcrossBoard.Add(29,24);
        moveAcrossBoard.Add(35,30);
    }
     //Implementació del so
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003018001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003018002, token.transform.position);
    }
}
