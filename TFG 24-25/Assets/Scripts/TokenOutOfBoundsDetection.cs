using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TokenOutOfBoundsDetection : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Token" || other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetChild(0).GetComponent<DragableGameObject>().SetOutsideBoard(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Token" || other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetChild(0).GetComponent<DragableGameObject>().SetOutsideBoard(false);
        }
    }
}
