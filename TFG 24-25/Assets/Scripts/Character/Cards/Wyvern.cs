using UnityEngine;

public class Wyvern : Character
{
    bool isTired = false;
    bool isReady = false;

    public Wyvern(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        if (!isTired)
        {
            return base.OnMovement(cellToMove);
        }
        else
        {
            MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
            return false;
        }
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();
        
        if (isReady)
        {
            isTired = false;
        }

        if (isTired)
        {
            isReady = true;
        }
    }
    public override void Attack(Character enemy)
    {
        if (!isTired)
        {
            base.Attack(enemy);
            isTired = true;
            isReady = false;
        }
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003039002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003039001, token.transform.position);
    }
}
