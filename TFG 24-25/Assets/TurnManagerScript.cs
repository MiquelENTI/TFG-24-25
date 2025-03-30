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

    int drawCardsStart = 2;

    int blueMana;
    int redMana;


    public ScoreManager scoreManager; 
    public GameObject finalCanvas;
    public TMP_Text winnerText;

    public bool isPCVersion = true;

    [SerializeField] private bool turnBypass;

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

        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager == null)
            {
                Debug.LogError("ScoreManager no encontrado en la escena.");
            }
        }

        if (finalCanvas == null)
        {
            finalCanvas = GameObject.Find("Final Canvas");
            if (finalCanvas == null)
            {
                Debug.LogError("Final Canvas no encontrado en la escena.");
            }
        }

        if (winnerText == null && finalCanvas != null)
        {
            winnerText = finalCanvas.transform.Find("Winner").GetComponent<TMP_Text>();
            if (winnerText == null)
            {
                Debug.LogError("Winner Text no encontrado como hijo de Final Canvas.");
            }
        }
    }

    public void RegisterPlayer(GameObject playerGameObject)
    {
        if (!player1Registered)
        {
            player1GameObject = playerGameObject;
            player1Registered = true;
            Debug.Log("Player 1 registrado en TurnManager: " + playerGameObject.name);
            DrawCards(5);
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
        photonView.RPC("DrawCards", RpcTarget.OthersBuffered, 1);
    }

    [PunRPC]
    public void UpdateTurn(bool newIsBlue)
    {
        SaveData.Instance.SaveNewAction("T" + (IsBlue ? "1" : "2"));

        IsBlue = newIsBlue;
        Debug.Log("Turno cambiado. Ahora es turno " + (IsBlue ? "Azul (Player 1)" : "Rojo (Player 2)"));

        if (newIsBlue)
        {
            Debug.Log("isBlueTurn: " + newIsBlue);
            //PlayerStats.Instance.IncreaseTotalMana(1);
            turnCounter++;
            turnCounterElement.text = "Turn Number: " + turnCounter;

            if (turnCounter > 10)
            {
                
                SaveData.Instance.Save();

                
                Debug.Log("Blue score is: " + scoreManager.BlueScore);
                Debug.Log("Red score is: " + scoreManager.RedScore);

                finalCanvas.SetActive(true);

                if (scoreManager.BlueScore > scoreManager.RedScore)
                {
                    winnerText.text = "Blue Wins!";
                    winnerText.color = Color.blue;
                }
                else if (scoreManager.RedScore > scoreManager.BlueScore)
                {
                    winnerText.text = "Red Wins!";
                    winnerText.color = Color.red;
                }
            }
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

    public int GetIsBlueInt()
    { return IsBlue ? 1 : 0; }

    [PunRPC]
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DeckManager.Instance.DrawCard(IsBlue);
        }
    }

    public void SetIsPCVersion(bool state)
    {
        isPCVersion = state;
    }

    public bool GetTurnBypass()
    {
        return turnBypass;
    }
}