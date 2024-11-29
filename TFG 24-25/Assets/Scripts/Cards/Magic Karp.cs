using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKarp : Character
{
    public MagicKarp(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
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
                    Debug.Log(enemy.GetTeamType().ToString());
                    enemy.ReceiveDamage(9999);
                }
            }
        }
    }
}
