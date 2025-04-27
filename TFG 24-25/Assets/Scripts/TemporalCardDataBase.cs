using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    Dictionary<int, CardStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, Movement type, Card Text
        { 1, new CharacterStats (CardType.CHARACTER, "Wizard",             3, 2,  2,  1, 8, 1, MovementType.Basic, "This has infinite Attack Range.") }, // Cavalier
        { 2, new CharacterStats (CardType.CHARACTER,"Cavalier",            3, 5,  5,  1, 1, 1, MovementType.Basic, "This can move and attack in the same turn.") }, // Cavalier
        { 3, new CharacterStats (CardType.CHARACTER,"Salmon",              4, 8,  7,  1, 1, 1, MovementType.Omni, "When this kills an enemy card it moves to that location. Each kill -1 health.")  }, // Salmon
        { 4, new EffectStats    (CardType.EFFECT,   "Flash",               3,                                     "asdasdasd") },
        { 5, new CharacterStats (CardType.CHARACTER,"Fly",                 1, 1,  1,  2, 1, 2, MovementType.Omni, "Pzzzzzzzzzzzzzzzt")  }, // Fly
        { 6, new CharacterStats (CardType.CHARACTER,"Turtle",              2, 5,  9,  1, 1, 1, MovementType.Basic, "This moves every 2 turns)") }, // Turtle (WIP)
        { 7, new CharacterStats (CardType.CHARACTER,"Mimic",               2, 3,  2,  1, 1, 2, MovementType.Basic, "When this is attacked destroy both the attacking card and this. Then the opponent draws a card.") }, // Mimic  (WIP)
        { 8, new CharacterStats (CardType.CHARACTER,"Pichi",               5, 9,  7,  1, 1, 1, MovementType.Basic, "When this attacks an enemy monster, that monster can't attack next turn..") }, // Mimic  (WIP)
        { 10, new CharacterStats(CardType.CHARACTER,"Medusa",              3, 3,  6,  1, 1, 1, MovementType.Basic, "When this attacks a monster that monster is petrified. It can't move or attack. When this dies remove the curse from all cards.") }, // Medusa
        { 13, new CharacterStats(CardType.CHARACTER,"Mummy",               3, 5,  12, 1, 1, 1, MovementType.Basic, "Each time this moves -1 attack and HP") }, // Mummy
        { 14, new CharacterStats(CardType.CHARACTER,"The Gun",             3, 7,  4,  1, 1, 1, MovementType.Omni, "When this attacks, it moves to the opposite direction.") }, // The Gun
        { 15, new CharacterStats(CardType.CHARACTER,"Human Werewolf",      3, 4,  4,  1, 1, 1, MovementType.Basic, "This is summoned as Human. Every time this moves flip the card.") }, // The Gun
        { 16, new CharacterStats(CardType.CHARACTER,"Death Horseman",      3, 12, 2,  1, 1, 1, MovementType.Basic, "¿Cuando pagan?") }, // Death Horseman
        { 17, new CharacterStats(CardType.CHARACTER,"Terracotta Warrior",  4, 5,  11, 1, 1, 1, MovementType.Basic, "When this scores, it heals as much HP as points scored.") }, // Terracotta Warrior
        { 18, new CharacterStats(CardType.CHARACTER,"Ghost",               2, 3,  5,  1, 1, 1, MovementType.Basic, "This can move from one side of the board to the other.") }, // Terracotta Warrior
        { 19, new CharacterStats(CardType.CHARACTER,"GrassHopper",         2, 5,  3,  2, 2, 2, MovementType.Omni,  "This can only move or attack to omnidirectional x2.") }, // Terracotta Warrior
        { 20, new CharacterStats(CardType.CHARACTER,"Magic Karp",          5, 6,  8,  1, 1, 1, MovementType.Basic, "When your turn starts, destroy all cards on orthogonal range") }, // Magic Karp
        { 21, new CharacterStats(CardType.CHARACTER,"Archer",              3, 3,  6,  1, 2, 1, MovementType.Omni, "This can attack at two tiles range") }, // Archer
        { 22, new CharacterStats(CardType.CHARACTER,"Chicken",             1, 2,  3,  1, 1, 2, MovementType.Basic, "When this is attacked it deals 5 damage to the attacker.") }, // Chicken
        { 25, new CharacterStats(CardType.CHARACTER,"Leech",               3, 3,  7,  1, 1, 1, MovementType.Basic, "When your turn starts, if this is on a scoring tile, deal 1 damage to all surrounding enemy cards.") }, // Leech
        { 26, new CharacterStats(CardType.CHARACTER,"Berserker",           3, 6,  5,  1, 1, 1, MovementType.Basic, "If this is on a scoring tile, +2 attack.") }, // Berserker
        { 27, new CharacterStats(CardType.CHARACTER,"Hydra",               4, 7,  8,  1, 1, 1, MovementType.Basic, "When this is attacked, it gains +1 attack.") }, // Hydra
        { 28, new CharacterStats(CardType.CHARACTER,"Zombie",              1, 2,  1,  1, 1, 1, MovementType.Omni, "When this is killed, This returns to your hand.") }, // Hydra
        { 29, new CharacterStats(CardType.CHARACTER,"Necromancer",         5, 6,  11, 1, 1, 1, MovementType.Basic, "When this kills a card, transform that card into a zombie of your control.") }, // Hydra
        { 30, new CharacterStats(CardType.CHARACTER,"Pocholo",             2, 1,  4,  2, 1, 2, MovementType.Omni, "If an enemy card moves close to this, this moves to the opposite direction.") }, // Hydra
        { 31, new CharacterStats(CardType.CHARACTER,"Charybdis",           3, 5,  7,  1, 1, 1, MovementType.Omni, "You summon this on a monster and this swallows it. When this dies it leaves that card on the tile this died.") }, // Hydra
        { 32, new CharacterStats(CardType.CHARACTER,"Kamikaze",            2, 9,  3,  1, 1, 2, MovementType.Basic, "When this kills a card destroy this.") }, // Kamikaze
        { 35, new CharacterStats(CardType.CHARACTER,"Dragon",              3, 3,  7,  1, 1, 1, MovementType.Basic, "When this attacks, it also attacks diagonally to the objective tile.") }, // Dragon
        { 36, new CharacterStats(CardType.CHARACTER,"Lacus",               4, 6,  7,  1, 1, 1, MovementType.Basic, "This moves or attacks every turn to the direction is facing. Spend mana to change direction. If this hits a wall destroy this.") }, // Dragon
        { 37, new CharacterStats(CardType.CHARACTER,"Samurai",             3, 5,  7,  1, 1, 1, MovementType.Basic, "When this scores, it gains as much attack as points scored.   ") }, // Samurai
        { 38, new CharacterStats(CardType.CHARACTER,"Giant",               5, 6,  11, 1, 1, 1, MovementType.Basic, "After this moves, deal 2 damage to all surrounding cards.") }, // Giant
        { 39, new CharacterStats(CardType.CHARACTER,"Ant",                 2, 1,  4,  1, 1, 2, MovementType.Omni, "This has +1 attack for each card around it.") }, // Ant
        { 39, new CharacterStats(CardType.CHARACTER,"Wyvern",              2, 10, 9,  1, 1, 1, MovementType.Omni, "If this makes an attack this turn, it can't move or attack next turn.") }, // Ant
        
        

        { 50, new CharacterStats(CardType.CHARACTER,"Radev",4, 11, 17, 1, 1, 1, MovementType.Basic, "Se queda") }, // Radev
        { 51, new CharacterStats(CardType.CHARACTER,"Richard",5, 15, 20, 1, 1, 1, MovementType.Basic, "El padrino") }, // Richard
        { 52, new CharacterStats(CardType.CHARACTER,"Joan",3, 8, 15, 1, 1, 1, MovementType.Basic, "S'esta passant el Halo") }, // Joan
        { 53, new CharacterStats(CardType.CHARACTER,"Hugo",3, 6, 9, 1, 1, 1, MovementType.Basic, "") }, // Hugo
        { 54, new CharacterStats(CardType.CHARACTER,"Hector",3, 7, 6, 1, 1, 1, MovementType.Basic, "") }, // Hector
        { 55, new CharacterStats(CardType.CHARACTER,"Coronado",2, 5, 4, 1, 1, 1, MovementType.Basic, "") }, // Coronado
        { 56, new CharacterStats(CardType.CHARACTER,"Jaumandreu",2, 4, 10, 1, 1, 1, MovementType.Basic, "") }, // Jaumandreu
        { 57, new CharacterStats(CardType.CHARACTER,"Mokiel",1, 3, 5, 1, 1, 1, MovementType.Basic, "Juga al Call of Duty (de Roblox)") }, // Mokiel
        { 58, new CharacterStats(CardType.CHARACTER,"Pinsa",1, 3, 7, 1, 1, 1, MovementType.Basic, "El programador que no programa") }, // Pinsa
        { 59, new CharacterStats(CardType.CHARACTER,"Sergi",2, 6, 7, 1, 1, 1, MovementType.Basic, "Ballar� professional de bachata") }, // Sergi
        { 60, new CharacterStats(CardType.CHARACTER,"Ferryklk",3, 10, 10, 1, 1, 1, MovementType.Basic, "Jugador professional del Habbo") }, // Ferryklk
        { 61, new CharacterStats(CardType.CHARACTER,"Carla",4, 9, 21, 1, 1, 1, MovementType.Basic, "T� un caball") }, // Carla
        { 62, new CharacterStats(CardType.CHARACTER,"Kiku",5, 12, 24, 1, 1, 1, MovementType.Basic, "El otro artist") }, // Kiku

        { -1, new CharacterStats(CardType.CHARACTER,"DummyCharacter",0, 0, 99999, 0, 0, 0, MovementType.Basic, "") }, // DummyBase
        { -2, new EffectStats   (CardType.EFFECT,   "DummyEffect", 0, "") }, // DummyBase
    };

    public Tuple<bool,CardStats> GetTemporalStats(int id)
    {
        if (characterStats.ContainsKey(id))
        {
            return Tuple.Create(true, characterStats[id]);
        }
        else
        {
            return Tuple.Create(false, characterStats[-1]);
        }
    }

    public int GetTemporalStatsIdByName(string name)
    {
        foreach (var stats in characterStats)
        {
            if (stats.Value.name == name)
            {
                return stats.Key; 
            }
        }
        return -1;
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
}
