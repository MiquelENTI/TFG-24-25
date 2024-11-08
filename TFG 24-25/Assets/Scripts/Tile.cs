using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileId;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Token")
        {
            Debug.Log("Entered " + tileId);
            other.GetComponent<SelectToken>().SetOnTileId(tileId);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Token")
    //    {
    //        Debug.Log("Exit " + tileId);
    //        other.GetComponent<SelectToken>().ResetOnTileId(tileId);
    //    }
    //}
}
