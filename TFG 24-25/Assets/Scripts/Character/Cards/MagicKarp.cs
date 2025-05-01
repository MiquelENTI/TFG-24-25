using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKarp : Character
{
    public MagicKarp(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();
        CellNode currentCell = CellNodeManager.Instance.GetNodeById(onTile);
        foreach (CellConnection direction in directions)
        {
            if (currentCell.CheckConnectionNode(direction))
            {
                Character enemy = currentCell.GetCellByDirection(direction).GetCharacter();
                if (enemy != null)
                {
                    enemy.ReceiveDamage(this, 9999);
                }
            }
        }
    }
}
