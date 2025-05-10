using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // Listado de efectos de sonidos de CARTAS
    Dictionary<int, string> sfxDictionary;
    // Listado de Musicas
    Dictionary<int, string> musicDictionary;
    //LListat de Ambient
    Dictionary<int, string> ambientDictionary;
    //LListat de VO
    Dictionary<int, string> voDictionary;
    //LListat de uibotton
    Dictionary<int, string> uibottonDictionary;
    GameObject board;

    void Awake()
    {
        /*Código de 9 cifras: CCC (categoría) + NNN (carta/nivel) + SSS (número de sonido)
        Los primeros 3 dígitos representan la categoría del sonido (por ejemplo, ambiente, música, SFX de cartas, etc.),
        los siguientes 3 dígitos identifican el número de la carta o el nivel dentro de la categoría,
        y los últimos 3 dígitos corresponden al número de sonido dentro de esa carta o nivel. 
        */

        musicDictionary = new()
        {

            // MUSICA
            // Taverna (Gameplay)
            { 002000001, "event:/MUSIC/GAMEPLAY/Taverna" },
            // Menú
            { 002000002, "event:/MUSIC/MENU/Menu" },

            // AMBIENT
            // Roomtone
            { 001001001, "event:/SFX/AMBIENTES/ROOMTONE" },
            // Bonfire
            { 001000002, "event:/SFX/AMBIENTES/TORCH/BONFIRE" },
            // Canvi de torn 
            { 001000003, "event:/SFX/AMBIENTES/CANVI DE TORN/RING" },


        };

        sfxDictionary = new()
        {
            //CARDS
            //Selección General para cartas sin sonido propio
            { 003000000, "event:/SFX/UI/CARDS/ARCHER/Archer_attack" },
            { 003000001, "event:/SFX/UI/CARDS/TURTLE/Turtle_select" },
            { 003000002, "event:/SFX/UI/MoveToken" },

            // Wizard
            { 003001001, "event:/SFX/CARTAS/MONSTERS/WIZZART/Wizzart_attack" },
            { 003001002, "event:/SFX/CARTAS/MONSTERS/WIZZART/Wizzart_Spown" },

            // Cavalier
            { 003002001, "event:/SFX/CARTAS/MONSTERS/CAVALIER/CAVALIER_ATTACK" },
            { 003002002, "event:/SFX/CARTAS/MONSTERS/CAVALIER/CAVALIER_SELECT" },

            // Salmon
            { 003003001, "event:/SFX/CARTAS/MONSTERS/SALMON/Salmon_selection" },
            { 003003002, "event:/SFX/CARTAS/MONSTERS/SALMON/Salmon_selection" },

            // Flash (Falta Implementar perque no se on estan els efectes)
            { 003004001, "event:/SFX/CARTAS/EFFECTS/Flash/FLASH" },


            // Fly 
            { 003005001, "event:/SFX/CARTAS/MONSTERS/FLY/Fly_Attack" },
            { 003005002, "event:/SFX/CARTAS/MONSTERS/FLY/Fly_Selection" },


            // Turtle
            { 003006001, "event:/SFX/CARTAS/MONSTERS/TURTLE/Turtle_Attack" },
            { 003006002, "event:/SFX/CARTAS/MONSTERS/TURTLE/Turtle_select" },


            // Mimic
            { 003007001, "event:/SFX/CARTAS/MONSTERS/MIMIC/Mimic_Attack" },
            { 003007002, "event:/SFX/CARTAS/MONSTERS/MIMIC/Mimic_Selecction" },


            // Pichi
            { 003008001, "event:/SFX/CARTAS/MONSTERS/PICHI/Pichi_Attack" },
            { 003008002, "event:/SFX/CARTAS/MONSTERS/PICHI/Pichi_Selection" },


            // Bowling Ball
            { 003009001, "event:/SFX/CARTAS/MONSTERS/BOWLING/Pleno" },
            { 003009002, "event:/SFX/CARTAS/MONSTERS/BOWLING/Inici_Bowling" },
            { 003009003, "event:/SFX/CARTAS/MONSTERS/BOWLING/Bowling_movement" },

            // Medusa
            { 003010001, "event:/SFX/CARTAS/MONSTERS/MEDUSA/Medusa_Attack" },
            { 003010002, "event:/SFX/CARTAS/MONSTERS/MEDUSA/Medusa_selection" },

            // 
            { 003011001, "" },
            { 003011002, "" },


            // Possession
            { 003012001, "" },
            { 003012002, "" },
            { 003012003, "" },

            // Mummy
            { 003013001, "event:/SFX/CARTAS/MONSTERS/MUMMY/Mummy_Attack" },
            { 003013002, "event:/SFX/CARTAS/MONSTERS/MUMMY/Mummy_Selecction" },


            // The Gun
            { 003014001, "event:/SFX/CARTAS/MONSTERS/THE GUN/TheGun_Attack" },
            { 003014002, "event:/SFX/CARTAS/MONSTERS/THE GUN/TheGun_selection" },


            // Human/Werewolf 
            { 003015001, "event:/SFX/CARTAS/MONSTERS/WAREWOLF/Howl" },
            { 003015002, "event:/SFX/CARTAS/MONSTERS/WAREWOLF/Howl" },


            // Death Horseman (Falta attack)
            { 003016001, "event:/SFX/CARTAS/MONSTERS/DEATHHORSEMAN/DeathHorseman_select" },
            { 003016002, "event:/SFX/CARTAS/MONSTERS/DEATHHORSEMAN/DeathHorseman_select" },

            // Terracotta Warrior 
            { 003017001, "event:/SFX/CARTAS/MONSTERS/TARRACOTA_WARRIOR/Tarracota_SELECT" },
            { 003017002, "event:/SFX/CARTAS/CARDS/TARRACOTA_WARRIOR/Tarracota_SELECT" },
            { 003017003, "" },

            // Fantasma
            { 003018001, "event:/SFX/CARTAS/MONSTERS/Fantasma/Fantasma_attack" },
            { 003018002, "event:/SFX/CARTAS/MONSTERS/Fantasma/Fantasma_Select" },
            { 003018003, "" },

            // Grasshopper
            { 003019001, "event:/SFX/CARTAS/MONSTERS/GRASSHOPPER/Grasshopper_Attack" },
            { 003019002, "event:/SFX/CARTAS/MONSTERS/GRASSHOPPER/Grasshopper_select" },
            { 003019003, "" },

            // Magic Karp
            { 003020001, "event:/SFX/CARTAS/MONSTERS/SALMON/Salmon_selection" },
            { 003020002, "event:/SFX/CARTAS/MONSTERS/SALMON/Salmon_selection" },
            { 003020003, "" },

            // Archer
            { 003021001, "event:/SFX/CARTAS/MONSTERS/ARCHER/Archer_attack" },
            { 003021002, "event:/SFX/CARTAS/MONSTERS/ARCHER/Archer_Select" },
            { 003021003, "" },

            // Chicken
            { 003022001, "event:/SFX/CARTAS/MONSTERS/CHICKEN/Chicken_Attack" },
            { 003022002, "event:/SFX/CARTAS/MONSTERS/CHICKEN/Chicken_select" },
            { 003022003, "" },

            // Mana Bottle (pendent de fer o carta o saber on ficar-lo)
            { 003023001, "event:/SFX/CARTAS/MANA/Drink_Mana" },
            { 003023002, "" },
            { 003023003, "" },

            // Return (effectes potser NO en espera)
            { 003024001, "" },
            { 003024002, "" },
            { 003024003, "" },

            // Leech
            { 003025001, "event:/SFX/CARTAS/MONSTERS/LEECH/LEECH" },
            { 003025002, "" },
            { 003025003, "" },

            // Berserker
            { 003026001, "event:/SFX/CARTAS/MONSTERS/BERSERKER/BERSERKER_ATACK" },
            { 003026002, "event:/SFX/CARTAS/MONSTERS/BERSERKER/BERSERKER_SELECT" },
            { 003026003, "" },

            // Hydra
            { 003027001, "event:/SFX/CARTAS/MONSTERS/HYDRA/HYDRA_ATTACK" },
            { 003027002, "event:/SFX/CARTAS/MONSTERS/HYDRA/HYDRA_ATTACK" },
            { 003027003, "" },

            // Zombie (SONS MUMMY)
            { 003028001, "event:/SFX/CARTAS/MONSTERS/MUMMY/Mummy_Attack" },
            { 003028002, "event:/SFX/CARTAS/MONSTERS/MUMMY/Mummy_Selecction" },
            { 003028003, "" },

            // Necromancer
            { 003029001, "event:/SFX/CARTAS/MONSTERS/Necromander/Necromander" },
            { 003029002, "event:/SFX/CARTAS/MONSTERS/Necromander/Necromander" },
            { 003029003, "" },

            // Pocholo (SONS PICHI)
            { 003030001, "event:/SFX/CARTAS/MONSTERS/PICHI/Pichi_Attack" },
            { 003030002, "event:/SFX/CARTAS/MONSTERS/PICHI/Pichi_Selection" },
            { 003030003, "" },

            // Charybdis
            { 003031001, "event:/SFX/CARTAS/MONSTERS/CHarybdis/Charybdis" },
            { 003031002, "event:/SFX/CARTAS/MONSTERS/CHarybdis/Charybdis" },
            { 003031003, "" },

            // Kamikaze
            { 003032001, "event:/SFX/CARTAS/MONSTERS/Kamikaze/Kamikaze_Attack" },
            { 003032002, "event:/SFX/CARTAS/MONSTERS/Kamikaze/Kamikaze_Select" },
            { 003032003, "" },

            // C4 del medievo
            { 003033001, "" },
            { 003033002, "" },
            { 003033003, "" },

            // Dragon
            { 003034001, "event:/SFX/CARTAS/MONSTERS/DRAGON/DRAGON" },
            { 003034002, "event:/SFX/CARTAS/MONSTERS/DRAGON/DRAGON" },
            { 003034003, "" },

            // Lacus
            { 003035001, "event:/SFX/CARTAS/MONSTERS/GIANT/GIANT_ATTACK" },
            { 003035002, "event:/SFX/CARTAS/MONSTERS/Lacus/Lacus" },
            { 003035003, "" },

            // Samurai (Pendent de no fer - possiblement no) 
            { 003036001, "" },
            { 003036002, "" },
            { 003036003, "" },

            // Giant
            { 003037001, "event:/SFX/CARTAS/MONSTERS/GIANT/GIANT_ATTACK" },
            { 003037002, "event:/SFX/CARTAS/MONSTERS/GIANT/GIANT_SELECT" },
            { 003037003, "" },

            // Ant
            { 003038001, "event:/SFX/CARTAS/MONSTERS/ANT/Ant_Select_Attack" },
            { 003038002, "event:/SFX/CARTAS/MONSTERS/ANT/Ant_Select_Attack" },
            { 003038003, "" },

            // Wyvern
            { 003039001, "event:/SFX/CARTAS/MONSTERS/Wyvern/Wyvern_Attack" },
            { 003039002, "event:/SFX/CARTAS/MONSTERS/Wyvern/Wyvern_Select" },
            { 003039003, "" },
        };

        // VO: event és únic, però el diccionari serveix com a identificador lògic
        voDictionary = new()
        {
            { 004000000, "CAMBIO_TURNO" },
            { 004000001, "MULTI_KILL" },
            { 004000002, "JUGADOR_INACTIVO" },
            { 004000003, "GENERAL" },
            { 004000004, "VICTORIA" },
            { 004000005, "DERROTA" },
        };
        uibottonDictionary = new()
        {
            //Botons Ui Credits
            { 005000001, "event:/SFX/UI/Button_Credits" },
            //Botons Ui Exit
            { 005000002, "event:/SFX/UI/Button_Exit" },
            //Botons Ui General
            { 005000003, "event:/SFX/UI/Buttons_General" },
        };

        board = GameObject.Find("Board");
        PlayMusic(002000001);
        PlayMusic(001001001);
    }


    public void PlaySFX(int index, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(sfxDictionary[index], position);
    }

    public void PlayMusic(int index)
    {
        FMODUnity.RuntimeManager.PlayOneShot(musicDictionary[index], board.transform.position);
    }
    public void PlayVO(int index, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(voDictionary[index], position); // si fos VO per event separat
    }

    public void PlayBonfireAtPosition(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(musicDictionary[001000002], position);
    }
    public void PlayMoscaMovement(GameObject mosca)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(sfxDictionary[003005002], mosca);
    }
    public void PlayGrasshopperMovement(GameObject grasshopper)
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(sfxDictionary[003019002], grasshopper);
    }

    // VO amb paràmetre VO_INDICATIONS
    public void PlayVOByID(int voID, Vector3 position)
    {
        if (!voDictionary.ContainsKey(voID))
        {
            Debug.LogWarning("VO ID no trobat: " + voID);
            return;
        }

        float parameterValue = voID - 004000000; // offset respecte del primer VO
        PlayVOIndication(parameterValue, position);
    }

    public void PlayVOIndication(float parameterValue, Vector3 position)
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance("event:/VOICE OFF/VO_PersonajeOmnipotente");
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(position));
        instance.setParameterByName("VO_INDICATIONS", parameterValue);
        instance.start();
        instance.release();
    }
}
