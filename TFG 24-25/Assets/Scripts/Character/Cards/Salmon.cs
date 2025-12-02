using UnityEngine;

public class Salmon : Character
{
    public Salmon(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
    }

    protected override void OnKillEnemy(Character enemy)
    {
        Debug.Log("ENTERING EATING ENEMY");
        base.OnKillEnemy(enemy);
        if (enemy.GetCharacterStats().name != "Charybdis")
        {
            Debug.Log("EATING ENEMY");
            BypassMovement(TileNodeManager.Instance.GetNodeById(enemy.GetOnTileId()));
            ReceiveDamageSelf(1);
        }
    }
    //Implementació del so 
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003003002, token.transform.position);
    }
    protected override void AttackSFX()
    {
        SoundManager.Instance.PlaySFX(003003001, token.transform.position);
    }
}
 
