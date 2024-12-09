using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    Dictionary<int, CharacterStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, Movement type, Card Text
        { 1, new CharacterStats ("Cavalier", 0, 5, 8,  1000, MovementType.Basic, "Esto es un Cavalier") }, // Cavalier
        { 3, new CharacterStats ("Salmon", 4, 9, 13, 1000, MovementType.Omni, "Esto es un Salmon")  }, // Salmon
        { 5, new CharacterStats ("Fly",1, 1, 1,  1000, MovementType.Omni, "Esto es un Fly")  }, // Fly
        { 6, new CharacterStats ("Turtle",2, 5, 18, 1000, MovementType.Basic, "Esto es una Turtle") }, // Turtle (WIP)
        { 7, new CharacterStats ("Mimic",2, 3, 6,  1000, MovementType.Basic, "Esto es un Mimic") }, // Mimic  (WIP)
        { 10, new CharacterStats("Medusa",3, 3, 12, 1000, MovementType.Basic, "Esto es una Medusa") }, // Medusa
        { 13, new CharacterStats("Mummy",3, 6, 23, 1000, MovementType.Basic, "Esto es una Mummy") }, // Mummy
        { 14, new CharacterStats("The Gun",3, 8, 9, 1000, MovementType.Omni, "Esto es una The Gun") }, // The Gun
        { 15, new CharacterStats("Human Werewolf",3, 4, 8, 1000, MovementType.Basic, "Esto es una The Gun") }, // The Gun
        { 16, new CharacterStats("Death Horseman",3, 14, 4, 1000, MovementType.Basic, "Esto es un Death Horseman") }, // Death Horseman
        { 17, new CharacterStats("Terracotta Warrior",4, 4, 21, 1000, MovementType.Basic, "Esto es un Terracotta Warrior") }, // Terracotta Warrior
        { 20, new CharacterStats("Magic Karp",5, 7, 15, 1000, MovementType.Basic, "Esto es un Magic Karp") }, // Magic Karp
        { 22, new CharacterStats("Chicken",1, 2, 7,  1000, MovementType.Basic, "Esto es un Chicken") }, // Chicken
        { -1, new CharacterStats("Dummy",0, 0, 99999, 0, MovementType.Basic, "") }, // DummyBase
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

    public CharacterCard GetCharacter(int characterId)
    {
        return new CharacterCard(characterStats[characterId]);
    }
}
