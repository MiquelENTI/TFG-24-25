using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class MatchManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Vector3 spawnPositionPlayer1 = new Vector3(11.8f, -8.7f, -0.14f);
    public Vector3 spawnPositionPlayer2 = new Vector3(-8.0f, -8.7f, -4.15f);

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado a Photon.");
        JoinOrCreateRoom();
    }

    public void JoinOrCreateRoom()
    {
        Debug.Log("Intentando unirse a una sala aleatoria...");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo unir a una sala aleatoria, creando una nueva sala.");
        CreateNewRoom();
    }

    void CreateNewRoom()
    {
        string roomName = "Sala_" + Guid.NewGuid().ToString();
        Debug.Log("Creando nueva sala: " + roomName);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public void JoinOrCreateRoomByName(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("El nombre de la sala está vacío.");
            return;
        }

        Debug.Log("Intentando unirse a la sala con nombre: " + roomName);
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Jugador unido a la sala: " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Es el MasterClient. Esperando al segundo jugador...");
        }
        else
        {
            Debug.Log("Juego listo para empezar, ambos jugadores están en la sala.");
        }

        if (playerPrefab != null)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            Quaternion spawnRotation = GetSpawnRotation();

            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);

            if (playerPrefab.name == "Player") // PREVENT STRANGE VR BEHAVIOR
            {
                TurnManagerScript.Instance.isPCVersion = true;
                
            }
            else
            {
                TurnManagerScript.Instance.isPCVersion = false;
            }

            TurnManagerScript.Instance.RegisterPlayer(player);

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {

                //player.GetComponent<PhotonView>().RPC("SetPlayerColor", RpcTarget.AllBuffered, true);
                //player.GetComponent<PhotonView>().RPC("ActivatePlayerHeads", RpcTarget.AllBuffered);

            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                //player.GetComponent<PhotonView>().RPC("SetPlayerColor", RpcTarget.AllBuffered, false);
                //player.GetComponent<PhotonView>().RPC("ActivatePlayerHeads", RpcTarget.AllBuffered
            }
        }
        else
        {
            Debug.LogError("Player prefab is missing.");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error al intentar unirse o crear la sala: " + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Jugador entró a la sala: " + newPlayer.NickName + " en sala: " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Sala: " + PhotonNetwork.CurrentRoom.Name + " está llena. Comenzando el juego...");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Jugador salió de la sala: " + otherPlayer.NickName + " en sala: " + PhotonNetwork.CurrentRoom.Name);
    }

    private Vector3 GetSpawnPosition()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            return spawnPositionPlayer1;
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            return spawnPositionPlayer2;
        }
        return Vector3.zero;
    }

    private Quaternion GetSpawnRotation()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            return Quaternion.Euler(0, 180, 0);
        }

        return Quaternion.identity;
    }

}