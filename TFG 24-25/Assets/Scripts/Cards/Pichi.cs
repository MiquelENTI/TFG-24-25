using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pichi : Character
{
    List<Character> charactersAttackDisabled;
    public Pichi(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        charactersAttackDisabled = new List<Character>();
    }

    public override void Attack(Character enemy)
    {
        charactersAttackDisabled.Add(enemy);
        enemy.SetDisarmAttack(true);
        base.Attack(enemy);
    }

    public override void OnStartTurn()
    {
        charactersAttackDisabled.ForEach(_ => disarmAttack = false);
        charactersAttackDisabled.Clear();
        base.OnStartTurn();
    }

    public override void OnDeath(Character attacker)
    {
        charactersAttackDisabled.ForEach(_ => disarmAttack = false);
        charactersAttackDisabled.Clear();
        base.OnDeath(attacker);
    }
    //Implementació del so
        
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003008001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003008002, token.transform.position);
    }
}
