using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum TileHighlightState { Movement, Attack, Occupied }

public class VFXManager : Singleton<VFXManager>
{
    private Vector3 posOffset = new Vector3(0, 0.03f, 0);

    private List<GameObject> blueHighlightList = new List<GameObject>();
    private List<GameObject> redHighlightList = new List<GameObject>();


    public void PlayVFXInPosition(string effectName, Vector3 pos)
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

    public void RemoveTeamHighlights()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (GameObject particle in blueHighlightList)
            {
                PhotonNetwork.Destroy(particle);
                Destroy(particle);
            }
            blueHighlightList.Clear();
        }
        else
        {
            foreach (GameObject particle in redHighlightList)
            {
                PhotonNetwork.Destroy(particle);
                Destroy(particle);
            }
            redHighlightList.Clear();
        }
    }
}
