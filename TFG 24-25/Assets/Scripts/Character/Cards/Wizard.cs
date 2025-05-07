using UnityEngine;

public class Wizard : Character
{
    public Wizard(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    protected override void OnDeath(Character attacker)
    {
        //DeckManager.Instance.BoardIntoHandDraw(teamType, stats.name);
        base.OnDeath(attacker);
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003001002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003001001, token.transform.position);
    }
}
