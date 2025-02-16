using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : Character
{
    List<Character> enemiesAttacked = new();
    public Medusa(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void Attack(Character enemy)
    {
        enemy.ApplyStun();
        enemiesAttacked.Add(enemy);
        base.Attack(enemy);
        //FMOD_ATTACK_MEDUSA
    //FMODUnity.RuntimeManager.PlayOneShot("event:/MEDUSA",token.transform.position);
    }

    public override void OnDeath(Character attacker)
    {
        foreach (Character enemy in enemiesAttacked)
        {
            if (enemy != null)
            {
                enemy.RemoveStun();
            }
        }

        base.OnDeath(attacker);
    }
    protected override void OnMovementSFX()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("",token.transform.position);
    }
    protected override void AttackSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/CARDS/MEDUSA/MEDUSA_ATTACK",token.transform.position);
    }
    protected override void OnSpawnSFX()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/CARDS/MEDUSA/MEDUSA_SELECTION",token.transform.position);
    }
}
