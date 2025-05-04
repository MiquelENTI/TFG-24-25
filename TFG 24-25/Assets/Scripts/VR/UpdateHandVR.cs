using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using static UnityEngine.Tilemaps.Tilemap;

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

    public void HandsPositionUpdating(GameObject myobject, Vector3 position, Quaternion rotation)
    {
        photonView.RPC("HandsPositionUpdating_RPC", RpcTarget.All, myobject, position, rotation);
    }

    [PunRPC]
    public void HandsPositionUpdating_RPC(GameObject myobject, Vector3 position, Quaternion rotation)
    {
        myobject.transform.SetPositionAndRotation(position, rotation);
    }
}
