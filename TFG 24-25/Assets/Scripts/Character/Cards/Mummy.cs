using UnityEngine;

public class Mummy : Character
{
    public Mummy(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnMovement(TileNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        DecreaseDamage(1);
        ReceiveDamageSelf(1);

        return true;
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003013002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003013001, token.transform.position);
    }
}
