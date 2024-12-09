using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanWerewolf : Character
{
    CharacterStats humanForm;
    CharacterStats wereWolfForm;
    bool isHumanForm = true;
    public HumanWerewolf(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
        humanForm = newStats;
        wereWolfForm = newStats;
        wereWolfForm.dmg = 9;
        wereWolfForm.hp = 16;
        wereWolfForm.movementType = MovementType.Omni;
    }

    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);
        ChangeForms();
       
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
}
