using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    int totalMana = 15;
    int currentMana = 15;
    TeamType teamColor;
    [SerializeField] TMP_Text manaText;

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
        manaText.text = "CURRENT MANA: " + currentMana;
    }

    public void ResetMana()
    {
        currentMana = totalMana;
        manaText.text = "CURRENT MANA: " + currentMana;
    }

    public void IncreaseTotalMana(int amount)
    {
        totalMana += amount;
        if (totalMana >= 10)
        { totalMana = 10; }
    }

    public int GetCurrentMana()
    {
        return currentMana;
    }

    public TeamType GetTeamType()
    {
        return teamColor;
    }
}
