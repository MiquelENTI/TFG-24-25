using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    Dictionary<int, CharacterStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, Movement type, Card Text
        { 1, new CharacterStats (0, 5, 8,  1000, MovementType.Basic, "Esto es un Cavalier") }, // Cavalier
        { 3, new CharacterStats (4, 9, 13, 1000, MovementType.Omni, "Esto es un Salmon")  }, // Salmon
        { 5, new CharacterStats (1, 1, 1,  1000, MovementType.Omni, "Esto es un Fly")  }, // Fly
        { 6, new CharacterStats (2, 5, 18, 1000, MovementType.Basic, "Esto es una Turtle") }, // Turtle (WIP)
        { 7, new CharacterStats (2, 3, 6,  1000, MovementType.Basic, "Esto es un Mimic") }, // Mimic  (WIP)
        { 10, new CharacterStats(3, 3, 12, 1000, MovementType.Basic, "Esto es una Medusa") }, // Medusa
        { 13, new CharacterStats(3, 6, 23, 1000, MovementType.Basic, "Esto es una Mummy") }, // Mummy
        { 16, new CharacterStats(3, 14, 4, 1000, MovementType.Basic, "Esto es un Death Horseman") }, // Death Horseman
        { 17, new CharacterStats(4, 4, 21, 1000, MovementType.Basic, "Esto es un Terracotta Warrior") }, // Terracotta Warrior
        { 20, new CharacterStats(5, 7, 15, 1000, MovementType.Basic, "Esto es un Magic Karp") }, // Magic Karp
        { 22, new CharacterStats(1, 2, 7,  1000, MovementType.Basic, "Esto es un Chicken") },
        { -1, new CharacterStats(0, 0, 99999, 0, MovementType.Basic, "") }, // DummyBase
    };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CharacterStats GetTemporalStats(int id)
    {
        return characterStats[id];
    }

    public List<CharacterCard> GetAllCharacters(int copies)
    {
        List<CharacterCard> temp = new();
        for (int i = 0; i < copies; i++)
        {
            foreach (CharacterStats stats in characterStats.Values)
            {
                temp.Add(new CharacterCard(stats));
            }
        }
        return temp;
    }

        
}
