using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // Listado de efectos de sonidos de CARTAS
    Dictionary<int, string> sfxDictionary;
    // Listado de Musicas
    Dictionary<int, string> musicDictionary;
    GameObject board;

    void Awake ()
    {
    /* Código de 9 cifras: CCC (categoría) + NNN (carta/nivel) + SSS (número de sonido)
    Los primeros 3 dígitos representan la categoría del sonido (por ejemplo, ambiente, música, SFX de cartas, etc.),
    los siguientes 3 dígitos identifican el número de la carta o el nivel dentro de la categoría,
    y los últimos 3 dígitos corresponden al número de sonido dentro de esa carta o nivel. 
    */

        sfxDictionary=new()
        {
        //CARDS
           //Selección General para cartas sin sonido propio
           {003000000,"event:/SFX/UI/CARDS/ARCHER/Archer_attack"},
           {003000001,"event:/SFX/UI/CARDS/TURTLE/Turtle_select"},
           {003000002,"event:/TOKEN MOVEMENT"},

           // Wizard
           {003001001, "event:/SFX/UI/CARDS/WIZZART/Wizzart_attack"},
           {003001002, "event:/SFX/UI/CARDS/WIZZART/Wizzart_Spown"},
           {003001003, ""},

           // Cavalier
           {003002001, ""},
           {003002002, ""},
           {003002003, ""},

           // Salmon
           {003003001, "event:/SFX/UI/CARDS/SALMON/Salmon_selection"},
           {003003002, "event:/SFX/UI/CARDS/SALMON/Salmon_selection"},
           {003003003, ""},

           // Flash
           {003004001, "event:/SFX/UI/EFFECTS/Flash/FLASH"},
           {003004002, ""},
           {003004003, ""},

           // Fly
           {003005001, "event:/SFX/UI/CARDS/FLY/Fly_Attack"},
           {003005002, "event:/SFX/UI/CARDS/FLY/Fly_Selection"},
           {003005003, ""},

           // Turtle
           {003006001, "event:/SFX/UI/CARDS/TURTLE/Turtle_select"},
           {003006002, "event:/SFX/UI/CARDS/TURTLE/Turtle_select"},
           {003006003, ""},

           // Mimic
           {003007001, "event:/SFX/UI/CARDS/MIMIC/Mimic_Attack"},
           {003007002, "event:/SFX/UI/CARDS/MIMIC/Mimic_Selecction"},
           {003007003, ""},

           // Pichi
           {003008001, "event:/SFX/UI/CARDS/PICHI/Pichi_Attack"},
           {003008002, "event:/SFX/UI/CARDS/PICHI/Pichi_Selection"},
           {003008003, ""},

           // Bowling Ball
           {003009001, "event:/SFX/UI/CARDS/BOWLING/Pleno"},
           {003009002, "event:/SFX/UI/CARDS/BOWLING/Inici_Bowling"},
           {003009003, "event:/SFX/UI/CARDS/BOWLING/Bowling_movement"},

           // Medusa
           {003010001, "event:/SFX/UI/CARDS/MEDUSA/Medusa_Attack"},
           {003010002, "event:/SFX/UI/CARDS/MEDUSA/Medusa_selection"},
           {003010003, ""},

           // Instant Transmission
           {003011001, ""},
           {003011002, ""},
           {003011003, ""},

           // Possession
           {003012001, ""},
           {003012002, ""},
           {003012003, ""},

           // Mummy
           {003013001, "event:/SFX/UI/CARDS/MUMMY/Mummy_Attack"},
           {003013002, "event:/SFX/UI/CARDS/MUMMY/Mummy_Selecction"},
           {003013003, ""},

           // The Gun
           {003014001, "event:/SFX/UI/CARDS/THE GUN/TheGun_Attack"},
           {003014002, "event:/SFX/UI/CARDS/THE GUN/TheGun_selection"},
           {003014003, ""},

           // Human/Werewolf
           {003015001, ""},
           {003015002, "event:/SFX/UI/CARDS/WAREWOLF/Howl"},
           {003015003, ""},

           // Death Horseman
           {003016001, ""},
           {003016002, "event:/SFX/UI/CARDS/DEATHHORSEMAN/DeathHorseman_select"},
           {003016003, ""},

           // Terracotta Warrior
           {003017001, ""},
           {003017002, "event:/SFX/UI/CARDS/TARRACOTA_WARRIOR/Tarracota_select"},
           {003017003, ""},

           // Fantasma
           {003018001, ""},
           {003018002, ""},
           {003018003, ""},

           // Grasshopper
           {003019001, ""},
           {003019002, "event:/SFX/UI/CARDS/GRASSHOPPER/Grasshopper_select"},
           {003019003, ""},

           // Magic Karp
           {003020001, ""},
           {003020002, ""},
           {003020003, ""},

           // Archer
           {003021001, "event:/SFX/UI/CARDS/ARCHER/Archer_attack"},
           {003021002, "event:/SFX/UI/CARDS/ARCHER/Archer_Select"},
           {003021003, ""},
           
           // Chicken
           {003022001, "event:/SFX/UI/CARDS/CHICKEN/Chicken_Attack"},
           {003022002, "event:/SFX/UI/CARDS/CHICKEN/Chicken_select"},
           {003022003, ""},
           
           // Mana Bottle
           {003023001, "event:/SFX/UI/MANA/Drink_Mana"},
           {003023002, ""},
           {003023003, ""},
           
           // Return
           {003024001, ""},
           {003024002, ""},
           {003024003, ""},
           
           // Leech
           {003025001, ""},
           {003025002, ""},
           {003025003, ""},
           
           // Berserker
           {003026001, ""},
           {003026002, ""},
           {003026003, ""},
           
           // Hydra
           {003027001, ""},
           {003027002, ""},
           {003027003, ""},
           
           // Zombie
           {003028001, ""},
           {003028002, ""},
           {003028003, ""},
           
           // Necromancer
           {003029001, ""},
           {003029002, ""},
           {003029003, ""},
           
           // Pocholo
           {003030001, ""},
           {003030002, ""},
           {003030003, ""},
           
           // Charybdis
           {003031001, ""},
           {003031002, ""},
           {003031003, ""},
           
           // Kamikaze
           {003032001, ""},
           {003032002, ""},
           {003032003, ""},

           // C4 del medievo
           {003033001, ""},
           {003033002, ""},
           {003033003, ""},
           
           // Dragon
           {003034001, ""},
           {003034002, ""},
           {003034003, ""},
           
           // Lacus
           {003035001, ""},
           {003035002, ""},
           {003035003, ""},
           
           // Samurai
           {003036001, ""},
           {003036002, ""},
           {003036003, ""},
           
           // Giant
           {003037001, ""},
           {003037002, ""},
           {003037003, ""},
           
           // Ant
           {003038001, ""},
           {003038002, ""},
           {003038003, ""},
           
           // Wyvern
           {003039001, ""},
           {003039002, ""},
           {003039003, ""},
           
           // Cambiaformes
           {003040001, ""},
           {003040002, ""},
           {003040003, ""}
          
        };     

         musicDictionary=new()
        {
        // AMBIENT
            // Roomtone
            {001001001, "event:/SFX/AMBIENT/ROOMTONE"},
            // Bonfire
            {001000002, "event:/SFX/AMBIENT/BONFIRE"},
            // Canvi de torn 
            {001000003, "event:/SFX/UI/CANVI DE TORN/Whisper"},
        // MUSICA
            // Taverna (Gameplay)
            {002000001, "event:/MUSIC/GAMEPLAY/Gameplay"},
            // Menú
            {002000002, "event:/MUSIC/MENU/Menu"},
        
        };   
        board = GameObject.Find("Board");
        //PlayMusic(002000001);
    }


   public void PlaySFX (int index, Vector3 position)
    {
        //FMODUnity.RuntimeManager.PlayOneShot(sfxDictionary[index],position);
    }

   public void PlayMusic (int index)
    {
        //FMODUnity.RuntimeManager.PlayOneShot(musicDictionary[index],board.transform.position);
    }
     public void PlayBonfireAtPosition (Vector3 position)
    {
        //FMODUnity.RuntimeManager.PlayOneShot(musicDictionary[001000002],position);
    }
}