using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int tileId;

    private void Start()
    {

    }

    public void EnterRaycast()
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
