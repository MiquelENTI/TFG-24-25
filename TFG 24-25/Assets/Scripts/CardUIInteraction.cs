using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardUIInteraction : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        
    }

}
