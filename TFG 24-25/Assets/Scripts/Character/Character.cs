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
    public int maxHp;
    public int dmg;
    public int manaCost;
    public int movementRange;
    public int attackRange;
    public MovementType movementType;
    public bool stun;
    public string description;
    public bool checkAllInRangeCells;
    public int scoreMultiplier;
    public CharacterStats(string _name, int _manaCost, int _dmg, int _hp, int _movementRange, int _attackRange, int _scoreMult, MovementType _movementType, string _description)
    {
        name = _name;
        maxHp = _hp;
        hp = _hp;
        dmg = _dmg;
        manaCost = _manaCost;
        movementRange = _movementRange;
        attackRange = _attackRange;
        scoreMultiplier = _scoreMult;
        movementType = _movementType;
        stun = false;
        description = _description;
        checkAllInRangeCells = false;
    }

    public void PrintStats()
    {
        Debug.Log
        (
        "Name : " + name +
        " | HP: " + hp +
        " | DMG: " + dmg +
        " | Mana Cost: " + manaCost +
        " | Movement Range: " + movementRange +
        " | Attack Range: " + attackRange +
        " | Movement Type: " + movementType.ToString()
        );
    }

    public string getName()
    { return name; }

    public string getHealthToString()
    {
        return hp.ToString();
    }

    public int getHealth()
    {
        return hp;
    }

    public string getAttackToString()
    {
        return dmg.ToString();
    }

    public int getManaCostToString()
    {
        return manaCost;
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

    protected bool toSpawn = true;
    protected bool canAttack = true;
    protected bool disarmAttack = false;

    protected GameObject token;
    protected PlayerStats playerStats;

    bool canScore = false;
    bool canMove = false;

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
        if (playerStats.GetCurrentMana() >= stats.manaCost && !disarmAttack)
        {
            return true;
        }
        else if (CharactersManager.Instance.GetBypassMana())
        {
            return true;
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
        token.transform.parent.GetChild(1).transform.position = position;
    }
    
    public GameObject GetToken()
    { return token; }

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

    public string GetName()
    {
        return stats.getName();
    }

    public int getManaCost()
    {
        return stats.getManaCost();
    }

    public Character(CharacterStats newStats, TeamType teamType, GameObject token)
    {
        GameObject scoreManagerObject = GameObject.Find("ScoreManager");
        if (scoreManagerObject != null)
        {
            scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
        }
        else
        {
            Debug.LogError("No se encontr� el GameObject 'ScoreManager' en la escena.");
        }
        playerStats = PlayerStats.Instance;
        stats = newStats;
        directions = new List<CellConnection>();

        ChangeMovementType(newStats.movementType);

        this.teamType = teamType;

        CharactersManager.Instance.AddCharacter(this);
        this.token = token;
    }

    public void ResetStats()
    {
        canAttack = true;
        canMove = true;
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
        if ((cellToMove.CanCharacterSpawn(this) && !cellToMove.IsOccupied() && playerStats.GetCurrentMana() >= stats.manaCost) || CharactersManager.Instance.GetBypassMana())
        {
            Debug.Log("SPAWN");

            DisableAttackAndMovement();

            DisableSpawn();

            playerStats.SubstractMana(stats.manaCost);

            SetOnTileId(cellToMove.GetId());
            //Debug.Log("ONTILE: " + onTile);
            MoveToken(cellToMove.GetPosition());
            
            cellToMove.SetCharacter(this);

            OnSpawnSFX();
        }
    }
    
    public virtual bool OnMovement(CellNode cellToMove)
    {
        if ((cellToMove.CheckNodes(id) && playerStats.GetCurrentMana() >= stats.manaCost && !stats.stun && canMove) || CharactersManager.Instance.GetBypassMana())
        {
            //GetCharacterStats().PrintStats();
            playerStats.SubstractMana(stats.manaCost);

            DisableAttackAndMovement();

            //Debug.Log(teamType.ToString() + " MOOOVE");

            CellNodeManager.Instance.GetNodeById(onTile).RemoveCharacter();

            SetOnTileId(cellToMove.GetId());
            MoveToken(cellToMove.GetPosition());

            cellToMove.SetCharacter(this);

            //Debug.Log("PREVIOUS TILE: " + previousTile + " OnTile" + onTile + " FUTURE TILE" + cellToMove.GetId());
            //Debug.Log("Previous After");
            //previousNode.PrintStatus();
            //Debug.Log("OnTIle AFter");
            //CellNodeManager.Instance.GetNodeById(onTile).PrintStatus();
            //Debug.Log("CellToMove After");
            //cellToMove.PrintStatus();

            OnMovementSFX();
            return true;
        }
        else
        {
            MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
            Debug.Log("NO?");
            cellToMove.PrintStatus();

            

            return false;
        }
    }

    protected void BypassMovement(int otherTileId)
    {
        canAttack = false; // Delete or not?

        Debug.Log(teamType.ToString() + " BYPASSED MOOOVE");

        CellNode cellToMove = CellNodeManager.Instance.GetNodeById(otherTileId);
        CellNode previousNode = CellNodeManager.Instance.GetNodeById(onTile);

        SetOnTileId(cellToMove.GetId());
        MoveToken(cellToMove.GetPosition());

        cellToMove.SetCharacter(this);
        previousNode.RemoveCharacter();
    }

    void TileScoring()
    {
        CellNode node = CellNodeManager.Instance.GetNodeById(onTile);

        Debug.Log("ENTERING TILE SCORING");


        if (node.GetScoreNode() == CellScoreType.QUICK)
        {
            // Prevent scoring in your own scoring tiles
            if (node.GetScoreAmount() == CellScoreAmount.ENEMYROWS && (int)node.GetSpawnable() == (int)teamType)
            { return; }

            int pointsToScore = (int)node.GetScoreAmount() * stats.scoreMultiplier;
            scoreManager.UpdateScore(teamType, pointsToScore);
            OnPointsScoring(pointsToScore);
        }
    }

    public virtual void OnStartTurn()
    {
        ResetStats();
        TileScoring();
    }
    public virtual void OnEndTurn()
    {

    }

    public virtual void OnDeath(Character attacker)
    {
        MyEventHandler.Instance.InvokeSamuraiEffect(teamType);

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

    public virtual void OnPointsScoring(int pointsScored)
    {

    }
    public virtual void Attack(Character enemy)
    {
        if (stats.stun) { return; }

        playerStats.SubstractMana(stats.manaCost);

        Debug.Log("ATTACK");
        enemy.stats.hp -= stats.dmg;

        AttackSFX();

        enemy.OnAttack(this);
        if(enemy.stats.hp <= 0)
        {
            OnKillEnemy(enemy);
            enemy.OnDeath(this);
        }
    }
    
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

    public void Heal(int amount)
    {
        stats.hp += amount;

        if (stats.hp > stats.maxHp)
        {
            stats.hp = stats.maxHp;
        }
    }

    public void ApplyStun()
    {
        stats.stun = true;
        Debug.Log("CHARACTER STUNNED!");
    }

    public void RemoveStun()
    {
        stats.stun = false;
        Debug.Log("CHARACTER NOT STUNNED!");
    }

    public void SetDisarmAttack(bool state)
    {
        disarmAttack = state;
    }

    public void DisableAttackAndMovement()
    {
        canAttack = false;
        canMove = false;
    }

    public void ApplyDOT()
    {

    }
    //Implementació del so general per a cartes no identificades 
    protected virtual void OnMovementSFX()
    {
        SoundManager.Instance.PlaySFX(003000002, token.transform.position);

    }
        
    protected virtual void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003000000, token.transform.position);
    }
    
    protected virtual void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003000001, token.transform.position);
    }
}
