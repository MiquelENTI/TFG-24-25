using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TokenOutOfBoundsDetection : MonoBehaviour
{
    // If in console shows error when moving the token inside the board or dragging it outside, check transform hierarchy, parent child etc.

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.tag == "TokenGameObject" || other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetChild(0).GetComponent<DragableGameObject>().SetOutsideBoard(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TokenGameObject" || other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetChild(0).GetComponent<DragableGameObject>().SetOutsideBoard(false);
        }
    }
}
