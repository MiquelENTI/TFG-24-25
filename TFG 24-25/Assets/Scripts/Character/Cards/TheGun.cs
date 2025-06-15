using UnityEngine;

public class TheGun : Character
{
    private TileNode currentTileToMove;

    public TheGun(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }
    public override bool OnMovement(TileNode tileToMove)
    {
        currentTileToMove = tileToMove;

        if (!base.OnMovement(tileToMove))
        { return false; }

        return true;
    }
    public override void Attack(Character enemy)
    {
        base.Attack(enemy);
        foreach (TileConnection direction in directions)
        {
            TileNode cellOnTile = TileNodeManager.Instance.GetNodeById(onTile);
            if (!cellOnTile.CheckConnectionNode(direction))
            {
                continue;
            }

            if (cellOnTile.GetTileByDirection(direction).GetId() == currentTileToMove.GetId())
            {
                TileNode cellToMove = cellOnTile.GetTileByInverseDirection((int)direction);
                
                if (cellToMove == null) { return; }

                if (!cellToMove.IsOccupied())
                {
                    BypassMovement(TileNodeManager.Instance.GetNodeById(onTile).GetTileByInverseDirection((int)direction));
                }
            }
        }
    }
    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003014002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003014001, token.transform.position);
    }
}
