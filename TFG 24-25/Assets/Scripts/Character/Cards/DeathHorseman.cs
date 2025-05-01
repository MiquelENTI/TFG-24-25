using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHorseman : Character
{
    public DeathHorseman(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }
   
    //Implementació del so general per a cartes no identificades
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003016002, token.transform.position);
    }
}
