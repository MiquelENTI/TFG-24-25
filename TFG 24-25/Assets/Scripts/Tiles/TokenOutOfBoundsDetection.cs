using UnityEngine;

public class TokenOutOfBoundsDetection : MonoBehaviour
{
    // If in console shows error when moving the token inside the board or dragging it outside, check transform hierarchy, parent child etc.

    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.tag == "TileCollider" || other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetComponent<DragableGameObject>().SetOutsideBoard(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TileCollider" || other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetComponent<DragableGameObject>().SetOutsideBoard(false);
        }
    }
}
