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

    GameObject token;

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

    public bool IsToSpawn() 
    { return toSpawn; }

    public void DisableSpawn() 
    { toSpawn = false; }

    public void MoveToken(Vector3 position)
    {  
        position.y = 0.5f;
        token.transform.position = position;
    }

    public Character(MovementType movementType, TeamType teamType, GameObject token)
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

        CharactersManager.Instance.AddCharacter(this);
        this.token = token;
    }
}
