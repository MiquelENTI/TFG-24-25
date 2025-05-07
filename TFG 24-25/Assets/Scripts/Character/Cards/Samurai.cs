using UnityEngine;

public class Samurai : Character
{
    public Samurai(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnSpawn(CellNode cellToMove)
    {
        return base.OnSpawn(cellToMove);
    }

    protected override void OnDeath(Character attacker)
    {
        base.OnDeath(attacker);
    }
    protected override void OnPointsScoring(int pointsScored)
    {
        base.OnPointsScoring(pointsScored);

        stats.dmg += pointsScored;
        DisplayActionsManager.Instance.CreateBuffText(pointsScored, true, token.transform.position);
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003036002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003036001, token.transform.position);
    }
}
