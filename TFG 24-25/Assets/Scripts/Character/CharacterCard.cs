using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCard : Cards
{
    
    CharacterStats stats;
    public CharacterCard(CharacterStats stats) : base(true) 
    { 
        this.stats = stats;
    }
    
    public override CharacterStats GetCharacterStats()
    {  return stats; }

}
