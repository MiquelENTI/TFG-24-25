using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TurnManagerScript : MonoBehaviourPun
{
    [SerializeField] bool IsBlue = true;
    int turnCounter = 1;

    PhotonView photonView;

    public TMP_Text turnCounterElement;

    private void Awake()
    {
        if (!TryGetComponent<PhotonView>(out photonView))
        {
            Debug.LogError("PhotonView no encontrado en el GameObject.");
        }
    }

    private void Start()
    {
        if (!photonView)
        {
            Debug.LogError("El PhotonView no está asignado correctamente en el GameObject");
            return;
        }
    }

    public void TurnManager()
    {
        if (photonView == null)
        {
            Debug.LogError("PhotonView no está asignado correctamente.");
            return;
        }

        IsBlue = !IsBlue;

        photonView.RPC("UpdateTurn", RpcTarget.AllBuffered, IsBlue);
    }

    [PunRPC]
    public void UpdateTurn(bool newIsBlue)
    {
        IsBlue = newIsBlue;
        Debug.Log("El turno ha cambiado. Ahora es turno " + (IsBlue ? "Azul" : "Rojo"));
        if(newIsBlue)        {
            turnCounter++;
            turnCounterElement.text = "Turn Number: " + turnCounter;
        }
    }

    public bool getIsBlue()
    {
        return IsBlue;
    }
}
