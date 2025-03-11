using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class TurnManagerScript : Singleton<TurnManagerScript>
{
    [SerializeField] private bool IsBlue = true;
    int turnCounter = 1;

    [SerializeField] public GameObject player1GameObject;
    [SerializeField] public GameObject player2GameObject;

    private bool player1Registered = false;
    private bool player2Registered = false;

    PhotonView photonView;
    public TMP_Text turnCounterElement;

    int blueMana;
    int redMana;

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

    public void RegisterPlayer(GameObject playerGameObject)
    {
        if (!player1Registered)
        {
            player1GameObject = playerGameObject;
            player1Registered = true;
            Debug.Log("Player 1 registrado en TurnManager: " + playerGameObject.name);
        }
        else if (!player2Registered)
        {
            player2GameObject = playerGameObject;
            player2Registered = true;
            Debug.Log("Player 2 registrado en TurnManager: " + playerGameObject.name);
        }
        else
        {
            Debug.LogWarning("Ya se han registrado Player 1 y Player 2. No se pueden registrar más jugadores a través de este TurnManager.");
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
        Debug.Log("Turno cambiado. Ahora es turno " + (IsBlue ? "Azul (Player 1)" : "Rojo (Player 2)"));

        if (newIsBlue)
        {
            Debug.Log("isBlueTurn: " + newIsBlue);
            PlayerStats.Instance.IncreaseTotalMana(1);
            turnCounter++;
            //turnCounterElement.text = "Turn Number: " + turnCounter;
        }

        CharactersManager.Instance.ActivateEndTurnCharactersByColor(IsBlue);
        CharactersManager.Instance.ActivateStartTurnCharactersByColor(IsBlue);

        Debug.Log("TO RESET MANA");

        PlayerStats.Instance.ResetMana();

        UpdatePlayerCanvas();
    }

    private void UpdatePlayerCanvas()
    {
        if (player1GameObject != null && player2GameObject != null)
        {
            Canvas canvasPlayer1 = player1GameObject.GetComponentInChildren<Canvas>();
            Canvas canvasPlayer2 = player2GameObject.GetComponentInChildren<Canvas>();

            if (canvasPlayer1 != null && canvasPlayer2 != null)
            {
                if (IsBlue)
                {
                    canvasPlayer1.enabled = true;
                    canvasPlayer2.enabled = false;
                    Debug.Log("Turno de Player 1. Canvas de Player 1 activado, Canvas de Player 2 desactivado.");
                }
                else
                {
                    canvasPlayer2.enabled = true;
                    canvasPlayer1.enabled = false;
                    Debug.Log("Turno de Player 2. Canvas de Player 2 activado, Canvas de Player 1 desactivado.");
                }
            }
            else
            {
                Debug.LogWarning("Uno o ambos jugadores no tienen un Canvas en sus GameObjects hijos.");
            }
        }
        else
        {
            Debug.LogWarning("Player 1 o Player 2 no han sido registrados todavía en TurnManager.");
        }
    }


    public bool getIsBlue()
    {
        return IsBlue;
    }
}