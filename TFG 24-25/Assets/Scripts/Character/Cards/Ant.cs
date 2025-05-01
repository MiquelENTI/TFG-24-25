using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : Character
{
    int originalAttack;
    int currentAttack;
    public Ant(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        originalAttack = newStats.dmg;
    }

    public override bool OnSpawn(CellNode cellToMove)
    {
        base.OnSpawn(cellToMove);

        CalculateCurrentAttack(cellToMove);

        return true;
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        CalculateCurrentAttack(cellToMove);

        return true;
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();

        CalculateCurrentAttack(CellNodeManager.Instance.GetNodeById(onTile));
    }

    void CalculateCurrentAttack(CellNode cellToMove)
    {
        List<CellNode> surrondingCells = cellToMove.GetSurrondingCells();

        currentAttack = originalAttack;
        foreach (CellNode surrondingCell in surrondingCells)
        {
            if (surrondingCell.GetCharacter() != null)
            {
                currentAttack++;
            }
        }
        stats.dmg = currentAttack;
        DisplayActionsManager.Instance.CreateCustomText("="+currentAttack.ToString(), new Color(220.0f / 255.0f, 20.0f / 255.0f, 60.0f / 255.0f), 8, 1.5f, token.transform.position, -0.2f);
    }
    //Implementació del so 
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003038001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003038002, token.transform.position);
    }

}
