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
        photonView.RPC("RPC_ChangeParentWithTag", RpcTarget.AllBuffered, tag);
    }
    [PunRPC]
    public void RPC_ChangeParentWithTag(string tag)
    {
        if (TurnManagerScript.Instance.isPCVersion)
        {
            transform.parent = GameObject.FindGameObjectWithTag(tag).transform.GetChild(0);
        }
        else
        {
            transform.parent = GameObject.FindGameObjectWithTag(tag).transform.GetChild(2).GetChild(0).GetChild(0);
        }
    }
}
