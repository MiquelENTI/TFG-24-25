using UnityEngine;
using UnityEngine.VFX;

public class Grasshopper : Character
{
    public Grasshopper(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        jumpMove = true;
    }
    //Implementació del so 
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003019001, token.transform.position);
    }
    
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003019001, token.transform.position);
    }
}
