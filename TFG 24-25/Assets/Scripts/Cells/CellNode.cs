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

        for (int i = 1; i < 5; i++)
        {
            int inverseDirection = i * -1;
            if (CheckConnectionNode((CellConnection)i))
            {
                temp.Add(GetCellByDirection((CellConnection)i));
            }
            if (CheckConnectionNode((CellConnection)inverseDirection))
            {
                temp.Add(GetCellByDirection((CellConnection)inverseDirection));
            }
        }

        return temp;
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
                if (!nextCell.CheckConnectionNode(direction.Key))
                {
                    break;
                }

                nextCell = nextCell.GetCellByDirection(direction.Key);
                if (nextCell.id == cellId)
                {
                    return Tuple.Create(i, direction.Key);
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Returns: If Character is Found on the way to the CellNode, Distance
    /// </summary>
    /// <param name="cellId"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public Tuple<bool, int> CheckIsCharacterOnTheWay(int cellId, int range)
    {
        Tuple<int, CellConnection> direction = GetDirectionToCellByRange(cellId, range);

        CellNode cellToCheck = GetCellByDirection(direction.Item2);

        for (int i = 1; i < direction.Item1; i++)
        {
            //Debug.Log("Checking in Distance " + i);
            if (cellToCheck.GetCharacter() != null)
            {
                //Debug.Log("Encountered Character at " + i);
                return Tuple.Create(true, i);
            }
            cellToCheck = cellToCheck.GetCellByDirection(direction.Item2);
        }
        //Debug.Log("NO Encounter");
        return Tuple.Create(false, 0);
    }

    /// <summary>
    /// Returns: Distance, Direction, Character Detected
    /// </summary>
    /// <param name="cellId"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public Tuple<int, CellConnection, Character> CheckForCharacterOnTheWay(int cellId, int range)
    {
        Tuple<int, CellConnection> direction = GetDirectionToCellByRange(cellId, range);

        CellNode cellToCheck = GetCellByDirection(direction.Item2);

        for (int i = 1; i < direction.Item1; i++)
        {
            if (cellToCheck.GetCharacter() != null)
            {
                return Tuple.Create(i,direction.Item2, cellToCheck.character);
            }
            cellToCheck = GetCellByDirection(direction.Item2);
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
            " |  Spawnable: " + spawnable.ToString() +
            " |  ScoreMode: " + scoreType.ToString() +
            " |  ScoreAmount: " + scoreAmount.ToString());
        }

        catch 
        {
            Debug.Log(id.ToString() +
                ": Position: X: " + position.x + " Z: " + position.z +
                " |  Num of Connections: " + connectionDictionary.Count +
                " |  IsOccupied: " + isOccupied.ToString() +
                " |  IsConnected: " + isConnected.ToString() +
                " |  Spawnable: " + spawnable.ToString() +
                " |  ScoreMode: " + scoreType.ToString() +
                " |  ScoreAmount: " + scoreAmount.ToString());
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
                //return false;
            }
            isAttacking = true;
            Debug.Log("IsAttacking");
        }

        CellNode characterTile = CellNodeManager.Instance.GetNodeById(characterMoving.GetOnTileId());

        Vector2 diff = positionInGrid - characterTile.positionInGrid;
        
        diff.x = Mathf.Abs(diff.x);
        diff.y = Mathf.Abs(diff.y);

        //Debug.Log("Diff on Enemy and Character: " + diff + "POSITIONS: " + positionInGrid + " - " + characterTile.positionInGrid);

        if ((diff.x > characterMoving.GetCharacterStats().movementRange || diff.y > characterMoving.GetCharacterStats().movementRange) && !isAttacking)
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
                    return CanAttackOrMoveLogic(isAttacking, characterMoving, diff);
                }
                break;
            }
            case MovementType.Diagonal:
            {
                if (diff.x == diff.y)
                {
                    return CanAttackOrMoveLogic(isAttacking, characterMoving, diff);
                }
                break;
            }
            case MovementType.Omni:
            {
                if (diff.x == 0.0f || diff.y == 0.0f || (diff.x == diff.y))
                {
                    return characterMoving.GetJumpMove() ? CanAttackOrMoveLogicJump(isAttacking, characterMoving, diff) : CanAttackOrMoveLogic(isAttacking, characterMoving, diff);
                }
                break;
            }
        }
        return false;
    }

    bool CanAttackOrMoveLogic(bool isAttacking, Character characterMoving, Vector2 diff)
    {
        if (!isAttacking)
        {
            return !CheckIsCharacterOnTheWay(characterMoving.GetOnTileId(), characterMoving.GetCharacterStats().movementRange).Item1;
        }

        if (!characterMoving.CanAttack())
        {
            //Debug.Log("CANNOT ATTACK");
            return false;
        
        }

        if (diff.x <= characterMoving.GetCharacterStats().attackRange || diff.y <= characterMoving.GetCharacterStats().attackRange)
        {   
            var temp = CheckIsCharacterOnTheWay(characterMoving.GetOnTileId(), characterMoving.GetCharacterStats().attackRange);

            //Debug.Log("TEMP: " + temp.Item1 + " " + temp.Item2);

            if (temp.Item1 == false)
            {
                //Debug.Log("Attacking");
                characterMoving.Attack(this.character);
                characterMoving.DisableAttackAndMovement();
            }
        }

        return false;
    }

    bool CanAttackOrMoveLogicJump(bool isAttacking, Character characterMoving, Vector2 diff)
    {
        if (!isAttacking && (diff.x == characterMoving.GetCharacterStats().movementRange || diff.y == characterMoving.GetCharacterStats().movementRange))
        {
            return true;
        }

        if (!characterMoving.CanAttack())
        {
            //Debug.Log("CANNOT ATTACK");
            return false;

        }

        if (diff.x == characterMoving.GetCharacterStats().attackRange || diff.y == characterMoving.GetCharacterStats().attackRange)
        {
            Debug.Log("Attacking");
            characterMoving.Attack(this.character);
            characterMoving.DisableAttackAndMovement();
        }

        return false;
    }
}
