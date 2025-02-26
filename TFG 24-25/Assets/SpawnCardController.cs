using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnCardController : MonoBehaviourPun
{
    [SerializeField] GameObject tokenPrefabBlue;
    [SerializeField] GameObject tokenPrefabRed;
    [SerializeField] Material blueMaterial;
    [SerializeField] Material redMaterial;

    Cards card;

    void Start() { }

    [PunRPC]
    public void InstantiateCharacterTokenRPC(int characterIdToAssign, int initiatingPlayerActorNumber, int tileId)
    {
        Debug.Log($"[SpawnCardController - START] InstantiateCharacterTokenRPC Received - ... InitiatingPlayerActorNr: {initiatingPlayerActorNumber}, LocalPlayer ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber}, LocalPlayer.IsMasterClient: {PhotonNetwork.LocalPlayer.IsMasterClient}");

        TeamType tokenTeam = PhotonNetwork.LocalPlayer.IsMasterClient ? TeamType.BLUE : TeamType.RED;

        Vector3 spawnPosition = new Vector3(0, -1000, 0);

        GameObject tokenPrefabToUse = tokenTeam == TeamType.BLUE ? tokenPrefabBlue : tokenPrefabRed;
        string prefabPath = tokenTeam == TeamType.BLUE ? tokenPrefabBlue.name : tokenPrefabRed.name;

        Debug.Log($"[SpawnCardController - BEFORE CONDITION] Prefab Path: {prefabPath}, Spawn Position: {spawnPosition}, Token Team: {tokenTeam}, InitiatingPlayerActorNr: {initiatingPlayerActorNumber}, LocalPlayer ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber}, Condition: (LocalPlayer.ActorNumber == initiatingPlayerActorNumber) = {(PhotonNetwork.LocalPlayer.ActorNumber == initiatingPlayerActorNumber)}"); // **NUEVO LOG - ANTES DEL IF**


        if (PhotonNetwork.LocalPlayer.ActorNumber == initiatingPlayerActorNumber)
        {
            GameObject instantiatedToken = PhotonNetwork.Instantiate(prefabPath, spawnPosition, Quaternion.identity);
            PhotonView tokenPhotonView = instantiatedToken.transform.GetChild(0).GetComponent<PhotonView>();
            if (tokenPhotonView != null)
            {
                Debug.Log($"[SpawnCardController - AFTER INSTANTIATE] Token instanciado, PhotonView ID: {tokenPhotonView.ViewID}, Owner ActorNr: {tokenPhotonView.OwnerActorNr}, IsMine: {tokenPhotonView.IsMine}, Instantiated for Team: {tokenTeam}");
            }
            else
            {
                Debug.LogError("¡El prefab del token instanciado no tiene un PhotonView!");
            }

            TokenGameObject tokenGameObjectScript = instantiatedToken.transform.GetChild(0).GetComponent<TokenGameObject>();
            if (tokenGameObjectScript != null)
            {
                instantiatedToken.transform.GetChild(0).GetComponent<PhotonView>().RPC("SetCharacterRPC", RpcTarget.AllBuffered, characterIdToAssign, (int)tokenTeam);

                MyEventHandler.Instance.RPC_MoveToken(tileId, instantiatedToken.transform.GetChild(0).GetComponent<TokenGameObject>().GetCharacter().GetId());
            }
            else
            {
                Debug.LogError("Token prefab no tiene SelectToken script!");
            }
        }
        else
        {
            Debug.Log($"[SpawnCardController - ELSE BRANCH] Cliente no iniciador (ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber}), NO instanciando token. InitiatingPlayerActorNr: {initiatingPlayerActorNumber}"); // **LOG EXISTENTE - EN EL ELSE**
        }

        Debug.Log($"[SpawnCardController - AFTER CONDITION] Controlador procesó InstantiateCharacterTokenRPC para Personaje: {characterIdToAssign}, Equipo: {tokenTeam}, InitiatingPlayerActorNr: {initiatingPlayerActorNumber}, LocalPlayer ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber}"); // **NUEVO LOG - DESPUÉS DEL IF**

    }

    Character CharacterClassSelector(int id, TeamType tokenTeam, GameObject instantiatedToken)
    {
        CharacterStats stats = TemporalCardDataBase.Instance.GetTemporalStats(id);
        switch (id)
        {
            case 1:
                return new Cavalier(stats, tokenTeam, instantiatedToken, null);
            case 3:
                return new Salmon(stats, tokenTeam, instantiatedToken, null);
            case 5:
                return new Fly(stats, tokenTeam, instantiatedToken, null);
            case 6:
                return new Turtle(stats, tokenTeam, instantiatedToken, null);
            case 7:
                return new Mimic(stats, tokenTeam, instantiatedToken, null);
            case 10:
                return new Medusa(stats, tokenTeam, instantiatedToken, null);
            case 13:
                return new Mummy(stats, tokenTeam, instantiatedToken, null);
            case 16:
                return new DeathHorseman(stats, tokenTeam, instantiatedToken, null);
            case 17:
                return new TerracottaWarrior(stats, tokenTeam, instantiatedToken, null);
            case 20:
                return new MagicKarp(stats, tokenTeam, instantiatedToken, null);
            case 22:
                return new Chicken(stats, tokenTeam, instantiatedToken, null);
            case -1:
                return new Dummy(stats, tokenTeam);
            case 14:
                return new TheGun(stats, tokenTeam, instantiatedToken, null);
            case 15:
                return new HumanWerewolf(stats, tokenTeam, instantiatedToken, null);
            default:
                Debug.LogError("NO CHARACTER RECOGNISED");
                return new Character(stats, tokenTeam, instantiatedToken, null);
        }

    }
}