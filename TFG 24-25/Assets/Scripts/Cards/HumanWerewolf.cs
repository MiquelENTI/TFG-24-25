using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWerewolf : Character
{
    CharacterStats humanForm;
    CharacterStats wereWolfForm;
    bool isHumanForm = true;
    public HumanWerewolf(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        humanForm = newStats;
        wereWolfForm = newStats;
        wereWolfForm.dmg = 8;
        wereWolfForm.hp = 8;
        wereWolfForm.movementType = MovementType.Omni;
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        ChangeForms();

        return true;
    }

    void ChangeForms()
    {
        isHumanForm = !isHumanForm;
        if (isHumanForm)
        {
            stats.hp = (int)(stats.hp / 2.0f);
            stats.hp = Mathf.RoundToInt(stats.hp);

            stats.movementType = humanForm.movementType;
            ChangeMovementType(stats.movementType);
            stats.dmg = humanForm.dmg;

            stats.PrintStats();
        }
        else
        {
            stats.hp *= 2;

            stats.movementType = wereWolfForm.movementType;
            stats.dmg = wereWolfForm.dmg;

            ChangeMovementType(stats.movementType);
            stats.PrintStats();
        }
        
    }

    //Implementació del so en relacio al SoundManager 
    protected override void OnMovementSFX()
    {

    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(017, token.transform.position);
    }
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(017, token.transform.position);
    }
}
