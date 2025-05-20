using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    private PhotonView photonView;

    public int blueScore = 0;
    public int redScore = 0;

    private Vector3 blueCoinSpawnPos = new(-0.67f, 1.0f, -0.85f);
    private Vector3 redCoinSpawnPos = new(1.4f, 1.0f, 0.2f);

    [PunRPC]
    public void UpdateScore(TeamType teamType, int amount)
    {
        if (teamType == TeamType.BLUE && PhotonNetwork.IsMasterClient)
        {
            blueScore += amount;

            for (int i = 0; i < amount; i++)
            {
                PhotonNetwork.Instantiate("Moneda1", blueCoinSpawnPos, Quaternion.identity);
            }
        }
        else
        {
            redScore += amount;

            for (int i = 0; i < amount; i++)
            {
                PhotonNetwork.Instantiate("Moneda1", redCoinSpawnPos, Quaternion.identity);
            }
        }
    }
}
