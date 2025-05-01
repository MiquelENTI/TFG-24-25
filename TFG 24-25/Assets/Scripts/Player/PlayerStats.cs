using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    int totalMana = 15;
    int currentMana = 15;
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

    public int GetCurrentMana()
    {
        return currentMana;
    }
}
