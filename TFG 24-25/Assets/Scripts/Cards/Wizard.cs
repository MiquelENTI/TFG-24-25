using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
    // This has infinite attack range. WIP


    public Wizard(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override void Effect() 
    {
        // Launch Fireball
    }
}
