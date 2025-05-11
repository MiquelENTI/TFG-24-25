using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class ChangeParentPhoton : MonoBehaviourPun
{
    private void Awake()
    {
    }
    

    public void ChangeParentWithTag(string tag)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_ChangeParentWithTag", RpcTarget.AllBuffered, tag);
        }
    }
    [PunRPC]
    public void RPC_ChangeParentWithTag(string tag)
    {
        transform.parent = GameObject.FindGameObjectWithTag(tag).transform;
    }
}
