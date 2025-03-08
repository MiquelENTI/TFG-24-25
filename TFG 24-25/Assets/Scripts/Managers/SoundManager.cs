using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // Listado de efectos de sonidos de CARTAS
    Dictionary<int, string> sfxDictionary;
    void Awake ()
    {
        sfxDictionary=new()
        {
        //CARDS
        {9999,},
        {9999,},
           //Salmon
           {00,"event:/SFX/UI/CARDS/SALMON/Salmon_selection"},
           {01,},
           //Fly
           {02,"event:/SFX/UI/CARDS/FLY/Fly_Attack"},
           {03,"event:/SFX/UI/CARDS/FLY/Fly_Selection"},
           //Turtle
           {04,"event:/SFX/UI/CARDS/TURTLE/Turtle_select"},
           {05,},
           // Mimic
           {06,"event:/SFX/UI/CARDS/MIMIC/Mimic_Attack"},
           {07,"event:/SFX/UI/CARDS/MIMIC/Mimic_Selecction"},
           // Pichí
           {08,"event:/SFX/UI/CARDS/PICHI/Pichi_Attack"},
           {09,"event:/SFX/UI/CARDS/PICHI/Pichi_Selection"},
           // Medusa
           {10,"event:/SFX/UI/CARDS/MEDUSA/Medusa_Attack"},
           {11,"event:/SFX/UI/CARDS/MEDUSA/Medusa_selection"},
           // Mummy
           {12,"event:/SFX/UI/CARDS/MUMMY/Mummy_Attack"},
           {13,"event:/SFX/UI/CARDS/MUMMY/Mummy_Selecction"},
           // The Gun
           {14,"event:/SFX/UI/CARDS/THE GUN/TheGun_Attack"},
           {15,"event:/SFX/UI/CARDS/THE GUN/TheGun_selection"},
           // Human/Werewolf
           {16,},
           {17,"event:/SFX/UI/CARDS/WAREWOLF/Howl"},
           // Death Horseman
           {18,},
           {19,"event:/SFX/UI/CARDS/DEATHHORSEMAN/DeathHorseman_select"},
           // Terracotta Warrior
           {20,},
           {21,"event:/SFX/UI/CARDS/TARRACOTA_WARRIOR/Tarracota_select"},
           // Fantasma
           {22,},
           {23,},
           // Grasshopper
           {24,"event:/SFX/UI/CARDS/GRASSHOPPER/Grasshopper_select"},
           {25,},
           // Magic Karp
           {26,},
           {27,},
           // Archer
           {28,"event:/SFX/UI/CARDS/ARCHER/Archer_attack"},
           {29,"event:/SFX/UI/CARDS/ARCHER/Archer_Select"},
           // Chicken
           {30,"event:/SFX/UI/CARDS/CHICKEN/Chicken_Attack"},
           {31,"event:/SFX/UI/CARDS/CHICKEN/Chicken_select"},
           // Leech
           {32,},
           {33,},
           // Berserker
           {34,},
           {35,},
           // Hydra
           {36,},
           {37,},
           // Zombie
           {38,},
           {39,},
           // Necromancer
           {40,},
           {41,},
           // Pocholo
           {42,},
           {43,},
           // Charybdis
           {44,},
           {45,},
           // Kamikaze
           {46,},
           {47,},
           // Dragon
           {48,},
           {49,},
           // Lacus
           {50,},
           {51,},
           // Samurai
           {52,},
           {53,},
           // Giant
           {54,},
           {55,},
           // Ant
           {56,},
           {57,},
          
    
        
        //EFFECTS
           //Flash
           {000,"event:/SFX/UI/EFFECTS/Flash/FLASH"},
           //Bowling Ball
           {001,"event:/SFX/UI/CARDS/BOWLING/Pleno"},
           {002,"event:/SFX/UI/CARDS/BOWLING/Inici_Bowling"},
           {003,"event:/SFX/UI/CARDS/BOWLING/Bowling_movement"},
           //Mana Bottle
           {004,"event:/SFX/UI/MANA/Drink_Mana"},
           //Canvi de torn
           {005,"event:/SFX/UI/CANVI DE TORN/Whisper"},
           //Return
           //C4 del medievo
           //Instant Transmission
           //Possession

        //AMBIENT
           //Roomtone
           {0000,"event:/SFX/AMBIENT/ROOMTONE"},
           //Bonfire
           {0001,"event:/SFX/AMBIENT/BONFIRE"},


          
        };     
    }
    public void PlaySFX (int index, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(sfxDictionary[index],position);
    }

    // Listado de MUSICA
    //Dictionary<int,?????????????> musicDictionary;
    
}
