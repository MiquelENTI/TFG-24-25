using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    Dictionary<int, CharacterStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, Movement type, Card Text
        { 2, new CharacterStats ("Cavalier", 3, 5, 5, 1, 1, 1, MovementType.Basic, "Puede moverse y atacar en el mismo turno") }, // Cavalier
        { 3, new CharacterStats ("Salmon", 4, 8, 7, 1, 1, 1, MovementType.Omni, "Al matar a un enemigo, se mueve a la casilla del enemigo y esta pierde 2 hp")  }, // Salmon
        { 5, new CharacterStats ("Fly",1, 1, 1,  2, 1, 2, MovementType.Omni, "")  }, // Fly
        { 6, new CharacterStats ("Turtle",2, 5, 9, 1, 1, 1, MovementType.Basic, "Se mueve cada dos turnos (se mueve, descansa, se mueve...)") }, // Turtle (WIP)
        { 7, new CharacterStats ("Mimic",2, 3, 2, 1, 1, 2, MovementType.Basic, "Cuando le atacan, se destruyen esta carta y el atacante") }, // Mimic  (WIP)
        { 10, new CharacterStats("Medusa",3, 3, 6, 1, 1, 1, MovementType.Basic, "Al atacar stunea, al morir esta carta quita stuns de los enemigos atacados") }, // Medusa
        { 13, new CharacterStats("Mummy",3, 5, 12, 1, 1, 1, MovementType.Basic, "Al moverse pierde 1 hp y 1 dmg") }, // Mummy
        { 14, new CharacterStats("The Gun",3, 7, 4, 1, 1, 1, MovementType.Omni, "Al atacar se mueve en direccion contraria al ataque") }, // The Gun
        { 15, new CharacterStats("Human Werewolf",3, 4, 4, 1, 1, 1, MovementType.Basic, "Al moverse se intercambian sus estadisticas") }, // The Gun
        { 16, new CharacterStats("Death Horseman",3, 12, 2, 1, 1, 1, MovementType.Basic, "") }, // Death Horseman
        { 17, new CharacterStats("Terracotta Warrior",4, 5, 11, 1, 1, 1, MovementType.Basic, "Al permanecer en una casilla de puntuacion, se cura los puntos conseguidos") }, // Terracotta Warrior
        { 20, new CharacterStats("Magic Karp",5, 6, 8, 1, 1, 1, MovementType.Basic, "Destruye las cartas a rango basico (aliadas tambien)") }, // Magic Karp
        { 21, new CharacterStats("Archer",3, 3, 6,  1, 1, 1, MovementType.Omni, "Te 2 de rang d'atac") }, // Archer
        { 22, new CharacterStats("Chicken",1, 2, 3, 1, 1, 2, MovementType.Basic, "Al recibir da�o el atacante recibe 5 de da�o") }, // Chicken
        { 25, new CharacterStats("Leech",3, 3, 7, 1, 1, 1, MovementType.Basic, "INICI DEL TORN: si esta en una casella de puntuaci�, enemics a una casella reben 1 de mal\n\n") }, // Leech
        { 26, new CharacterStats("Berserker",3, 6, 5, 1, 1, 1, MovementType.Basic, "Si esta en una casella de puntuaci�, fa +2 d'atac\n\n") }, // Berserker
        { 27, new CharacterStats("Hydra",4, 7, 8, 1, 1, 1, MovementType.Basic, "Si esta en una casella de puntuaci�, es cura la quantitat de punts obtinguts\n\n") }, // Hydra
        
        
        { 32, new CharacterStats("Kamikaze",2, 9, 3, 1, 1, 2, MovementType.Basic, "Quan mata a un enemic destrueix aquesta carta\n\n") }, // Hydra

        { 35, new CharacterStats("Dragon",3, 3, 7, 1, 1, 1, MovementType.Basic, "Ataca tambe a les diagonals adjacents de la direccio d'atac\n\n") }, // Hydra
    
        { 37, new CharacterStats("Samurai",3, 5, 7, 1, 1, 1, MovementType.Basic, "Quan mor un aliat, consegueix 1 d'atac, 8 maxim\n\n") }, // Hydra
        { 38, new CharacterStats("Giant",5, 6, 11, 1, 1, 1, MovementType.Basic, "Quan es mou fa 2 de mal a tots els personatges adjacents\n\n") }, // Hydra
        { 39, new CharacterStats("Ant",2, 1, 4, 1, 1, 2, MovementType.Omni, "Per cada personatge (aliat o enemic) adjecent rep 1 d'atac\n\n") }, // Hydra
        
        

        { 50, new CharacterStats("Radev",4, 11, 17, 1, 1, 1, MovementType.Basic, "Ciro Di Marzio") }, // Radev
        { 51, new CharacterStats("Richard",5, 15, 20, 1, 1, 1, MovementType.Basic, "El padrino") }, // Richard
        { 52, new CharacterStats("Joan",3, 8, 15, 1, 1, 1, MovementType.Basic, "S'esta passant el Halo") }, // Joan
        { 53, new CharacterStats("Hugo",3, 6, 9, 1, 1, 1, MovementType.Basic, "") }, // Hugo
        { 54, new CharacterStats("Hector",3, 7, 6, 1, 1, 1, MovementType.Basic, "") }, // Hector
        { 55, new CharacterStats("Coronado",2, 5, 4, 1, 1, 1, MovementType.Basic, "") }, // Coronado
        { 56, new CharacterStats("Jaumandreu",2, 4, 10, 1, 1, 1, MovementType.Basic, "") }, // Jaumandreu
        { 57, new CharacterStats("Mokiel",1, 3, 5, 1, 1, 1, MovementType.Basic, "Juga al Call of Duty (de Roblox)") }, // Mokiel
        { 58, new CharacterStats("Pinsa",1, 3, 7, 1, 1, 1, MovementType.Basic, "El programador que no programa") }, // Pinsa
        { 59, new CharacterStats("Sergi",2, 6, 7, 1, 1, 1, MovementType.Basic, "Ballar� professional de bachata") }, // Sergi
        { 60, new CharacterStats("Ferryklk",3, 10, 10, 1, 1, 1, MovementType.Basic, "Jugador professional del Habbo") }, // Ferryklk
        { 61, new CharacterStats("Carla",4, 9, 21, 1, 1, 1, MovementType.Basic, "T� un caball") }, // Carla
        { 62, new CharacterStats("Kiku",5, 12, 24, 1, 1, 1, MovementType.Basic, "El otro artist") }, // Kiku

        { -1, new CharacterStats("Dummy",0, 0, 99999, 0, 0, 0, MovementType.Basic, "") }, // DummyBase
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
