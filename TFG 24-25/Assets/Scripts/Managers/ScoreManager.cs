using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreManager : MonoBehaviourPun
{
    public int BlueScore;
    public int RedScore;

    Vector3 blueCoinSpawnPos = new(-0.67f, 1.0f, -0.85f);
    Vector3 redCoinSpawnPos = new(1.4f, 1.0f, 0.2f);

    void Start()
    {
        BlueScore = 0;
        RedScore = 0;
    }

    [PunRPC]
    public void UpdateScore(TeamType teamType, int amount)
    {
        if (teamType == TeamType.BLUE)
        {
            BlueScore += amount;

            PhotonNetwork.Instantiate("Moneda1", blueCoinSpawnPos, Quaternion.identity);
        }
        else
        {
            RedScore += amount;

            PhotonNetwork.Instantiate("Moneda1", redCoinSpawnPos, Quaternion.identity);
        }
    }
}
