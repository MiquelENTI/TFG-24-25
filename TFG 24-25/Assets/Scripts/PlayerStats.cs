using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    int totalMana = 5;
    int currentMana = 5;
    int playerId;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubstractMana(int amount)
    {
        currentMana -= amount;
    }

    public void ResetMana()
    {
        currentMana = totalMana;
    }

    public void IncreaseTotalMana(int amount)
    {
        totalMana += amount;
    }

    public int GetCurrentMana()
    {
        return currentMana;
    }
}
