using System.Collections.Generic;
using UnityEngine;

public class Pichi : Character
{
    private List<Character> charactersAttackDisabled;
    public Pichi(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        charactersAttackDisabled = new List<Character>();
    }

    public override void OnStartTurn()
    {
        charactersAttackDisabled.ForEach(_ => disarmAttack = false);
        charactersAttackDisabled.Clear();
        base.OnStartTurn();
    }
    protected override void OnDeath(Character attacker)
    {
        charactersAttackDisabled.ForEach(_ => disarmAttack = false);
        charactersAttackDisabled.Clear();
        base.OnDeath(attacker);
    }
    public override void Attack(Character enemy)
    {
        charactersAttackDisabled.Add(enemy);
        enemy.SetDisarmAttack(true);
        base.Attack(enemy);
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003008002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003008001, token.transform.position);
    }
}
