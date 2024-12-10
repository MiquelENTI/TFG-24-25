using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Character
{


    public Dummy(CharacterStats newStats, TeamType teamType) : base(newStats, teamType, null, null)
    {
    }

    public override void OnAttack(Character attacker)
    {
        Debug.Log("DUMMY ATTACKED");
        if (attacker.GetTeamType() != teamType) 
        {
            Debug.Log("INCREASING POINTS");
            scoreManager.UpdateScore(attacker.GetTeamType());
        }
    }
}
