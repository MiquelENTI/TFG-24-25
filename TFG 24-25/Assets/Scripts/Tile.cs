using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileId;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TileCollider")
        {
            // Debug.Log("ENTER: " + tileId);
            other.transform.parent.GetComponent<SelectToken>().SetOnTileId(tileId);
        }
    }
}
