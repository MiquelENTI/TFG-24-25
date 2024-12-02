using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum CellConnection { UP = 1, DOWN = -1, RIGHT = 2, LEFT = -2, UPRIGHT = 3, DOWNLEFT = -3, UPLEFT = 4, DOWNRIGHT = -4 }

public enum CellSpawnable { NONE = -1, RED = 0, BLUE = 1}
public class CellNode
{
    Dictionary<CellConnection, CellNode> connectionDictionary = new();
    int id;
    Vector3 position;
    Character character;
    bool isOccupied = false;
    bool isConnected = true;
    CellSpawnable spawnable = CellSpawnable.NONE;

    public CellNode(Vector3 position)
    {  
        this.position = position;

        CellNodeManager.Instance.AddNode(this);
    }


    public bool CheckConnectionNode(CellConnection cellConnection)
    {
        return connectionDictionary.ContainsKey(cellConnection);
    }

    public CellNode GetCellByDirection(CellConnection cellConnection)
    {
        return connectionDictionary[cellConnection];
    }

    public bool CheckSingleNode(CellConnection direction, int characterId)
    {
        if (CheckConnectionNode(direction))
        {
            Debug.Log("Node is disconnected from grid");
            return false;
        }
        if (connectionDictionary[direction].IsConnected())
        {
            Debug.Log("Node is not connected to this node");
            return false;
        }

        Character character = connectionDictionary[GetInverseDirection((int)direction)].GetCharacter();
        if (character == null)
        {
            Debug.Log("Character is not in the direction");
            return false;
        }
        if (character.GetId() != characterId)
        {
            Debug.Log("Wrong Character");
            return false;
        }

        return true;
    }

    
    public bool CheckMultipleNodeForCharacter(List<CellConnection> directions, int characterId)
    {
        if (!isConnected)
        {
            Debug.Log("Node to move is disconnected from grid");
            return false; 
        }

        bool isAttacking = false;
        if (isOccupied)
        {
            if (CharactersManager.Instance.GetCharacterInBoardById(characterId).GetTeamType() == character.GetTeamType())
            {
                Debug.Log("Node to move is occupied by another character");
                return false;
            }
            isAttacking = true;
        }

        foreach (CellConnection direction in directions)
        {
            if (!CheckConnectionNode(direction))
            {
                continue;
            }

            Character characterMoving = connectionDictionary[direction].GetCharacter();
            if (characterMoving == null)
            {
                continue;
            }
            if (characterMoving.GetId() != characterId)
            {
                continue;
            }

            if (isAttacking)
            {
                
                //if (characterMoving.CanAttack())
                if (true)
                {
                    characterMoving.Attack(this.character);
                }
                return false;
            }

            return true;
        }
        return false;
    }

    public void AddConnection(CellConnection cellMovement, CellNode cellNode)
    {
        connectionDictionary.Add(cellMovement, cellNode);
    }

    public Character GetCharacter()
    { 
        if (character != null)
            return character;
        else 
            return null;
    }

    CellConnection GetInverseDirection(int cellConnection)
    {
        return (CellConnection)(-cellConnection);
    }

    public void SetPosition(Vector3 pos)
    {
        position = pos;
    }

    public Vector3 GetPosition()
    { return position; }

    public void SetId(int id)
    {
        this.id = id;
    }

    public int GetId()
    { return id; }

    public void SetCharacter(Character character)
    { 
        this.character = character; 
        isOccupied = true;
    }

    public void RemoveCharacter() 
    { 
        character = null; 
        isOccupied = false;
    }

    public void SetSpawnable(CellSpawnable spawnable)
    { this.spawnable = spawnable; }

    public void SetOccupied(bool occupied)
    { isOccupied = occupied; }

    public bool IsOccupied()
    {  return isOccupied; }

    public bool IsConnected() 
    { return isConnected; }

    public bool CanCharacterSpawn(Character character)
    {
        // At the moment like this, in the future do return if statement
        if((CellSpawnable)character.GetTeamType() == spawnable && this.character == null)
        { return true; }
        else 
        { return false; }
    }

    public void PrintStatus()
    {
        try
        {
            Debug.Log(id.ToString() +
            ": Position: X: " + position.x + " Z: " + position.z +
            " |  Num of Connections: " + connectionDictionary.Count +
            " |  IsOccupied: " + isOccupied.ToString() +
            " by: " + GetCharacter().GetId().ToString() +
            " |  IsConnected: " + isConnected.ToString() +
            " |  Spawnable: " + spawnable.ToString());
        }

        catch 
        {
            Debug.Log(id.ToString() +
                ": Position: X: " + position.x + " Z: " + position.z +
                " |  Num of Connections: " + connectionDictionary.Count +
                " |  IsOccupied: " + isOccupied.ToString() +
                " |  IsConnected: " + isConnected.ToString() +
                " |  Spawnable: " + spawnable.ToString());
        }
        
    }

}
