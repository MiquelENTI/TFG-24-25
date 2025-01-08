using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public enum MovementType { Basic, Diagonal, Omni}
public enum TeamType { RED = 0, BLUE = 1}

public struct CharacterStats
{
    public string name;
    public int hp;
    public int dmg;
    public int manaCost;
    public int numOfMovements;
    public MovementType movementType;
    public bool stun;
    public string description;

    public CharacterStats(string _name, int _manaCost, int _dmg, int _hp, int _numOfMovements, MovementType _movementType, string _description)
    {
        name = _name;
        hp = _hp;
        dmg = _dmg;
        manaCost = _manaCost;
        numOfMovements = _numOfMovements;
        movementType = _movementType;
        stun = false;
        description = _description;
    }

    public void PrintStats()
    {
        Debug.Log
        (
        "Name : " + name +
        " | HP: " + hp +
        " | DMG: " + dmg +
        " | Mana Cost: " + manaCost +
        " | Num Of Movementst: " + numOfMovements +
        " | Movement Type: " + movementType.ToString()
        );
    }

    public string getName()
    { return name; }

    public int getHealth()
    {
        return hp;
    }

    public int getAttack()
    {
        return dmg;
    }

    public int getManaCost()
    {
        return manaCost;
    }

    public string getDescription()
    {
        return description;
    }
}

public class Character
{

    protected ScoreManager scoreManager;
    protected int id;
    protected CharacterStats stats;
    protected List<CellConnection> directions;

    protected TeamType teamType;
    protected int onTile;
    protected int previousTile;

    protected bool toSpawn = true;
    protected bool canAttack = true;

    protected GameObject token;
    [SerializeField] public Sprite cardSprite;
    [SerializeField] public string description;
    protected PlayerStats playerStats;

    bool canScore = false;
    bool inScoreTile14 = false;
    bool inScoreTile15 = false;
    bool inScoreTile20 = false;
    bool inScoreTile21 = false;


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

    //public int GetMovementsLeft()
    //{ return stats.movementsLeft; }

    //public void DecreaseMovement()
    //{ stats.movementsLeft--; }
    //void ResetMovementsLeft()
    //{ stats.movementsLeft = stats.numOfMovements; }

    public bool IsToSpawn() 
    { return toSpawn; }

    public void DisableSpawn() 
    { toSpawn = false; }

    public bool CanAttack()
    {
        if (playerStats.GetCurrentMana() >= stats.manaCost)
        {
            return canAttack;
        }
        else
        { 
            return false;
        }
    }

    public void MoveToken(Vector3 position)
    {  
        position.y = 0.5f;
        token.transform.position = position;
    }
    
    public GameObject GetToken()
    { return token; }

    public Sprite GetCardSprite()
    { return cardSprite; }

    public int GetHealth()
    {
        return stats.getHealth();
    }

    public int GetAttack()
    {
        return stats.getAttack();
    }

    public string GetDescription()
    {
        return stats.getDescription();
    }

    public int getManaCost()
    {
        return stats.getManaCost();
    }

    public Character(CharacterStats newStats, TeamType teamType, GameObject token, Sprite cardSprite)
    {
        GameObject scoreManagerObject = GameObject.Find("ScoreManager");
        if (scoreManagerObject != null)
        {
            scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
        }
        else
        {
            Debug.LogError("No se encontró el GameObject 'ScoreManager' en la escena.");
        }
        playerStats = PlayerStats.Instance;
        stats = newStats;
        directions = new List<CellConnection>();

        ChangeMovementType(newStats.movementType);

        this.teamType = teamType;

        CharactersManager.Instance.AddCharacter(this, cardSprite);
        this.token = token;
        this.cardSprite = cardSprite;
    }

    public void ResetStats()
    {
        //ResetMovementsLeft();
        canAttack = true;
    }

    public CharacterStats GetCharacterStats()
    {
        return stats;
    }

    public void ChangeMovementType(MovementType newType)
    {
        directions.Clear();

        switch (newType)
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
    }


    public virtual void Effect()
    {

    }

    public virtual void OnSpawn(CellNode cellToMove)
    {
        if (cellToMove.CanCharacterSpawn(this) && !cellToMove.IsOccupied() && playerStats.GetCurrentMana() >= stats.manaCost)
        {
            
            Debug.Log("SPAWN");
            DisableSpawn();

            playerStats.SubstractMana(stats.manaCost);

            SetOnTileId(cellToMove.GetId());
            previousTile = onTile;
            MoveToken(cellToMove.GetPosition());

            cellToMove.SetCharacter(this);
        }
    }
    
