using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum MovementType { Basic, Diagonal, Omni}
public enum TeamType { RED = 0, BLUE = 1}
public class Character
{
    int id;
    List<CellConnection> directions;
    TeamType teamType;
    int onTile;
    bool toSpawn = true;
    int numOfMovements;
    int movementsLeft;

    GameObject token;
    Sprite cardSprite;

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
    { return movementsLeft; }

    public void DecreaseMovement()
    { movementsLeft--; }
    void ResetMovementsLeft()
    { movementsLeft = numOfMovements; }

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

    public Character(MovementType movementType, TeamType teamType, int numOfMovements, GameObject token, Sprite cardSprite)
    {
        directions = new List<CellConnection>();
        switch (movementType)
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
        this.numOfMovements = numOfMovements;
        movementsLeft = numOfMovements;

        CharactersManager.Instance.AddCharacter(this, cardSprite);
        this.token = token;
        this.cardSprite = cardSprite;
    }

    public void ResetStats()
    {
        ResetMovementsLeft();
    }
}
