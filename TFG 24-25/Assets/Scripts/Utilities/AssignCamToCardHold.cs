using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AssignCamToCardHold : MonoBehaviour
{
    void Start()
    {
        GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold").GetComponent<CardHold>().AssignPlayerCam(gameObject);
    }
}
