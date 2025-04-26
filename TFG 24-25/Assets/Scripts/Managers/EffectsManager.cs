using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EffectsManager : Singleton<EffectsManager>
{
    PhotonView photonView;

    Vector3 posOffset;

    void Start()
    {
        posOffset = new Vector3(0,0.03f,0);

        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        
    }

    
    public void PlayFxInPosition(string effectName, Vector3 pos)
    {
        PhotonNetwork.Instantiate("Particles/" + effectName, pos, Quaternion.identity);
    }
}
