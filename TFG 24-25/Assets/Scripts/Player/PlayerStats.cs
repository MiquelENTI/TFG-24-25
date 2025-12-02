using TMPro;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    private int totalMana = 15;
    private int currentMana = 15;

    [SerializeField] private TMP_Text[] manaBottleTexts = new TMP_Text[2];

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
