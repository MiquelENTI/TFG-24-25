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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Token")
        {
            other.GetComponent<SelectToken>().SetOutsideBoard(false);
        }
    }
}
