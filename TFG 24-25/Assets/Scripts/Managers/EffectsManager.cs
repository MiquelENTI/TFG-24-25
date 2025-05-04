using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum TileHighlightState { Movement, Attack, Occupied }

public class EffectsManager : Singleton<EffectsManager>
{
    Vector3 posOffset;

    List<GameObject> blueHighlightList = new List<GameObject>();
    List<GameObject> redHighlightList = new List<GameObject>();

    void Start()
    {
        posOffset = new Vector3(0, 0.03f, 0);
    }

    public void PlayFxInPosition(string effectName, Vector3 pos)
    {
        PhotonNetwork.Instantiate("Particles/" + effectName, pos + posOffset, Quaternion.identity);
    }

    public void SetHighlightParticles(TileHighlightState state, Vector3 pos)
    {
        GameObject particle = PhotonNetwork.Instantiate("Particles/" + state.ToString(), pos, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            blueHighlightList.Add(particle);
        }
        else
        {
            redHighlightList.Add(particle);
        }
    }

    public void RemoveTeamEffects()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            blueHighlightList.Clear();
        }
        else
        {
            redHighlightList.Clear();
        }
    }
}
