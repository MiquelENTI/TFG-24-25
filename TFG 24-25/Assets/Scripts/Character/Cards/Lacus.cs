using Photon.Pun;
using UnityEngine;

public class Lacus : Character
{
    private CellConnection direction;

    public Lacus(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            direction = CellConnection.UP;
        }
        else
        {
            direction = CellConnection.DOWN;
        }
    }

    public override bool OnMovement(CellNode cellToMove)
    {
        MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
        if (playerStats.GetCurrentMana() >= stats.manaCost && !stats.stun)
        {
            Vector2 diff = cellToMove.GetPositionInGrid() - CellNodeManager.Instance.GetNodeById(onTile).GetPositionInGrid();

            diff.x = Mathf.Abs(diff.x);
            diff.y = Mathf.Abs(diff.y);

            if (diff.x > stats.movementRange || diff.y > stats.movementRange)
            {
                Debug.Log("ENTERED IF RANGE");
                return false;
            }

            if (diff.x == 0.0f || diff.y == 0.0f)
            {
                direction = CellNodeManager.Instance.GetNodeById(onTile).GetDirectionToCellByRange(cellToMove.GetId(), 1).Item2;
                Debug.Log("CURRENT DIRECTION: " + direction.ToString());

                DisplayActionsManager.Instance.CreateCustomText("Direction: " + direction.ToString(), Color.white, 4, 1.0f, token.transform.position);

                playerStats.SubstractMana(stats.manaCost);
                return true;
            }
        }

        return false;
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();

        CellNode currentCell = CellNodeManager.Instance.GetNodeById(onTile);

        if (stats.stun)
        { return; }

        if (!currentCell.CheckConnectionNode(direction))
        {
            DestroyCharacter();
            return;
        }

        Character infrontCharacter = currentCell.GetCellByDirection(direction).GetCharacter();
        if (infrontCharacter == null)
        {
            BypassMovement(currentCell.GetCellByDirection(direction));
            return; 
        }

        if (infrontCharacter.GetTeamType() == teamType)
        {
            return;
        }
        else
        {
            if (canAttack)
            {
                Attack(infrontCharacter);
            }
        }
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003035002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003035001, token.transform.position);
    }
}
