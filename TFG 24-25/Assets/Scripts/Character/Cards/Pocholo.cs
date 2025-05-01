using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Pocholo : Character
{
    public Pocholo(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        MyEventHandler.Instance.moveToken.AddListener((int cellId, int characterId) =>
        {
            EvadeEnemy();
        });

        MyEventHandler.Instance.spawnToken.AddListener((int cellId, int characterId, bool bypassSpawn) =>
        {
            EvadeEnemy();
        });
    }

    void EvadeEnemy()
    {
        CellNode currentCell = CellNodeManager.Instance.GetNodeById(onTile);

        List<CellNode> surroundingCells = new();
        surroundingCells = currentCell.GetSurrondingCells();

        foreach (CellNode cell in surroundingCells)
        {
            if (cell.GetCharacter() == null)
            { continue; }

            if (cell.GetCharacter().GetTeamType() != teamType)
            {
                CellConnection direction = currentCell.GetDirectionToCellByRange(cell.GetId(), 1).Item2;

                CellNode cellToMove = currentCell.GetCellByInverseDirection((int)direction);

                if (cellToMove == null) { return; }

                if (!cellToMove.IsOccupied())
                {
                    BypassMovement(CellNodeManager.Instance.GetNodeById(onTile).GetCellByInverseDirection((int)direction).GetId());
                }
            }
        }
    }
     //Implementació del so
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003030001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003030002, token.transform.position);
    }
}
