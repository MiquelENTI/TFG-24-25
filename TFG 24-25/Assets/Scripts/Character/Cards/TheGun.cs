using UnityEngine;

public class TheGun : Character
{
    private CellNode currentCellToMove;

    public TheGun(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }
    public override bool OnMovement(CellNode cellToMove)
    {
        currentCellToMove = cellToMove;

        if (!base.OnMovement(cellToMove))
        { return false; }

        return true;
    }
    public override void Attack(Character enemy)
    {
        base.Attack(enemy);
        foreach (CellConnection direction in directions)
        {
            CellNode cellOnTile = CellNodeManager.Instance.GetNodeById(onTile);
            if (!cellOnTile.CheckConnectionNode(direction))
            {
                continue;
            }

            if (cellOnTile.GetCellByDirection(direction).GetId() == currentCellToMove.GetId())
            {
                CellNode cellToMove = cellOnTile.GetCellByInverseDirection((int)direction);
                
                if (cellToMove == null) { return; }

                if (!cellToMove.IsOccupied())
                {
                    BypassMovement(CellNodeManager.Instance.GetNodeById(onTile).GetCellByInverseDirection((int)direction));
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
