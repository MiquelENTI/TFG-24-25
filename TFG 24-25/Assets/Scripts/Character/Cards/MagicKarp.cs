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

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003020002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003020001, token.transform.position);
    }
}
