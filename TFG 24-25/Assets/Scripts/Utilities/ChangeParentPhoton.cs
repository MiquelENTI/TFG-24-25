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
    

    public void ChangeParentWithTag(string tag, Vector3 PCoffset, Vector3 VRoffset)
    {
        photonView.RPC("RPC_ChangeParentWithTag", RpcTarget.AllBuffered, tag, PCoffset, VRoffset);
    }
    [PunRPC]
    public void RPC_ChangeParentWithTag(string tag, Vector3 PCoffset, Vector3 VRoffset)
    {
        if (PhotonNetwork.IsMasterClient && tag == "Master")
        {
            if (TurnManagerScript.Instance.isPCVersion)
            {
                Debug.Log("TRY 2");
                GameObject player = GameObject.FindGameObjectWithTag(tag);
                transform.position += PCoffset;
                transform.SetParent(player.transform.GetChild(0));
                Debug.Log("DONE 2");
            }
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag(tag);
                transform.position += VRoffset;
                transform.SetParent(player.transform.GetChild(2).GetChild(0).GetChild(0));
            }
        }
        else if (!PhotonNetwork.IsMasterClient && tag == "Client")
        {
            if (TurnManagerScript.Instance.isPCVersion)
            {
                Debug.Log("TRY 2");
                GameObject player = GameObject.FindGameObjectWithTag(tag);
                transform.position += PCoffset;
                transform.SetParent(player.transform.GetChild(0));
                Debug.Log("DONE 2");
            }
            else
            {
                GameObject player = GameObject.FindGameObjectWithTag(tag);
                transform.position += VRoffset;
                transform.SetParent(player.transform.GetChild(2).GetChild(0).GetChild(0));
            }
        }

        
    }
}
