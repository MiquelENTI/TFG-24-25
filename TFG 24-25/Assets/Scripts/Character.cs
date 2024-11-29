using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum MovementType { Basic, Diagonal, Omni}
public enum TeamType { RED = 0, BLUE = 1}

public struct CharacterStats
{
    public int hp;
    public int dmg;
    public int manaCost;
    public int numOfMovements;
    public int movementsLeft;
    public MovementType movementType;

    public CharacterStats(int _hp, int _dmg, int _manaCost, int _numOfMovements, MovementType _movementType)
    {
        hp = _hp;
        dmg = _dmg;
        manaCost = _manaCost;
        numOfMovements = _numOfMovements;
        movementsLeft = _numOfMovements;
        movementType = _movementType;
    }
}

public class Character
{
    protected int id;
    protected CharacterStats stats;
    protected List<CellConnection> directions;

    protected TeamType teamType;
    protected int onTile;

    protected bool toSpawn = true;
    protected bool canAttack = true;

    protected GameObject token;
    protected Sprite cardSprite;

    public int GetId()
    { return id; }

    public void SetId(int id)
    { this.id = id; }

    public int GetOnTileId()
    { return onTile; }

    public void SetOnTileId(int id)
    { this.onTile = id; }

    public TeamType GetTeamType()
    { return teamType; }

    public List<CellConnection> GetDirections()
    { return directions; }

    public int GetMovementsLeft()
    { return stats.movementsLeft; }

    public void DecreaseMovement()
    { stats.movementsLeft--; }
    void ResetMovementsLeft()
    { stats.movementsLeft = stats.numOfMovements; }

    public bool IsToSpawn() 
    { return toSpawn; }

    public void DisableSpawn() 
    { toSpawn = false; }


    public void MoveToken(Vector3 position)
    {  
        position.y = 0.5f;
        token.transform.position = position;
    }
    
    public Sprite GetCardSprite()
    { return cardSprite; }

    public Character(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite)
    {
        stats = newStats;
        directions = new List<CellConnection>();
        switch (newStats.movementType)
        {
            case MovementType.Basic:
                directions.Add(CellConnection.UP);
                directions.Add(CellConnection.DOWN);
                directions.Add(CellConnection.LEFT);
                directions.Add(CellConnection.RIGHT);
                break;
            case MovementType.Diagonal:
                directions.Add(CellConnection.UPRIGHT);
                directions.Add(CellConnection.UPLEFT);
                directions.Add(CellConnection.DOWNRIGHT);
                directions.Add(CellConnection.DOWNLEFT);
                break;
            case MovementType.Omni:
                directions.Add(CellConnection.UP);
                directions.Add(CellConnection.DOWN);
                directions.Add(CellConnection.LEFT);
                directions.Add(CellConnection.RIGHT);
                directions.Add(CellConnection.UPRIGHT);
                directions.Add(CellConnection.UPLEFT);
                directions.Add(CellConnection.DOWNRIGHT);
                directions.Add(CellConnection.DOWNLEFT);
                break;
            default:
                break;
        }
        this.teamType = teamType;

        CharactersManager.Instance.AddCharacter(this, cardSprite);
        this.token = token;
        this.cardSprite = cardSprite;
    }

    public void ResetStats()
    {
        ResetMovementsLeft();
        canAttack = true;
    }




    public virtual void Effect()
    {

    }

    public virtual void OnSpawn(CellNode cellToMove)
    {
        if (cellToMove.CanCharacterSpawn(this) && !cellToMove.IsOccupied())
        {
            Debug.Log("SPAWN");
            DisableSpawn();
            cellToMove.SetCharacter(this);

            SetOnTileId(cellToMove.GetId());
            MoveToken(cellToMove.GetPosition());

            cellToMove.SetOccupied(true);
        }
    }
    
    public virtual void OnMovement(CellNode cellToMove)
    {
        canAttack = false;

        if (cellToMove.CheckMultipleNodeForCharacter(GetDirections(), id) && GetMovementsLeft() > 0) // Mana Cost
        {
            Debug.Log("MOOOVE");
            DecreaseMovement();
            CellNode previousNode = CellNodeManager.Instance.GetNodeById(onTile);

            previousNode.RemoveCharacter();
            cellToMove.SetCharacter(this);

            SetOnTileId(cellToMove.GetId());
            MoveToken(cellToMove.GetPosition());

            previousNode.SetOccupied(false);
            cellToMove.SetOccupied(true);
        }
        else
        {
            MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
            Debug.Log("NO?");
        }
    }

    public virtual void OnStartTurn()
    {

    }
    public virtual void OnEndTurn()
    {

    }

    public virtual void OnDeath()
    {

    }
    public virtual void OnKillEnemy()
    {

    }
}
