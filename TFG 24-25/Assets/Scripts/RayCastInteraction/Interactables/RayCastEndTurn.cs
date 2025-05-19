using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RayCastEndTurn : RayCastInteractable
{
    TurnManagerScript turnManagerScript;

    private void Start()
    {
        turnManagerScript = TurnManagerScript.Instance;
    }

    public override void Interact(bool isBluePlayer)
    {
        if (turnManagerScript.GetIsTurnBlue() == isBluePlayer || turnManagerScript.GetTurnBypass())
        {
            turnManagerScript.TurnManager();
        }
        RingBell();
    }
    //Implementació del so 
    public void RingBell()
    {
        SoundManager.Instance.PlayMusic(001000003,transform.position);
    }
}
