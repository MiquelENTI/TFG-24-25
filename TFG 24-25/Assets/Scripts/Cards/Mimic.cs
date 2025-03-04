using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mimic : Character
{
    public Mimic(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnAttack(Character attacker)
    {
        base.OnAttack(attacker);

        // Enemy draws a cards 
        attacker.ReceiveDamage(9999);
        Debug.Log("11");
        ReceiveDamage(9999);
        Debug.Log("12");
    }
}
