using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grasshopper : Character
{
    public Grasshopper(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        jumpMove = true;
    }
}
