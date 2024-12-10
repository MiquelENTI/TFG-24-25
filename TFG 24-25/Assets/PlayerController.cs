using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public GameObject playerCamera;
    public GameObject menu;
    public Button endTurnButton;
    public GameObject hand;
    private Text buttonText;
    public bool IsBlue;
    public int score = 0;

    private TurnManagerScript TurnManagerScript;
    private ScoreManager scoreManager;

    int turnCounter = 0;
    TeamType turnColor;

    void Start()
    {
        GameObject turnManagerObject = GameObject.Find("TurnManager");

        if (turnManagerObject != null)
        {
            TurnManagerScript = turnManagerObject.GetComponent<TurnManagerScript>();
            if (TurnManagerScript == null)
            {
                Debug.LogError("TurnManagerScript no encontrado en el GameObject 'TurnManager'.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el GameObject 'TurnManager' en la escena.");
        }

        GameObject scoreManagerObject = GameObject.Find("ScoreManager");
        if (scoreManagerObject != null)
        {
            scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
        }
        else
        {
            Debug.LogError("No se encontró el GameObject 'ScoreManager' en la escena.");
        }

        if (!photonView.IsMine)
        {
            playerCamera.SetActive(false);
        }
        else
        {
            playerCamera.SetActive(true);
        }

        if (menu != null)
        {
            menu.SetActive(false);
        }

        if (endTurnButton != null)
        {
            buttonText = endTurnButton.GetComponentInChildren<Text>();
            buttonText.text = "Finalizar Turno";
            endTurnButton.interactable = photonView.IsMine;
        }

        SetCardsDragState(photonView.IsMine);
    }

    [PunRPC]
    public void SetPlayerColor(bool isBlue)
    {
        IsBlue = isBlue;
        if (IsBlue)
        {
            Debug.Log("Este jugador es azul.");
        }
        else
        {
            Debug.Log("Este jugador es rojo.");
            // Si este jugador es rojo, desactivar su botón de turno
            if (endTurnButton != null)
            {
                endTurnButton.interactable = false;
                buttonText = endTurnButton.GetComponentInChildren<Text>();
                buttonText.text = "Esperando al otro jugador...";
            }
            SetCardsDragState(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu != null)
            {
                bool isActive = menu.activeSelf;
                menu.SetActive(!isActive);
            }
        }
    }

    public void EndTurn()
    {
        if (photonView.IsMine)
        {
            endTurnButton.interactable = false;
            buttonText.text = "Turno del oponente";
            SetCardsDragState(false);

            photonView.RPC("ActivateOtherPlayerTurn", RpcTarget.All);

            if (TurnManagerScript != null)
            {
                TurnManagerScript.TurnManager();
            }

        }
    }

    public void increaseScore()
    {
        score++;
        Debug.Log("Puntaje del jugador incrementado. Nuevo puntaje: " + score);

        if (photonView.IsMine && scoreManager != null)
        {
            photonView.RPC("UpdateScore", RpcTarget.All, IsBlue);
        }
    }

    [PunRPC]
    public void StartPlayer2()
    {
        Debug.Log("Llamando a la función StartPlayer2");
        if (photonView.IsMine)
        {
            endTurnButton.interactable = false;
            buttonText = endTurnButton.GetComponentInChildren<Text>();
            buttonText.text = "Finalizar Turno";
            SetCardsDragState(false);
        }
    }

    [PunRPC]
    public void ActivateOtherPlayerTurn()
    {
        Debug.Log("RPC llamado para activar el turno del otro jugador. ID de PhotonView: " + photonView.ViewID);

        Debug.Log("El turno es del otro jugador. Habilitando su botón.");
        CharactersManager.Instance.ActivateEndTurnCharactersByColor(IsBlue ? TeamType.BLUE : TeamType.RED);

        CharactersManager.Instance.ActivateStartTurnCharactersByColor(IsBlue ? TeamType.RED : TeamType.BLUE);

        if (turnCounter % 2 == 0)
        {
            turnColor = TeamType.BLUE;
            if (turnCounter != 0)
            {
                PlayerStats.Instance.IncreaseTotalMana(1);
            }
        }
        else
        {
            turnColor = TeamType.RED;
        }
        PlayerStats.Instance.ResetMana();

        turnCounter++;

        if (turnCounter >= 10) // PINSA KNOWS
        {
            // GAME OVER
        }
        else
        {

        }

        endTurnButton.interactable = true;
        buttonText.text = "Finalizar Turno";
        SetCardsDragState(true);
    }

    public void SetCardsDragState(bool state)
    {
        if (hand != null)
        {
            foreach (Transform card in hand.transform)
            {
                DragableUIObject dragableScript = card.GetComponent<DragableUIObject>();
                if (dragableScript != null)
                {
                    dragableScript.enabled = state;
                }
            }
        }
    }
}
