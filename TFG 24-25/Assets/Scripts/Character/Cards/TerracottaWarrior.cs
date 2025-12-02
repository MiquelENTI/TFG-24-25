using UnityEngine;

public class TerracottaWarrior : Character
{
    public TerracottaWarrior(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }
    public override bool OnMovement(TileNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        return true;
    }

    protected override void OnPointsScoring(int pointsScored)
    {
        base.OnPointsScoring(pointsScored);

        Heal(pointsScored);
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003017002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003017001, token.transform.position);
    }
}
