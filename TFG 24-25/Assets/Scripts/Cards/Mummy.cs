using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : Character
{
    public Mummy(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        DecreaseDamage(1);
        ReceiveDamage(1);

        return true;
    }
   
    //Implementació del so en relacio al SoundManager 
    protected override void OnMovementSFX()
    {

    }
    protected override void AttackSFX()
    {
        base.AttackSFX();
        SoundManager.Instance.PlaySFX(012, token.transform.position);
    }
    protected override void OnSpawnSFX()
    {
        base.OnSpawnSFX();
        SoundManager.Instance.PlaySFX(013, token.transform.position);
        Debug.Log("Distancia: " + Vector3.Distance(Camera.main.transform.position, token.transform.position));
    }
}
