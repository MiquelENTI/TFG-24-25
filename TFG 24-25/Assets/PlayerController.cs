using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public GameObject playerCamera;

    void Start()
    {
        if (!photonView.IsMine)
        {
            playerCamera.SetActive(false);
        }
        else
        {
            playerCamera.SetActive(true);
        }
    }
}

