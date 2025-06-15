using UnityEngine;

public class MagicKarp : Character
{
    public MagicKarp(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();
        TileNode currentTile = TileNodeManager.Instance.GetNodeById(onTile);
        foreach (TileConnection direction in directions)
        {
            if (currentTile.CheckConnectionNode(direction))
            {
                Character enemy = currentTile.GetTileByDirection(direction).GetCharacter();
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
