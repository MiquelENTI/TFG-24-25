using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHorseman : Character
{
    public DeathHorseman(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }
   
    //Implementació del so en relacio al SoundManager 
    protected override void OnMovementSFX()
    {

    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(019, token.transform.position);
    }
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(019, token.transform.position);
    }
}
