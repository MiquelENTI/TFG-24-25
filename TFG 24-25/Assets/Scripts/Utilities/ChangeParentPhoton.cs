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
        //PhotonView.FindObjectOfType<ChangeParentPhoton>().gameObject.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
        Debug.Log(PhotonView.FindObjectOfType<ChangeParentPhoton>().gameObject.transform.name);
    }

    public void ChangeParentWithTag(string tag)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_ChangeParent", RpcTarget.AllBuffered, tag);
        }
    }
}
