using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastEndTurn : MonoBehaviour
{
    public void Interact()
    {
        TurnManagerScript.Instance.TurnManager();
    }
}
