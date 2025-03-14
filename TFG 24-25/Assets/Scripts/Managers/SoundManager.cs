using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // Listado de efectos de sonidos de CARTAS
    Dictionary<int, string> sfxDictionary;
    // Listado de Musicas
    Dictionary<int, string> musicDictionary;

    void Awake ()
    {
        sfxDictionary=new()
        {
        //CARDS
        {3000,""},
        {3001,""},
           //Salmon
           {000,"event:/SFX/UI/CARDS/SALMON/Salmon_selection"},
           {001,""},
           //Fly
           {002,"event:/SFX/UI/CARDS/FLY/Fly_Attack"},
           {003,"event:/SFX/UI/CARDS/FLY/Fly_Selection"},
           //Turtle
           {004,"event:/SFX/UI/CARDS/TURTLE/Turtle_select"},
           {005,""},
           // Mimic
           {006,"event:/SFX/UI/CARDS/MIMIC/Mimic_Attack"},
           {007,"event:/SFX/UI/CARDS/MIMIC/Mimic_Selecction"},
           // Pichí
           {008,"event:/SFX/UI/CARDS/PICHI/Pichi_Attack"},
           {009,"event:/SFX/UI/CARDS/PICHI/Pichi_Selection"},
           // Medusa
           {010,"event:/SFX/UI/CARDS/MEDUSA/Medusa_Attack"},
           {011,"event:/SFX/UI/CARDS/MEDUSA/Medusa_selection"},
           // Mummy
           {012,"event:/SFX/UI/CARDS/MUMMY/Mummy_Attack"},
           {013,"event:/SFX/UI/CARDS/MUMMY/Mummy_Selecction"},
           // The Gun
           {014,"event:/SFX/UI/CARDS/THE GUN/TheGun_Attack"},
           {015,"event:/SFX/UI/CARDS/THE GUN/TheGun_selection"},
           // Human/Werewolf
           {016,""},
           {017,"event:/SFX/UI/CARDS/WAREWOLF/Howl"},
           // Death Horseman
           {018,""},
           {019,"event:/SFX/UI/CARDS/DEATHHORSEMAN/DeathHorseman_select"},
           // Terracotta Warrior
           {020,""},
           {021,"event:/SFX/UI/CARDS/TARRACOTA_WARRIOR/Tarracota_select"},
           // Fantasma
           {022,""},
           {023,""},
           // Grasshopper
           {024,"event:/SFX/UI/CARDS/GRASSHOPPER/Grasshopper_select"},
          //{25,},
          //Magic Karp
           {026,""},
           {027,""},
           // Archer
           {028,"event:/SFX/UI/CARDS/ARCHER/Archer_attack"},
           {029,"event:/SFX/UI/CARDS/ARCHER/Archer_Select"},
           // Chicken
           {030,"event:/SFX/UI/CARDS/CHICKEN/Chicken_Attack"},
           {031,"event:/SFX/UI/CARDS/CHICKEN/Chicken_select"},
           // Leech
           {032,""},
           {033,""},
           // Berserker
           {034,""},
           {035,""},
           // Hydra
           {036,""},
           {037,""},
           // Zombie
           {038,""},
           {039,""},
           // Necromancer
           {040,""},
           {041,""},
           // Pocholo
           {042,""},
           {043,""},
           // Charybdis
           {044,""},
           {045,""},
           // Kamikaze
           {046,""},
           {047,""},
           // Dragon
           {048,""},
           {049,""},
           // Lacus
           {050,""},
           {051,""},
           // Samurai
           {052,""},
           {053,""},
           // Giant
           {054,""},
           {055,""},
           // Ant
           {056,""},
           {057,""},
          
    
        
        //EFFECTS
           //Flash
           {1000,"event:/SFX/UI/EFFECTS/Flash/FLASH"},
           //Bowling Ball
           {1001,"event:/SFX/UI/CARDS/BOWLING/Pleno"},
           {1002,"event:/SFX/UI/CARDS/BOWLING/Inici_Bowling"},
           {1003,"event:/SFX/UI/CARDS/BOWLING/Bowling_movement"},
           //Mana Bottle
           {1004,"event:/SFX/UI/MANA/Drink_Mana"},
           //Canvi de torn
           {1005,"event:/SFX/UI/CANVI DE TORN/Whisper"},
           //Return
           //C4 del medievo
           //Instant Transmission
           //Possession

        //AMBIENT
           //Roomtone
           {2000,"event:/SFX/AMBIENT/ROOMTONE"},
           //Bonfire
           {2001,"event:/SFX/AMBIENT/BONFIRE"},
          
        };     

         musicDictionary=new()
        {
        // Listado de MUSICA
           //Taverna
           //{3000,"event:/MUSIC/GAMEPLAY/Gameplay"},
           //Menú
           //{3001,"event:/MUSIC/MENU/Menu"},
          
        };   

        PlayMusic(3000);
    }


   public void PlaySFX (int index, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(sfxDictionary[index],position);
    }

   public void PlayMusic (int index)
    {
        //FMODUnity.RuntimeManager.PlayOneShot(musicDictionary[index],GameObject.Find("Board").transform.position);
    
    }
}