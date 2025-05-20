using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MovementType { Basic, Diagonal, Omni}
public enum AbilityType { nothing, onAttack, onKill, attacked, startTurn, onDeath, onMove, scoringTile, onScore }
public enum TeamType { RED = 0, BLUE = 1}

public class Character
{
    protected int id;
    protected CharacterStats stats;
    protected List<CellConnection> directions;
    protected TeamType teamType;
    protected int onTile;
    protected GameObject token;

    protected bool canMove = false;
    protected bool canAttack = true;
    protected bool disarmAttack = false;
    protected bool canScore = false;
    protected bool jumpMove = false;

    protected PlayerStats playerStats;
    protected ScoreManager scoreManager;

    public Character(CharacterStats newStats, TeamType teamType, GameObject token)
    {
        directions = new List<CellConnection>();

        CharactersManager.Instance.AddCharacter(this);

        stats = newStats;
        stats = (CharacterStats)newStats.Clone();

        ChangeMovementType(newStats.movementType);

        this.teamType = teamType;
        this.token = token;

        scoreManager = ScoreManager.Instance;
        playerStats = PlayerStats.Instance;
    }
    public virtual bool OnSpawn(CellNode cellToMove)
    {
        if ((cellToMove.CanCharacterSpawn(this) && !cellToMove.IsOccupied() && playerStats.GetCurrentMana() >= stats.manaCost) || CharactersManager.Instance.GetBypassMana())
        {
            Debug.Log("SPAWN");

            DisableAttackAndMovement();

            playerStats.SubstractMana(stats.manaCost);

            SetOnTileId(cellToMove.GetId());
            //Debug.Log("ONTILE: " + onTile);
            MoveToken(cellToMove.GetPosition());

            cellToMove.SetCharacter(this);

            OnSpawnSFX();
            OnSpawnVFX();

            DisplayActionsManager.Instance.CreateNameText(stats.name, token.transform.position);

            return true;
        }
        else
        {
            stats.PrintStats();
            cellToMove.PrintStatus();
            //Debug.Log("ZOMBIE??");
            return false;
        }
    }
    public virtual bool OnMovement(CellNode cellToMove)
    {
        if ((playerStats.GetCurrentMana() >= stats.manaCost && !stats.stun && canMove) || SceneManager.GetActiveScene().name == "ReplayScene")
        {
            if (cellToMove.CheckNodes(id) || CharactersManager.Instance.GetBypassMana() || SceneManager.GetActiveScene().name == "ReplayScene")
            {
                //GetCharacterStats().PrintStats();
                playerStats.SubstractMana(stats.manaCost);

                DisableAttackAndMovement();

                //Debug.Log(teamType.ToString() + " MOOOVE");

                CellNodeManager.Instance.GetNodeById(onTile).RemoveCharacter();

                SetOnTileId(cellToMove.GetId());
                MoveToken(cellToMove.GetPosition());

                cellToMove.SetCharacter(this);

                SaveData.Instance.SaveNewAction("M" + cellToMove.GetId() + "/" + id);

                OnMovementSFX();
                OnMovementVFX();

                return true;
            }
            else
            {
                MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
                Debug.Log("NO? 2");
                cellToMove.PrintStatus();
                return false;
            }
        }
        else
        {
            MoveToken(CellNodeManager.Instance.GetNodeById(onTile).GetPosition());
            Debug.Log("NO? 1");
            cellToMove.PrintStatus();
            return false;
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
    protected virtual void OnDeath(Character attacker)
    {
        CharactersManager.Instance.AddToRemoveList(this);

        attacker.OnKillEnemy(this);
    }
    protected virtual void OnDeathSelf()
    {
        CharactersManager.Instance.AddToRemoveList(this);
    }
    protected virtual void OnKillEnemy(Character enemy)
    {

    }
    protected virtual void OnAttacked(Character attacker)
    {
        ReceiveDamage(attacker, attacker.stats.dmg);
    }
    protected virtual void OnPointsScoring(int pointsScored)
    {

    }
    public virtual void Attack(Character enemy)
    {
        if (stats.stun) { return; }

        playerStats.SubstractMana(stats.manaCost);

        AttackSFX();

        enemy.OnAttacked(this);
    }
    protected void BypassMovement(CellNode cellToMove)
    {
        DisableAttackAndMovement();

        CellNodeManager.Instance.GetNodeById(onTile).RemoveCharacter();

        SetOnTileId(cellToMove.GetId());
        MoveToken(cellToMove.GetPosition());

        cellToMove.SetCharacter(this);

        OnMovementSFX();
        OnMovementVFX();
    }
    public void BypassSpawn(CellNode cellToMove)
    {
        DisableAttackAndMovement();

        SetOnTileId(cellToMove.GetId());
        MoveToken(cellToMove.GetPosition());

        cellToMove.SetCharacter(this);

        OnSpawnSFX();
        OnSpawnVFX();

        DisplayActionsManager.Instance.CreateNameText(stats.name, token.transform.position);
    }
    protected void MoveToken(Vector3 position)
    {
        position.y = 0.5f;
        token.transform.position = position;
        token.transform.GetChild(1).transform.position = position;
    }
    public bool CanAttack()
    {
        if (playerStats.GetCurrentMana() >= stats.manaCost && !disarmAttack && canAttack)
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
    protected void ChangeMovementType(MovementType newType)
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
    private void TileScoring()
    {
        CellNode node = CellNodeManager.Instance.GetNodeById(onTile);
        
        if (node.GetScoreNode() != CellScoreType.NOPOINTS)
        {
            // Prevent scoring in your own scoring tiles
            if (node.GetScoreAmount() == CellScoreAmount.ENEMYROWS && (int)node.GetSpawnable() == (int)teamType)
            { return; }

            int pointsToScore = (int)node.GetScoreAmount() * stats.scoreMultiplier;
            scoreManager.UpdateScore(teamType, pointsToScore);
            OnPointsScoring(pointsToScore);
            DisplayActionsManager.Instance.CreatePointsText(pointsToScore, token.transform.position);
        }
    }
    public void ReceiveDamage(Character attacker, int damage)
    {
        stats.hp -= damage;

        OnAttackedVFX();

        DisplayActionsManager.Instance.CreateDamageText(damage, token.transform.position);

        if (stats.hp <= 0)
        {
            OnDeath(attacker);
        }
    }
    public void ReceiveDamageSelf(int damage)
    {
        stats.hp -= damage;

        OnAttackedVFX();

        DisplayActionsManager.Instance.CreateDamageText(damage, token.transform.position);

        if (stats.hp <= 0)
        {
            OnDeathSelf();
        }
    }
    protected void DecreaseDamage(int amount)
    {
        stats.dmg -= amount;
        if (stats.dmg <= 0)
        { 
            stats.dmg = 0;
            DisplayActionsManager.Instance.CreateDebuffText(amount, true, token.transform.position);
        }
        else
        {
            DisplayActionsManager.Instance.CreateDebuffText(0, true, token.transform.position);
        }
    }
    protected void Heal(int amount)
    {
        stats.hp += amount;

        HealVFX();
        DisplayActionsManager.Instance.CreateHealText(amount, token.transform.position);

        if (stats.hp > stats.maxHp)
        {
            stats.hp = stats.maxHp;
        }
    }
    public void ApplyStun()
    {
        stats.stun = true;
    }
    public void RemoveStun()
    {
        stats.stun = false;
    }
    private void ResetStats()
    {
        canAttack = true;
        canMove = true;
    }
    public void DisableAttackAndMovement()
    {
        canAttack = false;
        canMove = false;
    }
    public void DestroyCharacter()
    {
        CharactersManager.Instance.AddToRemoveList(this);
    }

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

    public GameObject GetToken()
    { return token; }

    public CharacterStats GetCharacterStats()
    { return stats; }

    public bool GetJumpMove()
    { return jumpMove; }

    public void SetDisarmAttack(bool state)
    {
        disarmAttack = state;
    }

    //Implementació del so general per a cartes no identificades 
    protected virtual void OnMovementSFX()
    {
        SoundManager.Instance.PlaySFX(003000002, token.transform.position);
    }

    protected virtual void OnMovementVFX(string vfxName = "Stomp")
    {
        VFXManager.Instance.PlayVFXInPosition(vfxName, token.transform.position);
    }
        
    protected virtual void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003000000, token.transform.position);
    }
    
    protected virtual void OnAttackedVFX()
    {
        VFXManager.Instance.PlayVFXInPosition("Sparks", token.transform.position);
        VFXManager.Instance.PlayVFXInPosition("Explosion", token.transform.position);
    }

    protected virtual void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003000001, token.transform.position);
    }

    protected virtual void OnSpawnVFX(string vfxName = "Stomp")
    {
        VFXManager.Instance.PlayVFXInPosition(vfxName, token.transform.position);
    }

    protected virtual void HealVFX(string vfxName = "Heal")
    {
        VFXManager.Instance.PlayVFXInPosition(vfxName, token.transform.position);
    }
}