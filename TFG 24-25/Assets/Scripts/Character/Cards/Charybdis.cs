using Photon.Pun;
using UnityEngine;

public class Charybdis : Character
{
    private SpawnCardController spawnManager;
    private int characterIdToSpawn;

    public Charybdis(CharacterStats newStats, TeamType teamType, GameObject token) : base(newStats, teamType, token)
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnCardController>();
    }

    public override bool OnSpawn(CellNode cellToMove)
    {
        if (cellToMove.GetCharacter() == null) { return false; }

        if (cellToMove.GetCharacter().GetTeamType() == teamType)
        {
            characterIdToSpawn = TemporalCardDataBase.Instance.GetTemporalStatsIdByName(cellToMove.GetCharacter().GetCharacterStats().name);

            cellToMove.GetCharacter().DestroyCharacter();

            base.OnSpawn(cellToMove);

        }

        return true;
    }

    protected override void OnDeath(Character attacker)
    {
        spawnManager.photonView.RPC("InstantiateCharacterTokenRPC", RpcTarget.AllBuffered, characterIdToSpawn, PhotonNetwork.LocalPlayer.ActorNumber, onTile, true);

        base.OnDeath(attacker);
    }
}
