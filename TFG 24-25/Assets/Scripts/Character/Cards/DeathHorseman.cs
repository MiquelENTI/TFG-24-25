using UnityEngine;

public class DeathHorseman : Character
{
    public DeathHorseman(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    //Implementació del so 
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003016002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003016002, token.transform.position);
    }
}
