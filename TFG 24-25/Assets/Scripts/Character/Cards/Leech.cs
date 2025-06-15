using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Leech : Character
{
    private int surroundingDamage = 1;

    public Leech(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();

        TileNode node = TileNodeManager.Instance.GetNodeById(onTile);

        if (node.GetScoreNode() == TileScoreType.NOPOINTS)
        { return; }

        List<TileNode> surrondingTiles = node.GetSurrondingTiles();

        foreach (TileNode cellnode in surrondingTiles)
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
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003025001, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003025001, token.transform.position);
    }
}
