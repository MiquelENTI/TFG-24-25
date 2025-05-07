using UnityEngine;
using UnityEngine.VFX;

public class Grasshopper : Character
{
    public Grasshopper(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        jumpMove = true;
    }

    public override bool OnSpawn(CellNode cellToMove)
    {
        return base.OnSpawn(cellToMove);
    }

    protected override void OnDeath(Character attacker)
    {
        base.OnDeath(attacker);
    }
}
