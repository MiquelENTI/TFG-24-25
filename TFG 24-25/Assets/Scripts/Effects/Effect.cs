using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EffectStats
{
    public string name;
    public int manaCost;
    public string description;
    public EffectStats(string _name, int _manaCost, string _description)
    {
        name = _name;
        manaCost = _manaCost;
        description = _description;
    }

    public void PrintStats()
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


public class Effect
{
    protected EffectStats stats;

    public Effect(EffectStats stats)
    {
        this.stats = stats;
    }

    public virtual void OnSpawn()
    {

    }
}
