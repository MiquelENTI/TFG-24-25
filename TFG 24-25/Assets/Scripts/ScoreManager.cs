using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreManager : MonoBehaviourPun
{
    public int BlueScore;
    public int RedScore;
    public TMP_Text redText;
    public TMP_Text blueText;


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
            Debug.Log("Puntaje azul incrementado. Nuevo puntaje: " + BlueScore);
            blueText.text = "Blue Score: " + BlueScore;
        }
        else
        {
            RedScore += amount;
            Debug.Log("Puntaje rojo incrementado. Nuevo puntaje: " + RedScore);
            redText.text = "Red Score: " + RedScore;
        }
    }
}
