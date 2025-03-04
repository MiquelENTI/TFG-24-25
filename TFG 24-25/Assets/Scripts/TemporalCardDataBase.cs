using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    Dictionary<int, CharacterStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, Movement type, Card Text
        { 1, new CharacterStats ("Cavalier", 0, 5, 8,  1, MovementType.Basic, "CAVALIER\n\nPuede moverse y atacar en el mismo turno") }, // Cavalier
        { 3, new CharacterStats ("Salmon", 4, 9, 13, 1, MovementType.Omni, "SALMON\n\nAl matar a un enemigo, se mueve a la casilla del enemigo y esta pierde 2 hp")  }, // Salmon
        { 5, new CharacterStats ("Fly",1, 1, 1,  1, MovementType.Omni, "FLY")  }, // Fly
        { 6, new CharacterStats ("Turtle",2, 5, 18, 1, MovementType.Basic, "TURTLE\n\nSe mueve cada dos turnos (se mueve, descansa, se mueve...)") }, // Turtle (WIP)
        { 7, new CharacterStats ("Mimic",2, 3, 6,  1, MovementType.Basic, "MIMIC\n\nCuando le atacan, se destruyen esta carta y el atacante") }, // Mimic  (WIP)
        { 10, new CharacterStats("Medusa",3, 3, 12, 1, MovementType.Basic, "MEDUSA\n\nAl atacar stunea, al morir esta carta quita stuns de los enemigos atacados") }, // Medusa
        { 13, new CharacterStats("Mummy",3, 6, 23, 1, MovementType.Basic, "MUMMY\n\nAl moverse pierde 1 hp y 1 dmg") }, // Mummy
        { 14, new CharacterStats("The Gun",3, 8, 9, 1, MovementType.Omni, "THE GUN\n\nAl atacar se mueve en direccion contraria al ataque") }, // The Gun
        { 15, new CharacterStats("Human Werewolf",3, 4, 8, 1, MovementType.Basic, "HUMAN/WEREWOLF\n\nAl moverse se cambian sus estadisticas") }, // The Gun
        { 16, new CharacterStats("Death Horseman",3, 14, 4, 1, MovementType.Basic, "DEATH HORSEMAN") }, // Death Horseman
        { 17, new CharacterStats("Terracotta Warrior",4, 4, 21, 1, MovementType.Basic, "TERRACOTTA WARRIOR\n\nAl permanecer immobil durante un turno, regenera 3 hp") }, // Terracotta Warrior
        { 20, new CharacterStats("Magic Karp",5, 7, 15, 1, MovementType.Basic, "MAGIC KARP\n\nDestruye las cartas a rango basico (aliadas tambien)") }, // Magic Karp
        { 21, new CharacterStats("Archer",1, 2, 7,  1, MovementType.Basic, "ARCHER\n\nTe 2 de rang d'atac") }, // Archer
        { 22, new CharacterStats("Chicken",1, 2, 7,  1, MovementType.Basic, "CHICKEN\n\nAl recibir daño el atacante recibe 16 de daño") }, // Chicken
        { 25, new CharacterStats("Leech",3, 3, 7,  1, MovementType.Basic, "LEECH\n\nINICI DEL TORN: si esta en una casella de puntuació, enemics a una casella reben 1 de mal\n\n") }, // Leech
        { 26, new CharacterStats("Berserker",3, 6, 5,  1, MovementType.Basic, "BERSERKER\n\nSi esta en una casella de puntuació, fa +2 d'atac\n\n") }, // Berserker
        { 27, new CharacterStats("Hydra",4, 7, 9,  1, MovementType.Basic, "HYDRA\n\nSi esta en una casella de puntuació, es cura la quantitat de punts obtinguts\n\n") }, // Hydra
        
        

        { 50, new CharacterStats("Radev",4, 11, 17,  1, MovementType.Basic, "RADEV\n\nCiro Di Marzio") }, // Radev
        { 51, new CharacterStats("Richard",5, 15, 20,  1, MovementType.Basic, "RICHARD\n\nEl padrino") }, // Richard
        { 52, new CharacterStats("Joan",3, 8, 15,  1, MovementType.Basic, "JOAN\n\nS'esta passant el Halo") }, // Joan
        { 53, new CharacterStats("Hugo",3, 6, 9,  1, MovementType.Basic, "HUGO\n\n") }, // Hugo
        { 54, new CharacterStats("Hector",3, 7, 6,  1, MovementType.Basic, "HECTOR\n\n") }, // Hector
        { 55, new CharacterStats("Coronado",2, 5, 4,  1, MovementType.Basic, "CORONADO\n\n") }, // Coronado
        { 56, new CharacterStats("Jaumandreu",2, 4, 10,  1, MovementType.Basic, "JAUMANDREU\n\n") }, // Jaumandreu
        { 57, new CharacterStats("Mokiel",1, 3, 5,  1, MovementType.Basic, "MOKIEL\n\nJuga al Call of Duty (de Roblox)") }, // Mokiel
        { 58, new CharacterStats("Pinsa",1, 3, 7,  1, MovementType.Basic, "PINSA\n\nEl programador que no programa") }, // Pinsa
        { 59, new CharacterStats("Sergi",2, 6, 7,  1, MovementType.Basic, "SERGI\n\nBallarí professional de bachata") }, // Sergi
        { 60, new CharacterStats("Ferryklk",3, 10, 10,  1, MovementType.Basic, "FERRYKLK\n\nJugador professional del Habbo") }, // Ferryklk
        { 61, new CharacterStats("Carla",4, 9, 21,  1, MovementType.Basic, "CARLA\n\nTé un caball") }, // Carla
        { 62, new CharacterStats("Kiku",5, 12, 24,  1, MovementType.Basic, "KIKU\n\nEl otro artist") }, // Kiku

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

    public List<int> GetAllCharacters(int copies)
    {
        List<int> temp = new();
        for (int i = 0; i < copies; i++)
        {
            foreach (var keyPair in characterStats)
            {
                temp.Add(keyPair.Key);
            }
        }
        return temp;
    }

    public CharacterCard GetCharacter(int characterId)
    {
        return new CharacterCard(characterStats[characterId]);
    }
}
