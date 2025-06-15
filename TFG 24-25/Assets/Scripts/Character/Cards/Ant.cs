using System.Collections.Generic;
using UnityEngine;

public class Ant : Character
{
    private int originalAttack;
    private int currentAttack;
    public Ant(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        originalAttack = newStats.dmg;
    }

    public override bool OnSpawn(TileNode cellToMove)
    {
        base.OnSpawn(cellToMove);

        CalculateCurrentAttack(cellToMove);

        return true;
    }

    public override bool OnMovement(TileNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        CalculateCurrentAttack(cellToMove);

        return true;
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();

        CalculateCurrentAttack(TileNodeManager.Instance.GetNodeById(onTile));
    }

    private void CalculateCurrentAttack(TileNode cellToMove)
    {
        List<TileNode> surrondingTiles = cellToMove.GetSurrondingTiles();

        currentAttack = originalAttack;
        foreach (TileNode surrondingTile in surrondingTiles)
        {
            if (surrondingTile.GetCharacter() != null)
            {
                currentAttack++;
            }
        }
        stats.dmg = currentAttack;
        DisplayActionsManager.Instance.CreateCustomText("="+currentAttack.ToString(), new Color(220.0f / 255.0f, 20.0f / 255.0f, 60.0f / 255.0f), 8, 1.5f, token.transform.position, -0.2f);
    }

    //Implementació del so 
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003038002, token.transform.position);
    }

    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003038001, token.transform.position);
    }
}
