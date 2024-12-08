using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards
{
    protected bool isCharacter = true;
    public Cards(bool isCharacter) { this.isCharacter = isCharacter; }

    public virtual CharacterStats GetCharacterStats()
    {
        return new CharacterStats();
    }



    public bool IsCharacter()
    { return isCharacter; }

}
