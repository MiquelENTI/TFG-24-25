using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TokenOutOfBoundsDetection : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Token")
        {
            other.GetComponent<SelectToken>().SetOutsideBoard(true);
        }
        else if (other.gameObject.tag == "SpawnTileCollider")
        {
            Debug.Log("OUTSIDE");
            other.transform.parent.GetChild(0).GetComponent<DragableGameObject>().SetOutsideBoard(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Token")
        {
            other.GetComponent<SelectToken>().SetOutsideBoard(false);
        }
        else if (other.gameObject.tag == "SpawnTileCollider")
        {
            Debug.Log("INSIDE");
            other.transform.parent.GetChild(0).GetComponent<DragableGameObject>().SetOutsideBoard(false);
        }
    }
}