    public virtual void OnMovement(CellNode cellToMove)
    {
        if (cellToMove.CheckNodes(GetDirections(), id) && playerStats.GetCurrentMana() >= stats.manaCost && !stats.stun)
        {
            playerStats.SubstractMana(stats.manaCost);
            canAttack = false;

            Debug.Log(teamType.ToString() + " MOOOVE");

            CellNode previousNode = CellNodeManager.Instance.GetNodeById(previousTile);

            previousTile = onTile;
            SetOnTileId(cellToMove.GetId());
            MoveToken(cellToMove.GetPosition());

            cellToMove.SetCharacter(this);
            previousNode.RemoveCharacter();

            //previousNode.PrintStatus();
            cellToMove.PrintStatus();

            inScoreTile14 = false;
            inScoreTile15 = false;
            inScoreTile20 = false;
            inScoreTile21 = false;
        }
        else
        {
            MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
            Debug.Log("NO?");
            cellToMove.PrintStatus();
        }
    }

    protected void BypassMovement(int otherTileId)
    {
        canAttack = false; // Delete or not?

        Debug.Log(teamType.ToString() + " BYPASSED MOOOVE");

        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(otherTileId);
        CellNode previousNode = CellNodeManager.Instance.GetNodeById(onTile);

        previousTile = onTile;
        SetOnTileId(cellToMove.GetId());
        MoveToken(cellToMove.GetPosition());

        cellToMove.SetCharacter(this);
        previousNode.RemoveCharacter();
    }

    void TileScoring()
    {
        CellNode node = CellNodeManager.Instance.GetNodeById(onTile);

        switch (node.GetScoreNode())
        {
            case CellScoreType.NORMAL:
                if (canScore && previousTile == onTile)
                {
                    scoreManager.UpdateScore(teamType);
                    canScore = false;
                    return;
                }
                canScore = true;
            break;

            case CellScoreType.QUICK:
                scoreManager.UpdateScore(teamType);
            break;

            default:
                break;
        }
    }

    public virtual void OnStartTurn()
    {
        TileScoring();
        Debug.Log("OnStartPassed");
        //if (onTile == 14)
        //{
        //    if (inScoreTile14)
        //    {
        //        scoreManager.UpdateScore(teamType);
        //        inScoreTile14 = false;
        //        return;
        //    }
        //    inScoreTile14 = true;
        //}
        //else if (onTile == 15)
        //{
        //    if (inScoreTile15)
        //    {
        //        scoreManager.UpdateScore(teamType);
        //        inScoreTile15 = false;
        //        return;
        //    }
        //    inScoreTile15 = true;
        //}
        //else if (onTile == 20)
        //{
        //    if (inScoreTile20)
        //    {
        //        scoreManager.UpdateScore(teamType);
        //        inScoreTile20 = false;
        //        return;
        //    }
        //    inScoreTile20 = true;
        //}
        //else if (onTile == 21)
        //{
        //    if (inScoreTile21)
        //    {
        //        scoreManager.UpdateScore(teamType);
        //        inScoreTile21 = false;
        //        return;
        //    }
        //    inScoreTile21 = true;
        //}
    }
    public virtual void OnEndTurn()
    {

    }

    public virtual void OnDeath(Character attacker)
    {
        CellNode currentCell = CellNodeManager.Instance.GetNodeById(onTile);
        currentCell.RemoveCharacter();
        CharactersManager.Instance.RemoveCharacter(id);
    }
    public virtual void OnKillEnemy(Character enemy)
    {

    }
    public virtual void OnAttack(Character attacker)
    {

    }
    public virtual void Attack(Character enemy)
    {
        if (stats.stun) { return; }

        playerStats.SubstractMana(stats.manaCost);

        Debug.Log("ATTACK");
        enemy.stats.hp -= stats.dmg;

        enemy.OnAttack(this);
        if(enemy.stats.hp <= 0)
        {
            OnKillEnemy(enemy);
            enemy.OnDeath(this);
        }
    }
    
    // Maybe Character parameter?
    public void ReceiveDamage(int damage)
    {
        stats.hp -= damage;

        if (stats.hp <= 0)
        {
            OnDeath(this);
        }
        Debug.Log("RECEIVED DAMAGE:" + damage + " HP LEFT: " + stats.hp);
    }
    public void DecreaseDamage(int amount)
    {
        stats.dmg -= amount;
        if (stats.dmg <= 0)
        { stats.dmg = 0; }
    }

    public void ApplyStun()
    {
        stats.stun = true;
    }

    public void RemoveStun()
    {
        stats.stun = false;
    }

    public void ApplyDOT()
    {

    }
}
