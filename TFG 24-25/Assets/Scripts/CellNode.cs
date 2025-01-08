using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum CellConnection { UP = 1, DOWN = -1, RIGHT = 2, LEFT = -2, UPRIGHT = 3, DOWNLEFT = -3, UPLEFT = 4, DOWNRIGHT = -4 }
public enum CellScoreType { NOPOINTS = 0, NORMAL = 1, QUICK = 2 }
public enum CellSpawnable { NONE = -1, RED = 0, BLUE = 1}
public class CellNode
{
    Dictionary<CellConnection, CellNode> connectionDictionary = new();
    
    public Dictionary<CellConnection, List<CellNode>> baseConnections = null;
    bool isBaseNode = false;

    int id;
    Vector3 position;
    Character character;
    bool isOccupied = false;
    bool isConnected = true;
    CellSpawnable spawnable = CellSpawnable.NONE;
    Material cellMovementIndicator;
    bool isCellMovementIndicatorVisble = false;
    
    CellScoreType scoreType = CellScoreType.NOPOINTS;

    public CellNode(Vector3 position, GameObject movementIndicator)
    {  
        this.position = position;

        cellMovementIndicator = movementIndicator.GetComponent<MeshRenderer>().material;
        //ChangeMovementIndicatorVisibility();

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
                
                //if (characterMoving.CanAttack()) // Already has manacost calculation
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

    bool CheckMultipleNodeForCharacterFromBase(List<CellConnection> directions, int characterId)
    {
        for (int i = 0; i < baseConnections[CellConnection.DOWN].Count; i++)
            baseConnections[CellConnection.DOWN][i].PrintStatus();

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
                Debug.Log("Do not Attack Yourself!");
                return false;
            }
            isAttacking = true;
        }

        CellConnection direction = CellConnection.DOWN;
        if (character.GetTeamType() == TeamType.RED)
        {
            direction = CellConnection.DOWN;
        }
        else if (character.GetTeamType() == TeamType.BLUE)
        {
            direction = CellConnection.UP;
        }

        for (int i = 0; i < baseConnections[direction].Count; i++)
        {

            Character characterMoving = baseConnections[direction][i].GetCharacter();
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
                    return false;
                }
            }
        }
        return false;
    }

    public bool CheckNodes(List<CellConnection> directions, int characterId)
    {
        if (!isBaseNode)
        {
           return CheckMultipleNodeForCharacter(directions, characterId);
        }
        else
        {
           return CheckMultipleNodeForCharacterFromBase(directions, characterId);
        }
    }

    public void AddConnection(CellConnection cellMovement, CellNode cellNode)
    {
        connectionDictionary.Add(cellMovement, cellNode);
    }

    public void AddConnectionToBase(CellConnection cellMovement, List<CellNode> cellNodes)
    {
        baseConnections.Add(cellMovement, cellNodes);
    }

    public Character GetCharacter()
    { 
        if (character != null)
            return character;
        else 
            return null;
    }

    public CellConnection GetInverseDirection(int cellConnection)
    {
        return (CellConnection)(-cellConnection);
    }

    public CellNode GetCellByInverseDirection(int cellConnection)
    {
        if (CheckConnectionNode(GetInverseDirection(cellConnection)))
        {
            return connectionDictionary[GetInverseDirection(cellConnection)];
        }
        return null;
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

    public void ChangeMovementIndicatorVisibility()
    {
        isCellMovementIndicatorVisble = !isCellMovementIndicatorVisble;

        if (isCellMovementIndicatorVisble)
        {
            cellMovementIndicator.color = new Color(cellMovementIndicator.color.r, cellMovementIndicator.color.g, cellMovementIndicator.color.b, 139.0f/256.0f);
        }
        else
        {
            cellMovementIndicator.color = new Color(cellMovementIndicator.color.r, cellMovementIndicator.color.g, cellMovementIndicator.color.b, 0);
        }
    }

    public void IsBaseNode()
    {
        isBaseNode = true;
    }

    public CellNode GetCellByDirectionWithRange(CellNode currentCell, CellConnection direction, int range)
    {
        if (!currentCell.CheckConnectionNode(direction))
        {
            return null;
        }

        if (range > 0)
        {
            currentCell.GetCellByDirectionWithRange(currentCell.GetCellByDirection(direction), direction, range-1);
        }
        else
        {
            currentCell.PrintStatus();
            return currentCell;
        }

        return null;
    }
    
    
    public void SetScoreNode(CellScoreType type)
    {
        scoreType = type;
    }
    public CellScoreType GetScoreNode()
    {
        return scoreType;
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
