using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cavalier : Character
{
    public Cavalier(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite) : base(newStats, teamType, token, cardSprite)
    {
    }

    public override void OnMovement(CellNode cellToMove)
    {
        base.OnMovement(cellToMove);
    }
    public override void Attack(Character enemy)
    {
        canAttack = true;
        base.Attack(enemy);
    }
}
