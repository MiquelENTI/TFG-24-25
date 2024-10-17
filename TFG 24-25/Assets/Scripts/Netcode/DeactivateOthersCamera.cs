using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;


// Hi ha un error amb les càmeres, per mes info consultar Miki
// Ficar als prefabs del personatges


public class DeactivateOthersCamera : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
