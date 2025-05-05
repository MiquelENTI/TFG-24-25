using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    int totalMana = 15;
    int currentMana = 15;

    [SerializeField] TMP_Text[] manaBottleTexts = new TMP_Text[2];

    private void Start()
    {
        manaBottleTexts = GameObject.FindGameObjectWithTag("ManaBottleCanvas").GetComponentsInChildren<TMP_Text>();
    }

    public void SubstractMana(int amount)
    {
        currentMana -= amount;
        manaBottleTexts[0].text = currentMana.ToString();
        manaBottleTexts[1].text = currentMana.ToString();
    }

    public void ResetMana()
    {
        currentMana = totalMana;
        manaBottleTexts[0].text = currentMana.ToString();
        manaBottleTexts[1].text = currentMana.ToString();
    }

    public int GetCurrentMana()
    {
        return currentMana;
    }
}
