using DilmerGames.Core.Singletons;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersManager : Singleton<PlayersManager>
{
    private NetworkVariable<int> playersConnected = new NetworkVariable<int>();

    public int PlayersConnected
    {
        get { return playersConnected.Value; }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Logger.Instance.LogInfo($"#{id} connected");
            playersConnected.Value++;
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            Logger.Instance.LogInfo($"#{id} disconnected");
            playersConnected.Value--;
        };
    }
}
