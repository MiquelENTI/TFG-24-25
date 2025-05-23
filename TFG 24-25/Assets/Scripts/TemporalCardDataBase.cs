using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalCardDataBase : Singleton<TemporalCardDataBase>
{
    private Dictionary<int, CardStats> characterStats = new() {
        // ManaCost, Attack, Hp, Movement amount, attack range, score multiplier, Movement type, Ability Type, Card Text
        { 1, new CharacterStats (CardType.CHARACTER, "Wizard",             3, 2,  2,  1, 8, 1, MovementType.Basic, AbilityType.onAttack,"This has infinite Attack Range.") }, // Cavalier
        { 2, new CharacterStats (CardType.CHARACTER,"Cavalier",            3, 5,  5,  1, 1, 1, MovementType.Basic, AbilityType.nothing, "This can move and attack in the same turn.") }, // Cavalier
        { 3, new CharacterStats (CardType.CHARACTER,"Salmon",              4, 8,  7,  1, 1, 1, MovementType.Omni, AbilityType.onKill, "This moves to the killed monster location, -1 HP.")  }, // Salmon
        { 4, new EffectStats    (CardType.EFFECT,   "Flash",               3,                                     "asdasdasd") },
        { 5, new CharacterStats (CardType.CHARACTER,"Fly",                 1, 1,  1,  2, 1, 2, MovementType.Omni, AbilityType.nothing,"Pzzzzzzzzzzzzzzzt")  }, // Fly
        { 6, new CharacterStats (CardType.CHARACTER,"Turtle",              2, 5,  9,  1, 1, 1, MovementType.Basic, AbilityType.attacked, "This receives -3 damage from attacks.") }, // Turtle
        { 7, new CharacterStats (CardType.CHARACTER,"Mimic",               2, 3,  2,  1, 1, 2, MovementType.Basic, AbilityType.attacked, "Destroy both the attacking card and this. Then the opponent draws a card.") }, // Mimic
        { 8, new CharacterStats (CardType.CHARACTER,"Pichi",               5, 9,  7,  1, 1, 1, MovementType.Basic, AbilityType.onAttack, "Monsters attacked by this can't attack next turn.") }, // Pichí
        { 10, new CharacterStats(CardType.CHARACTER,"Medusa",              3, 3,  6,  1, 1, 1, MovementType.Basic, AbilityType.onAttack, "Monsters attacked by this are petrified. They can't move or attack. When this dies remove the curse from all cards.") }, // Medusa
        { 13, new CharacterStats(CardType.CHARACTER,"Mummy",               3, 5,  12, 1, 1, 1, MovementType.Basic, AbilityType.onMove, "-1 HP and -1 Attack.") }, // Mummy
        { 14, new CharacterStats(CardType.CHARACTER,"Cannon",              3, 7,  4,  1, 1, 1, MovementType.Omni, AbilityType.onAttack, "This moves to the attack's opposite direction .") }, // Cannon
        { 15, new CharacterStats(CardType.CHARACTER,"Human Werewolf",      3, 4,  4,  1, 1, 1, MovementType.Basic, AbilityType.onMove, "Flip the card.") }, // Human/Werewolf
        { 16, new CharacterStats(CardType.CHARACTER,"Death Horseman",      3, 12, 2,  1, 1, 1, MovementType.Basic, AbilityType.nothing,"¿Cuando pagan?") }, // Death Horseman
        { 17, new CharacterStats(CardType.CHARACTER,"Terracotta Warrior",  4, 5,  11, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "This heals as much HP as points scores.") }, // Terracotta Warrior
        { 18, new CharacterStats(CardType.CHARACTER,"Ghost",               2, 3,  5,  1, 1, 1, MovementType.Basic, AbilityType.nothing, "This can move from one side of the board to the other.") }, // Ghosts
        { 19, new CharacterStats(CardType.CHARACTER,"GrassHopper",         2, 5,  3,  2, 2, 2, MovementType.Omni, AbilityType.nothing, "This can only move or attack to omnidirectional x2.") }, // Grasshoppers
        { 20, new CharacterStats(CardType.CHARACTER,"Magic Karp",          5, 6,  8,  1, 1, 1, MovementType.Basic, AbilityType.startTurn, "Destroy cards on orthogonal range.") }, // Magic Karp
        { 21, new CharacterStats(CardType.CHARACTER,"Archer",              3, 3,  6,  1, 2, 1, MovementType.Omni, AbilityType.nothing, "This can attack at two tiles range") }, // Archer
        { 22, new CharacterStats(CardType.CHARACTER,"Chicken",             1, 2,  3,  1, 1, 2, MovementType.Basic, AbilityType.attacked, "This deals 5 damage to the attacker.") }, // Chicken
        { 25, new CharacterStats(CardType.CHARACTER,"Leech",               3, 3,  7,  1, 1, 1, MovementType.Basic, AbilityType.startTurn, "If this is on a scoring tile, deal 1 damage to all surrounding enemy cards.") }, // Leech
        { 26, new CharacterStats(CardType.CHARACTER,"Berserker",           3, 6,  5,  1, 1, 1, MovementType.Basic, AbilityType.scoringTile, "+2 attack.") }, // Berserker
        { 27, new CharacterStats(CardType.CHARACTER,"Hydra",               4, 7,  8,  1, 1, 1, MovementType.Basic, AbilityType.attacked, "+1 attack.") }, // Hydra
        { 28, new CharacterStats(CardType.CHARACTER,"Zombie",              1, 2,  1,  1, 1, 1, MovementType.Omni, AbilityType.onDeath, "This returns to your hand.") }, // Zombie
        { 29, new CharacterStats(CardType.CHARACTER,"Necromancer",         5, 6,  11, 1, 1, 1, MovementType.Basic, AbilityType.onKill, "Transform the killed card into a zombie of your control. ") }, // Necromancer
        { 30, new CharacterStats(CardType.CHARACTER,"Pocholo",             2, 1,  4,  2, 1, 2, MovementType.Omni, AbilityType.nothing, "If an enemy card moves close to this, this moves to the opposite direction.") }, // Pocholo
        { 31, new CharacterStats(CardType.CHARACTER,"Charybdis",           3, 5,  7,  1, 1, 1, MovementType.Omni, AbilityType.onDeath, "You summon this on a monster and this swallows it. This leaves that card on the tile this died.") }, // Charybdis
        { 32, new CharacterStats(CardType.CHARACTER,"Kamikaze",            2, 9,  3,  1, 1, 2, MovementType.Basic, AbilityType.onKill, "Destroy this.") }, // Kamikaze
        { 35, new CharacterStats(CardType.CHARACTER,"Dragon",              3, 5,  7,  1, 1, 1, MovementType.Basic, AbilityType.nothing, "This also attacks diagonally to the objective tile.") }, // Dragon
        { 36, new CharacterStats(CardType.CHARACTER,"Lacus",               4, 6,  7,  1, 1, 1, MovementType.Basic, AbilityType.startTurn, "This moves or attacks every turn to the direction is facing. Spend mana to change direction. If this hits a wall it dies.") }, // Lacus
        { 37, new CharacterStats(CardType.CHARACTER,"Samurai",             3, 3,  4,  1, 1, 1, MovementType.Basic, AbilityType.nothing, "This gains as much attack as points scores.") }, // Samurai
        { 38, new CharacterStats(CardType.CHARACTER,"Giant",               5, 4,  11, 1, 1, 1, MovementType.Basic, AbilityType.onMove, "Deal 2 damage to all surrounding cards.") }, // Giant
        { 39, new CharacterStats(CardType.CHARACTER,"Ant",                 2, 1,  4,  1, 1, 2, MovementType.Omni, AbilityType.nothing, "This has +1 attack for each card around it.") }, // Ant
        { 40, new CharacterStats(CardType.CHARACTER,"Wyvern",              2, 10, 9,  1, 1, 1, MovementType.Omni, AbilityType.onAttack, "This can't move or attack next turn.") }, // Wyvern
        
        

        { 50, new CharacterStats(CardType.CHARACTER,"Radev",4, 11, 17, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "Se queda") }, // Radev
        { 51, new CharacterStats(CardType.CHARACTER,"Richard",5, 15, 20, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "El padrino") }, // Richard
        { 52, new CharacterStats(CardType.CHARACTER,"Joan",3, 8, 15, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "S'esta passant el Halo") }, // Joan
        { 53, new CharacterStats(CardType.CHARACTER,"Hugo",3, 6, 9, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "") }, // Hugo
        { 54, new CharacterStats(CardType.CHARACTER,"Hector",3, 7, 6, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "") }, // Hector
        { 55, new CharacterStats(CardType.CHARACTER,"Coronado",2, 5, 4, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "") }, // Coronado
        { 56, new CharacterStats(CardType.CHARACTER,"Jaumandreu",2, 4, 10, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "") }, // Jaumandreu
        { 57, new CharacterStats(CardType.CHARACTER,"Mokiel",1, 3, 5, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "Juga al Call of Duty (de Roblox)") }, // Mokiel
        { 58, new CharacterStats(CardType.CHARACTER,"Pinsa",1, 3, 7, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "El programador que no programa") }, // Pinsa
        { 59, new CharacterStats(CardType.CHARACTER,"Sergi",2, 6, 7, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "Ballar� professional de swing") }, // Sergi
        { 60, new CharacterStats(CardType.CHARACTER,"Ferryklk",3, 10, 10, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "Jugador professional del Habbo") }, // Ferryklk
        { 61, new CharacterStats(CardType.CHARACTER,"Carla",4, 9, 21, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "T� un caball") }, // Carla
        { 62, new CharacterStats(CardType.CHARACTER,"Kiku",5, 12, 24, 1, 1, 1, MovementType.Basic, AbilityType.nothing, "El otro artist") }, // Kiku

        { -1, new CharacterStats(CardType.CHARACTER,"DummyCharacter",0, 0, 99999, 0, 0, 0, MovementType.Basic, AbilityType.nothing, "") }, // DummyBase
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
