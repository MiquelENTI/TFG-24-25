using UnityEngine;

public class Kamikaze : Character
{
    public Kamikaze(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    protected override void OnKillEnemy(Character enemy)
    {
        base.OnKillEnemy(enemy);

        ReceiveDamageSelf(9999);
    }
    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(0003032002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003032001, token.transform.position);
    }
}
