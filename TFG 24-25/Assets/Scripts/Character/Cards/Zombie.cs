using UnityEngine;

public class Zombie : Character
{
    public Zombie(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
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
        SoundManager.Instance.PlaySFX(003028002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003028001, token.transform.position);
    }
}
