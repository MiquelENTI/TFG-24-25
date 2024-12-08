using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchManager : MonoBehaviourPunCallbacks
{
    private string roomName = "SalaDePrueba";

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
        CreateOrJoinRoom(roomName);
    }

    public void CreateOrJoinRoom(string roomName)
    {
        Debug.Log("Intentando unirse o crear la sala: " + roomName);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
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

            if (spawnPosition == spawnPositionPlayer2)
            {
                playerPrefab.transform.GetChild(0).GetChild(0).GetChild(1).tag = "RedHand";
            }
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);
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
        Debug.Log("Jugador entró a la sala: " + newPlayer.NickName);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Ambos jugadores están en la sala. Comenzando el juego...");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Jugador salió de la sala: " + otherPlayer.NickName);
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
