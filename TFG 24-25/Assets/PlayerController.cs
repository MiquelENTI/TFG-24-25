using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public GameObject playerCamera;
    public GameObject menu;
    public Button endTurnButton;
    private Text buttonText;

    void Start()
    {
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

            photonView.RPC("ActivateOtherPlayerButton", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void ActivateOtherPlayerButton()
    {
        if (photonView.IsMine)
        {
            endTurnButton.interactable = true;
            buttonText.text = "Finalizar Turno";
        }
    }
}
