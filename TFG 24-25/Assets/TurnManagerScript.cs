using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManagerScript : MonoBehaviour
{
    [SerializeField] bool IsBlue = true;

    public void TurnManager()
    {
        IsBlue = !IsBlue;
    }
}
