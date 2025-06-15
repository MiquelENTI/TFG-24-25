using Photon.Pun;
using UnityEngine;

public class Lacus : Character
{
    private TileConnection direction;

    public Lacus(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            direction = TileConnection.UP;
        }
        else
        {
            direction = TileConnection.DOWN;
        }
    }

    public override bool OnMovement(TileNode cellToMove)
    {
        MoveToken(TileNodeManager.Instance.GetNodeById(onTile).GetPosition());
        if (playerStats.GetCurrentMana() >= stats.manaCost && !stats.stun)
        {
            Vector2 diff = cellToMove.GetPositionInGrid() - TileNodeManager.Instance.GetNodeById(onTile).GetPositionInGrid();

            diff.x = Mathf.Abs(diff.x);
            diff.y = Mathf.Abs(diff.y);

            if (diff.x > stats.movementRange || diff.y > stats.movementRange)
            {
                Debug.Log("ENTERED IF RANGE");
                return false;
            }

            if (diff.x == 0.0f || diff.y == 0.0f)
            {
                direction = TileNodeManager.Instance.GetNodeById(onTile).GetDirectionToTileByRange(cellToMove.GetId(), 1).Item2;
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

        TileNode currentTile = TileNodeManager.Instance.GetNodeById(onTile);

        if (stats.stun)
        { return; }

        if (!currentTile.CheckConnectionNode(direction))
        {
            DestroyCharacter();
            return;
        }

        Character infrontCharacter = currentTile.GetTileByDirection(direction).GetCharacter();
        if (infrontCharacter == null)
        {
            BypassMovement(currentTile.GetTileByDirection(direction));
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
