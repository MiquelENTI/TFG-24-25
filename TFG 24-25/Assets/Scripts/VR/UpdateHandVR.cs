using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using static UnityEngine.Tilemaps.Tilemap;

public class UpdateHandVR : MonoBehaviour, IPunObservable
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

    public void HandsPositionUpdating(Vector3 position, Quaternion rotation)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("HandsPositionUpdating_RPC", RpcTarget.All, position, rotation);
        }
    }

    [PunRPC]
    public void HandsPositionUpdating_RPC(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
