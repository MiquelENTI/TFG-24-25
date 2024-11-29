using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    Dictionary<int, CharacterStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, Movement type
        { 1, new CharacterStats (0, 5, 8,  1000, MovementType.Basic) }, // Cavalier
        { 5, new CharacterStats (1, 1, 1,  2,    MovementType.Omni) },  // Fly
        { 13, new CharacterStats(3, 6, 23, 1000, MovementType.Basic) }, // Mummy
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
