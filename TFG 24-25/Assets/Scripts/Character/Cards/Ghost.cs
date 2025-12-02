using System.Collections.Generic;
using UnityEngine;

public class Ghost : Character
{
    private Dictionary<int, int> moveAcrossBoard = new Dictionary<int, int>();

    public Ghost(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        InitAdditionalMoves();
    }

    public override bool OnMovement(TileNode cellToMove)
    {
        if (CheckIfMoveAcrossBoard(cellToMove) && playerStats.GetCurrentMana() >= stats.manaCost && !stats.stun && canMove)
        {
            playerStats.SubstractMana(stats.manaCost);
            BypassMovement(cellToMove);

            return true;
        }
        return base.OnMovement(cellToMove);
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003018002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003018001, token.transform.position);
    }

    private bool CheckIfMoveAcrossBoard(TileNode cellToMove)
    {
        if (!moveAcrossBoard.ContainsKey(onTile))
        { return false; }

        if (moveAcrossBoard[onTile] != cellToMove.GetId())
        { return false; }

        if (TileNodeManager.Instance.GetNodeById(moveAcrossBoard[onTile]).GetCharacter() != null)
        { return false; }

        return true;
    }
    private void InitAdditionalMoves()
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
}
