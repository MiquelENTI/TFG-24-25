using UnityEngine;

public class Mimic : Character
{
    public Mimic(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    protected override void OnAttacked(Character attacker)
    {
        // Enemy draws a cards 
        attacker.ReceiveDamageSelf(9999);
        ReceiveDamageSelf(9999);
    }

    //Implementació del so 
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003007002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003007001, token.transform.position);
    }
}
