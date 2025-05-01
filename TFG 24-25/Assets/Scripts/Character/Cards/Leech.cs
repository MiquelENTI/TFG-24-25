using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Leech : Character
{
    int surroundingDamage = 1;

    public Leech(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();

        CellNode node = CellNodeManager.Instance.GetNodeById(onTile);

        if (node.GetScoreNode() == CellScoreType.NOPOINTS)
        { return; }

        List<CellNode> surrondingCells = node.GetSurrondingCells();

        foreach (CellNode cellnode in surrondingCells)
        {
            Character enemy = cellnode.GetCharacter();
            if (enemy == null)
            { continue; }

            if (enemy.GetTeamType() != teamType)
            {
                enemy.ReceiveDamage(this, surroundingDamage);
            }
        }
    }
     
     //Implementació del so
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003025001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003025001, token.transform.position);
    }
}
