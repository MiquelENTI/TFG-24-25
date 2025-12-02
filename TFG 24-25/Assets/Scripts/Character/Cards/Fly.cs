using UnityEngine;

public class Fly : Character
{
    public Fly(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }
    public override bool OnSpawn(TileNode cellToMove)
    {
        return base.OnSpawn(cellToMove);
    }
    protected override void OnDeath(Character attacker)
    {
        base.OnDeath(attacker);
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003005002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003005001, token.transform.position);
    }
}
