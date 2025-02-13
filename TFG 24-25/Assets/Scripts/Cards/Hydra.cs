using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : Character
{
    int amountOfHealing = 1;

    public Hydra(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnPointsScoring()
    {
        base.OnPointsScoring();
        Heal(amountOfHealing);
    }
}
