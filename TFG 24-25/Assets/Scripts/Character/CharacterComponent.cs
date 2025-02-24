using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterComponent : MonoBehaviour
{
    [SerializeField] private Sprite characterSprite;
    private Character character;

    Button testButton;

    public Sprite CharacterSprite => characterSprite;

    public Character GetCharacter()
    {
        return character;
    }

    public void InitializeCharacter(CharacterStats stats, TeamType teamType, GameObject token)
    {
        character = new Character(stats, teamType, token, characterSprite);
    }

    InputField testInputField;


    
}
