using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class EffectsManager : Singleton<EffectsManager>
{
    [SerializeField] List<GameObject> effectsPrefabs = new List<GameObject>();
    Dictionary<string, GameObject> fxDictionary = new Dictionary<string, GameObject>();

    PhotonView photonView;

    Vector3 posOffset;

    void Start()
    {
        posOffset = new Vector3(0,0.03f,0);

        photonView = GetComponent<PhotonView>();

        foreach (GameObject prefab in effectsPrefabs)
        {
            fxDictionary.Add(prefab.name, prefab);
        }
    }

    void Update()
    {
        
    }

    
    public void PlayFxInPosition(string effectName, Vector3 pos)
    {
        photonView.RPC("PlayFxInPosition_RPC", RpcTarget.AllBuffered, effectName, pos + posOffset);
    }

    [PunRPC]
    public void PlayFxInPosition_RPC(string effectName, Vector3 pos)
    {
        PhotonNetwork.Instantiate("Particles/" + effectName, pos, Quaternion.identity);

        // Add Sound
    }
}
