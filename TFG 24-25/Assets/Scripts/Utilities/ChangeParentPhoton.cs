using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class ChangeParentPhoton : MonoBehaviour
{
    PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void RPC_ChangeParentWithTag(string tag)
    {
        transform.parent = GameObject.FindGameObjectWithTag(tag).transform;
    }

    public void ChangeParentWithTag(string tag)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_ChangeParent", RpcTarget.AllBuffered, tag);
        }
    }
}
