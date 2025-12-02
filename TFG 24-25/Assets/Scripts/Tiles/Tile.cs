using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int tileId;
    public void EnterRaycast() // Appear / Disappear highlightParticle
    {
        
    }

    public void ExitRaycast()
    {
        
    }

    public void SetTileId(int id)
    { tileId = id; }
    public int GetTileId()
    { return tileId; }
}
