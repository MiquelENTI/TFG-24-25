using System.Collections.Generic;
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


    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003030002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003030001, token.transform.position);
    }

    private void EvadeEnemy()
    {
        TileNode currentTile = TileNodeManager.Instance.GetNodeById(onTile);

        List<TileNode> surroundingTiles = new();
        surroundingTiles = currentTile.GetSurrondingTiles();

        foreach (TileNode cell in surroundingTiles)
        {
            if (cell.GetCharacter() == null)
            { continue; }

            if (cell.GetCharacter().GetTeamType() != teamType)
            {
                TileConnection direction = currentTile.GetDirectionToTileByRange(cell.GetId(), 1).Item2;

                TileNode cellToMove = currentTile.GetTileByInverseDirection((int)direction);

                if (cellToMove == null) { return; }

                if (!cellToMove.IsOccupied())
                {
                    BypassMovement(TileNodeManager.Instance.GetNodeById(onTile).GetTileByInverseDirection((int)direction));
                }
            }
        }
    }
}
