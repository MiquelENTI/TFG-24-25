using UnityEngine;

public class HumanWerewolf : Character
{
    private CharacterStats humanForm;
    private CharacterStats wereWolfForm;
    private bool isHumanForm = true;
    public HumanWerewolf(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        humanForm = newStats;
        wereWolfForm = newStats;
        wereWolfForm.dmg = 8;
        wereWolfForm.hp = 8;
        wereWolfForm.movementType = MovementType.Omni;
    }

    public override bool OnMovement(TileNode cellToMove)
    {
        if (!base.OnMovement(cellToMove))
        { return false; }

        ChangeForms();

        return true;
    }

    //Implementació del so
    protected override void OnSpawnSFX()
    {
        SoundManager.Instance.PlaySFX(003015002, token.transform.position);
    }

    private void ChangeForms()
    {
        isHumanForm = !isHumanForm;
        if (isHumanForm)
        {
            stats.hp = (int)(stats.hp / 2.0f);
            stats.hp = Mathf.RoundToInt(stats.hp);

            stats.movementType = humanForm.movementType;
            ChangeMovementType(stats.movementType);
            stats.dmg = humanForm.dmg;

            DisplayActionsManager.Instance.CreateCustomText("=" + stats.dmg.ToString(), new Color(220.0f / 255.0f, 20.0f / 255.0f, 60.0f / 255.0f), 8, 1.0f, token.transform.position);
            DisplayActionsManager.Instance.CreateCustomText("=" + stats.hp.ToString(), new Color(50.0f / 255.0f, 245.0f / 255.0f, 99.0f / 255.0f), 8, 1.0f, token.transform.position, -0.2f);
        }
        else
        {
            stats.hp *= 2;

            stats.movementType = wereWolfForm.movementType;
            stats.dmg = wereWolfForm.dmg;

            ChangeMovementType(stats.movementType);

            DisplayActionsManager.Instance.CreateCustomText("=" + stats.dmg.ToString(), new Color(220.0f / 255.0f, 20.0f / 255.0f, 60.0f / 255.0f), 8, 1.0f, token.transform.position);
            DisplayActionsManager.Instance.CreateCustomText("=" + stats.hp.ToString(), new Color(50.0f / 255.0f, 245.0f / 255.0f, 99.0f / 255.0f), 8, 1.0f, token.transform.position, -0.2f);
        }

    }
}
