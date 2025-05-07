using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class SpawnCardController : Singleton<SpawnCardController>
{
    [SerializeField] GameObject tokenPrefabBlue;
    [SerializeField] GameObject tokenPrefabRed;

    public PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void InstantiateCharacterTokenRPC(int characterIdToAssign, int initiatingPlayerActorNumber, int tileId, bool bypassSpawn)
    {
        //Debug.Log($"[SpawnCardController - START] InstantiateCharacterTokenRPC Received - ... InitiatingPlayerActorNr: {initiatingPlayerActorNumber}, LocalPlayer ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber}, LocalPlayer.IsMasterClient: {PhotonNetwork.LocalPlayer.IsMasterClient}");

        TeamType tokenTeam = PhotonNetwork.LocalPlayer.IsMasterClient ? TeamType.BLUE : TeamType.RED;

        Vector3 spawnPosition = new Vector3(2, 0, 0);

        GameObject tokenPrefabToUse = tokenTeam == TeamType.BLUE ? tokenPrefabBlue : tokenPrefabRed;
        string prefabPath = tokenTeam == TeamType.BLUE ? tokenPrefabBlue.name : tokenPrefabRed.name;

        //Debug.Log($"[SpawnCardController - BEFORE CONDITION] Prefab Path: {prefabPath}, Spawn Position: {spawnPosition}, Token Team: {tokenTeam}, InitiatingPlayerActorNr: {initiatingPlayerActorNumber}, LocalPlayer ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber}, Condition: (LocalPlayer.ActorNumber == initiatingPlayerActorNumber) = {(PhotonNetwork.LocalPlayer.ActorNumber == initiatingPlayerActorNumber)}"); // **NUEVO LOG - ANTES DEL IF**


        if (PhotonNetwork.LocalPlayer.ActorNumber == initiatingPlayerActorNumber || SceneManager.GetActiveScene().name == "ReplayScene")
        {
            GameObject instantiatedToken = PhotonNetwork.Instantiate(prefabPath, spawnPosition, Quaternion.identity);
            PhotonView tokenPhotonView = instantiatedToken.transform.GetChild(0).GetComponent<PhotonView>();
            if (tokenPhotonView != null)
            {
                //Debug.Log($"[SpawnCardController - AFTER INSTANTIATE] Token instanciado, PhotonView ID: {tokenPhotonView.ViewID}, Owner ActorNr: {tokenPhotonView.OwnerActorNr}, IsMine: {tokenPhotonView.IsMine}, Instantiated for Team: {tokenTeam}");
            }
            else
            {
                Debug.LogError("¡El prefab del token instanciado no tiene un PhotonView!");
            }

            TokenGameObject tokenGameObjectScript = instantiatedToken.transform.GetChild(0).GetComponent<TokenGameObject>();
            if (tokenGameObjectScript != null)
            {
                instantiatedToken.transform.GetChild(0).GetComponent<PhotonView>().RPC("SetCharacterRPC", RpcTarget.AllBuffered, characterIdToAssign, (int)tokenTeam);

                MyEventHandler.Instance.spawnToken.Invoke(tileId, instantiatedToken.transform.GetChild(0).GetComponent<TokenGameObject>().GetCharacter().GetId(), bypassSpawn);
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

        //Debug.Log($"[SpawnCardController - AFTER CONDITION] Controlador procesó InstantiateCharacterTokenRPC para Personaje: {characterIdToAssign}, Equipo: {tokenTeam}, InitiatingPlayerActorNr: {initiatingPlayerActorNumber}, LocalPlayer ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber}"); // **NUEVO LOG - DESPUÉS DEL IF**

    }

    public void InstantiateCharacterTokenReplay(int characterIdToAssign, int tileId, bool bypassSpawn, bool blueTurn)
    {
        //Debug.Log($"[SpawnCardController - START] InstantiateCharacterTokenRPC Received - ... InitiatingPlayerActorNr: {initiatingPlayerActorNumber}, LocalPlayer ActorNr: {PhotonNetwork.LocalPlayer.ActorNumber}, LocalPlayer.IsMasterClient: {PhotonNetwork.LocalPlayer.IsMasterClient}");

        TeamType tokenTeam = blueTurn ? TeamType.BLUE : TeamType.RED;

        Vector3 spawnPosition = new Vector3(2, 0, 0);

        string prefabPath = tokenTeam == TeamType.BLUE ? tokenPrefabBlue.name : tokenPrefabRed.name;

        GameObject instantiatedToken = Instantiate((GameObject)Resources.Load(prefabPath), spawnPosition, Quaternion.identity);

        

        TokenGameObject tokenGameObjectScript = instantiatedToken.transform.GetChild(0).GetComponent<TokenGameObject>();
        if (tokenGameObjectScript != null)
        {
            instantiatedToken.transform.GetChild(0).GetComponent<TokenGameObject>().SetCharacterRPC(characterIdToAssign, (int)tokenTeam);

            MyEventHandler.Instance.spawnToken.Invoke(tileId, instantiatedToken.transform.GetChild(0).GetComponent<TokenGameObject>().GetCharacter().GetId(), bypassSpawn);
        }
        else
        {
            Debug.LogError("Token prefab no tiene SelectToken script!");
        }
    }
}