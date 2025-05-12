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
    

    public void ChangeParentWithTag(string tag, int index)
    {
        photonView.RPC("RPC_ChangeParentWithTag", RpcTarget.AllBuffered, tag, index);
    }
    [PunRPC]
    public void RPC_ChangeParentWithTag(string tag, int index)
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag(tag);
        Debug.Log(list[0].name + " " + list[1] + ", INDEX: " + index + " " + list[index]);
        transform.parent = GameObject.FindGameObjectsWithTag(tag)[index].transform;
    }
}
