using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RayCastEndTurn : MonoBehaviour
{
    TurnManagerScript turnManagerScript;

    [SerializeField] Material interactAvailable;
    [SerializeField] Material interactUnavailable;

    Renderer r;

    PhotonView photonView;

    private void Start()
    {
        turnManagerScript = TurnManagerScript.Instance;
        photonView = GetComponent<PhotonView>();

        r = GetComponent<Renderer>();
    }

    public void Interact(bool isBluePlayer)
    {   
        if (turnManagerScript.getIsBlue() == isBluePlayer || turnManagerScript.GetTurnBypass())
        {
            turnManagerScript.TurnManager();
            ChangeMaterial();
            r.material = interactUnavailable;
        }
        if (turnManagerScript.GetTurnBypass())
        {
            r.material = interactAvailable;
        }
    }

    public void ChangeMaterial()
    {
        photonView.RPC("ChangeMaterial_RPC", RpcTarget.Others);
    }

    [PunRPC]
    public void ChangeMaterial_RPC()
    {
        r.material = interactAvailable;

    }
}
