using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DisableOtherPlayerComponents : MonoBehaviourPun
{
    void Start()
    {
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }
}
