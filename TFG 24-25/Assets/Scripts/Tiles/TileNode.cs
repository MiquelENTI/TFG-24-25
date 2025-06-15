using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileConnection { UP = 1, DOWN = -1, RIGHT = 2, LEFT = -2, UPRIGHT = 3, DOWNLEFT = -3, UPLEFT = 4, DOWNRIGHT = -4 }
public enum TileScoreType { NOPOINTS = 0, POINTS = 1 }
public enum TileScoreAmount { NORMAL = 1, ENEMYROWS = 2 }
public enum TileSpawnable { NONE = -1, RED = 0, BLUE = 1}
public class TileNode
{
    private int id;
    private Vector3 position;
    private Vector2 positionInGrid;
    private Character character;
    private bool isOccupied = false;

    private TileSpawnable spawnable = TileSpawnable.NONE;
    private TileScoreType scoreType = TileScoreType.NOPOINTS;
    private TileScoreAmount scoreAmount = TileScoreAmount.NORMAL;

    private Dictionary<TileConnection, TileNode> connectionDictionary = new();

    public TileNode(Vector3 position, int x, int y)
    {  
        this.position = position;
        positionInGrid = new Vector2(x, y);

        TileNodeManager.Instance.AddNode(this);
    }

    public bool CheckConnectionNode(TileConnection cellConnection)
    {
        return connectionDictionary.ContainsKey(cellConnection);
    }
    public void AddConnection(TileConnection cellConnection, TileNode cellNode)
    {
        connectionDictionary.Add(cellConnection, cellNode);
    }

