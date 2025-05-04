using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class UpdateHandVR : MonoBehaviour
{
    PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandsPositionUpdating()
    {
        photonView.RPC("HandsPositionUpdating_RPC", RpcTarget.OthersBuffered);
    }

    [PunRPC]
    public void HandsPositionUpdating_RPC()
    {
        transform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
