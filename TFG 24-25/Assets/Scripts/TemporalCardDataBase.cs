using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    Dictionary<int, CharacterStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, Movement type, Card Text
        { 1, new CharacterStats ("Cavalier", 0, 5, 8,  1000, MovementType.Basic, "CAVALIER\n\nPuede moverse y atacar en el mismo turno") }, // Cavalier
        { 3, new CharacterStats ("Salmon", 4, 9, 13, 1000, MovementType.Omni, "SALMON\n\nAl matar a un enemigo, se mueve a la casilla del enemigo y esta pierde 2 hp")  }, // Salmon
        { 5, new CharacterStats ("Fly",1, 1, 1,  1000, MovementType.Omni, "FLY")  }, // Fly
        { 6, new CharacterStats ("Turtle",2, 5, 18, 1000, MovementType.Basic, "TURTLE\n\nSe mueve cada dos turnos (se mueve, descansa, se mueve...)") }, // Turtle (WIP)
        { 7, new CharacterStats ("Mimic",2, 3, 6,  1000, MovementType.Basic, "MIMIC\n\nCuando le atacan, se destruyen esta carta y el atacante") }, // Mimic  (WIP)
        { 10, new CharacterStats("Medusa",3, 3, 12, 1000, MovementType.Basic, "MEDUSA\n\nAl atacar stunea, al morir esta carta quita stuns de los enemigos atacados") }, // Medusa
        { 13, new CharacterStats("Mummy",3, 6, 23, 1000, MovementType.Basic, "MUMMY\n\nAl moverse pierde 1 hp y 1 dmg") }, // Mummy
        { 14, new CharacterStats("The Gun",3, 8, 9, 1000, MovementType.Omni, "THE GUN\n\nAl atacar se mueve en direccion contraria al ataque") }, // The Gun
        { 15, new CharacterStats("Human Werewolf",3, 4, 8, 1000, MovementType.Basic, "HUMAN/WEREWOLF\n\nAl moverse se cambian sus estadisticas") }, // The Gun
        { 16, new CharacterStats("Death Horseman",3, 14, 4, 1000, MovementType.Basic, "DEATH HORSEMAN") }, // Death Horseman
        { 17, new CharacterStats("Terracotta Warrior",4, 4, 21, 1000, MovementType.Basic, "TERRACOTTA WARRIOR\n\nAl permanecer immobil durante un turno, regenera 3 hp") }, // Terracotta Warrior
        { 20, new CharacterStats("Magic Karp",5, 7, 15, 1000, MovementType.Basic, "MAGIC KARP\n\nDestruye las cartas a rango basico (aliadas tambien)") }, // Magic Karp
        { 22, new CharacterStats("Chicken",1, 2, 7,  1000, MovementType.Basic, "CHICKEN\n\nAl recibir daño el atacante recibe 16 de daño") }, // Chicken
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

    public List<CharacterCard> GetAllCharacters(int copies)
    {
        List<CharacterCard> temp = new();
        for (int i = 0; i < copies; i++)
        {
            foreach (CharacterStats stats in characterStats.Values)
            {
                temp.Add(new CharacterCard(stats));
            }
        }
        return temp;
    }

    public CharacterCard GetCharacter(int characterId)
    {
        return new CharacterCard(characterStats[characterId]);
    }
}
