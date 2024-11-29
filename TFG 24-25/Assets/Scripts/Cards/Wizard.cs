using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Character
{
    // This has infinite attack range. WIP


    public Wizard(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void Effect() 
    {
        // Launch Fireball
    }
}
