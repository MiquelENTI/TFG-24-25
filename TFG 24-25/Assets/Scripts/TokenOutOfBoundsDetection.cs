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
            Debug.Log("OUTSIDE BOARD");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Token")
        {
            other.GetComponent<SelectToken>().SetOutsideBoard(false);
            Debug.Log("INSIDE BOARD");
        }
    }


}
