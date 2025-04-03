using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CardType { CHARACTER, EFFECT }
public class CardStats
{

    public CardType cardType;
    public string name;
    public int manaCost;
    public string description;

    public virtual void PrintStats()
    {
        Debug.Log
        (
        "Name : " + name +
        " | Mana Cost: " + manaCost
        );
    }

    public string getName()
    { return name; }
    public int getManaCost()
    {
        return manaCost;
    }
    public string getDescription()
    {
        return description;
    }
}

public class CharacterStats : CardStats
{
    public int hp;
    public int maxHp;
    public int dmg;
    public int movementRange;
    public int attackRange;
    public MovementType movementType;
    public bool stun;
    public bool checkAllInRangeCells;
    public int scoreMultiplier;
    public CharacterStats(CardType _cardType, string _name, int _manaCost, int _dmg, int _hp, int _movementRange, int _attackRange, int _scoreMult, MovementType _movementType, string _description)
    {
        cardType = _cardType;
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

    public override void PrintStats()
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
}

public class EffectStats : CardStats
{
   public EffectStats(CardType _cardType, string _name, int _manaCost, string _description)
    {
        cardType = _cardType;
        name = _name;
        manaCost = _manaCost;
        description = _description;
    }

    public override void PrintStats()
    {
        Debug.Log
        (
        "Name : " + name +
        " | Mana Cost: " + manaCost
        );
    }
}