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

    private TurnManagerScript TurnManagerScript;

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
