using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdatePositionPhoton : MonoBehaviourPun
{
    public void Update()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("UpdatePosition_RPC", RpcTarget.AllBuffered, transform.position, transform.rotation);
        }
    }

    public void UpdatePosition_RPC(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }
}
