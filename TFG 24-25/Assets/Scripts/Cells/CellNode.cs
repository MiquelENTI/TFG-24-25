using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum CellConnection { UP = 1, DOWN = -1, RIGHT = 2, LEFT = -2, UPRIGHT = 3, DOWNLEFT = -3, UPLEFT = 4, DOWNRIGHT = -4 }
public enum CellScoreType { NOPOINTS = 0, NORMAL = 1, QUICK = 2 }
public enum CellScoreAmount { NORMAL = 1, ENEMYROWS = 2 }
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
    GameObject cellMovementIndicator;
    bool isCellMovementIndicatorVisble = false;
    
    CellScoreType scoreType = CellScoreType.NOPOINTS;
    CellScoreAmount scoreAmount = CellScoreAmount.NORMAL;

    // New Movement
    public Vector2 positionInGrid;

    public CellNode(Vector3 position, GameObject movementIndicator, int x, int y)
    {  
        this.position = position;
        positionInGrid = new Vector2(x, y);

        cellMovementIndicator = movementIndicator.transform.GetChild(0).gameObject;
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

    public List<CellNode> GetSurrondingCells()
    {
        List<CellNode> temp = new List<CellNode>();

        temp.Add(GetCellByDirection(CellConnection.UP));
        temp.Add(GetCellByDirection(CellConnection.UPRIGHT));
        temp.Add(GetCellByDirection(CellConnection.RIGHT));
        temp.Add(GetCellByDirection(CellConnection.DOWNRIGHT));
        temp.Add(GetCellByDirection(CellConnection.DOWN));
        temp.Add(GetCellByDirection(CellConnection.DOWNLEFT));
        temp.Add(GetCellByDirection(CellConnection.LEFT));
        temp.Add(GetCellByDirection(CellConnection.UPLEFT));

        return temp;
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

    public bool CheckNodes(int characterId)
    {
        return NewMovement(characterId);
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

    public Tuple<int, CellConnection> GetDirectionToCellByRange(int cellId, int range)
    {
        List<CellNode> surroundingCells = GetSurrondingCells();
        foreach (var direction in connectionDictionary)
        {
            CellNode nextCell = this;
            for (int i = 0; i < range; i++)
            {
                // Cell To Check
                nextCell = nextCell.GetCellByDirection(direction.Key);

                if (nextCell.id == cellId)
                {
                    return Tuple.Create(cellId, direction.Key);
                }
            }
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

    public CellSpawnable GetSpawnable()
    { return spawnable; }

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

        cellMovementIndicator.SetActive(isCellMovementIndicatorVisble);
    }

    public void ChangeMovementIndicatorVisibility(bool state)
    {
        isCellMovementIndicatorVisble = state;

        cellMovementIndicator.SetActive(isCellMovementIndicatorVisble);
    }

    public void SetScoreNode(CellScoreType type)
    {
        scoreType = type;
    }
    public CellScoreType GetScoreNode()
    {
        return scoreType;
    }

    public void SetScoreAmount(CellScoreAmount type)
    {
        scoreAmount = type;
    }
    public CellScoreAmount GetScoreAmount()
    {
        return scoreAmount;
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

    bool NewMovement(int characterId)
    {
        if (!isConnected)
        {
            Debug.Log("Node to move is disconnected from grid");
            return false;
        }

        Character characterMoving = CharactersManager.Instance.GetCharacterInBoardById(characterId);
        bool isAttacking = false;
        if (isOccupied)
        {
            if (characterMoving.GetTeamType() == character.GetTeamType())
            {
                Debug.Log("Node to move is occupied by another character");
                return false;
            }
            isAttacking = true;
        }

        CellNode characterTile = CellNodeManager.Instance.GetNodeById(characterMoving.GetOnTileId());

        Vector2 diff = positionInGrid - characterTile.positionInGrid;

        diff.x = Mathf.Abs(diff.x);
        diff.y = Mathf.Abs(diff.y);

        //Debug.Log("Diff on Enemy and Character: " + diff + "POSITIONS: " + positionInGrid + " - " + characterTile.positionInGrid);

        if (diff.x > characterMoving.GetCharacterStats().range || diff.y > characterMoving.GetCharacterStats().range)
        {
            Debug.Log("ENTERED IF RANGE");
            return false; }

        //Debug.Log("Character Range: " + characterMoving.GetCharacterStats().range);

        //Debug.Log(characterMoving.GetCharacterStats().movementType.ToSafeString());
        switch (characterMoving.GetCharacterStats().movementType)
        {
            case MovementType.Basic:
            {
                if (diff.x == 0.0f || diff.y == 0.0f)
                {
                    if (isAttacking)
                    {
                        if (characterMoving.CanAttack())
                        {
                            characterMoving.Attack(this.character);
                        }
                        return false;
                    }
                    return true;
                }
                break;
            }
            case MovementType.Diagonal:
            {
                if (diff.x == diff.y)
                {
                    if (isAttacking)
                    {
                        if (characterMoving.CanAttack())
                        {
                            characterMoving.Attack(this.character);
                        }
                        return false;
                    }
                    return true;
                }
                break;
            }
            case MovementType.Omni:
            {
                if (diff.x == 0.0f || diff.y == 0.0f || (diff.x == diff.y))
                {
                    if (isAttacking)
                    {
                        if (characterMoving.CanAttack())
                        {
                            characterMoving.Attack(this.character);
                        }
                        return false;
                    }
                    return true;
                }
                break;
            }
        }
        return false;
    }
}
