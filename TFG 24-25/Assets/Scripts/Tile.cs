using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileId;

    private void OnTriggerEnter(Collider other)
    {
        // Token has an emptyObject with collision and its tag is TileCollider
        if (other.gameObject.tag == "TileCollider")
        {
            other.transform.parent.GetComponent<SelectToken>().SetOnTileId(tileId);
        }
        // Cards on Hand have an emptyObject with collision and its tag is SpawnTileCollider
        else if (other.gameObject.tag == "SpawnTileCollider")
        {
            //if (!other.GetComponent<DragableGameObject>().isInside)
            {
                Debug.Log("ASDASD");
                other.transform.parent.GetChild(0).rotation = Quaternion.Euler(90, 0, 0);
                other.transform.parent.GetChild(0).GetComponent<DragableGameObject>().SetOnTileId(tileId);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
