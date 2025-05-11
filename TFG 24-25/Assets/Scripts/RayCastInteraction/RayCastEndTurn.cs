using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RayCastEndTurn : MonoBehaviour
{
    TurnManagerScript turnManagerScript;

    private void Start()
    {
        turnManagerScript = TurnManagerScript.Instance;
    }

    public void Interact(bool isBluePlayer)
    {   
        if (turnManagerScript.GetIsTurnBlue() == isBluePlayer || turnManagerScript.GetTurnBypass())
        {
            turnManagerScript.TurnManager();
        }
        SoundManager.Instance.PlayAmbient(001000003,transform.position);
    }
}