    public bool CheckNodes(int characterId)
    {
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

        TileNode characterTile = TileNodeManager.Instance.GetNodeById(characterMoving.GetOnTileId());

        Vector2 diff = positionInGrid - characterTile.positionInGrid;

        diff.x = Mathf.Abs(diff.x);
        diff.y = Mathf.Abs(diff.y);

        //Debug.Log("Diff on Enemy and Character: " + diff + "POSITIONS: " + positionInGrid + " - " + characterTile.positionInGrid);

        if ((diff.x > characterMoving.GetCharacterStats().movementRange || diff.y > characterMoving.GetCharacterStats().movementRange) && !isAttacking)
        {
            Debug.Log("ENTERED IF RANGE");
            return false;
        }

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
    private bool CanAttackOrMoveLogic(bool isAttacking, Character characterMoving, Vector2 diff)
    {
        if (!isAttacking)
        {
            // If There is     Character On Way returns true  -> fun returns false meaning that character cannot move, its for characters with movementRange > 1
            // If there is not Character on Way returns false -> fun returns true  meaning its clear to move

            return !CheckIsCharacterOnTheWay(characterMoving.GetOnTileId(), characterMoving.GetCharacterStats().movementRange).Item1;
        }

        if (!characterMoving.CanAttack())
        {
            return false;

        }

        if (diff.x <= characterMoving.GetCharacterStats().attackRange || diff.y <= characterMoving.GetCharacterStats().attackRange)
        {
            var temp = CheckIsCharacterOnTheWay(characterMoving.GetOnTileId(), characterMoving.GetCharacterStats().attackRange);

            if (temp.Item1 == false)
            {
                characterMoving.Attack(this.character);
                characterMoving.DisableAttackAndMovement();
            }
        }

        return false;
    }
    private bool CanAttackOrMoveLogicJump(bool isAttacking, Character characterMoving, Vector2 diff)
    {
        if (!isAttacking && (diff.x == characterMoving.GetCharacterStats().movementRange || diff.y == characterMoving.GetCharacterStats().movementRange))
        {
            return true;
        }

        if (!characterMoving.CanAttack())
        {
            return false;

        }

        if (diff.x == characterMoving.GetCharacterStats().attackRange || diff.y == characterMoving.GetCharacterStats().attackRange)
        {
            characterMoving.Attack(this.character);
            characterMoving.DisableAttackAndMovement();
        }

        return false;
    }
    private Tuple<bool, int> CheckIsCharacterOnTheWay(int cellId, int range)
    {
        Tuple<int, TileConnection> direction = GetDirectionToTileByRange(cellId, range);
        if (direction.Item1 == 0) { return Tuple.Create(true, 0); }

        // Debug.Log("Direction: " + direction.Item1 + " " + direction.Item2);

        TileNode cellToCheck = GetTileByDirection(direction.Item2);

        for (int i = 1; i < direction.Item1; i++)
        {
            //Debug.Log("Checking in Distance " + i);
            if (cellToCheck.GetCharacter() != null)
            {
                //Debug.Log("Encountered Character at " + i);
                return Tuple.Create(true, i);
            }
            cellToCheck = cellToCheck.GetTileByDirection(direction.Item2);
        }
        //Debug.Log("NO Encounter");
        return Tuple.Create(false, 0);
    }
    private Tuple<int, TileConnection, Character> CheckForCharacterOnTheWay(int cellId, int range)
    {
        Tuple<int, TileConnection> direction = GetDirectionToTileByRange(cellId, range);

        TileNode cellToCheck = GetTileByDirection(direction.Item2);

        for (int i = 1; i < direction.Item1; i++)
        {
            if (cellToCheck.GetCharacter() != null)
            {
                return Tuple.Create(i, direction.Item2, cellToCheck.character);
            }
            cellToCheck = GetTileByDirection(direction.Item2);
        }
        return null;
    }
    public bool CanCharacterSpawn(Character character)
    {
        return (TileSpawnable)character.GetTeamType() == spawnable && this.character == null;
    }
    public void RemoveCharacter()
    {
        character = null;
        isOccupied = false;
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
                " |  Spawnable: " + spawnable.ToString() +
                " |  ScoreMode: " + scoreType.ToString() +
                " |  ScoreAmount: " + scoreAmount.ToString());
        }
    }

    public TileNode GetTileByDirection(TileConnection cellConnection)
    {
        return connectionDictionary[cellConnection];
    }
    private TileConnection GetInverseDirection(int cellConnection)
    {
        return (TileConnection)(-cellConnection);
    }
    public TileNode GetTileByInverseDirection(int cellConnection)
    {
        if (CheckConnectionNode(GetInverseDirection(cellConnection)))
        {
            return connectionDictionary[GetInverseDirection(cellConnection)];
        }
        return null;
    }
    public List<TileNode> GetSurrondingTiles()
    {
        List<TileNode> temp = new List<TileNode>();

        foreach (var cellNode in connectionDictionary)
        {
            temp.Add(cellNode.Value);
        }

        return temp;
    }
    public Tuple<int, TileConnection> GetDirectionToTileByRange(int cellId, int range)
    {
        foreach (var direction in connectionDictionary)
        {
            TileNode nextTile = this;
            for (int i = 0; i < range; i++)
            {
                // Tile To Check
                if (!nextTile.CheckConnectionNode(direction.Key))
                {
                    break;
                }

                nextTile = nextTile.GetTileByDirection(direction.Key);
                if (nextTile.id == cellId)
                {
                    nextTile.PrintStatus();
                    return Tuple.Create(i + 1, direction.Key);
                }
            }
        }
        return Tuple.Create(0, TileConnection.UP);
    }

    public void SetId(int id)
    { this.id = id; }
    public int GetId()
    { return id; }
    public Vector3 GetPosition()
    { return position; }
    public Vector2 GetPositionInGrid()
    { return positionInGrid; }
    public bool IsOccupied()
    { return isOccupied; }
    public void SetCharacter(Character character)
    { 
        this.character = character; 
        isOccupied = true;
    }
    public Character GetCharacter()
    {
        if (character != null)
            return character;
        else
            return null;
    }
    public void SetSpawnable(TileSpawnable spawnable)
    { this.spawnable = spawnable; }
    public TileSpawnable GetSpawnable()
    { return spawnable; }
    public void SetScoreNode(TileScoreType type)
    { scoreType = type; }
    public TileScoreType GetScoreNode()
    { return scoreType; }
    public void SetScoreAmount(TileScoreAmount type)
    { scoreAmount = type; }
    public TileScoreAmount GetScoreAmount()
    { return scoreAmount; }
}