using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    Dictionary<int, CharacterStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, Movement type
        { 1, new CharacterStats (0, 5, 8,  1000, MovementType.Basic) }, // Cavalier
        { 3, new CharacterStats (4, 9, 13, 1000, MovementType.Omni)  }, // Salmon
        { 5, new CharacterStats (1, 1, 1,  1000, MovementType.Omni)  }, // Fly
        { 6, new CharacterStats (2, 5, 18, 1000, MovementType.Basic) }, // Turtle (WIP)
        { 7, new CharacterStats (2, 3, 6,  1000, MovementType.Basic) }, // Mimic  (WIP)
        { 10, new CharacterStats(3, 3, 12, 1000, MovementType.Basic) }, // Medusa
        { 13, new CharacterStats(3, 6, 23, 1000, MovementType.Basic) }, // Mummy
        { 16, new CharacterStats(3, 14, 4, 1000, MovementType.Basic) }, // Death Horseman
        { 17, new CharacterStats(4, 4, 21, 1000, MovementType.Basic) }, // Terracotta Warrior
        { 20, new CharacterStats(5, 7, 15, 1000, MovementType.Basic) }, // Magic Karp
        { 22, new CharacterStats(1, 2, 7,  1000, MovementType.Basic) }, // Chicken
    };


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
