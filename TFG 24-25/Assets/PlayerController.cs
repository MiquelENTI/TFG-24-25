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

        SetCardsDragState(photonView.IsMine);
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

            photonView.RPC("ActivateOtherPlayerTurn", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void ActivateOtherPlayerTurn()
    {
        Debug.Log("RPC called on player with PhotonView ID: " + photonView.ViewID);
        if (photonView.IsMine)
        {
            endTurnButton.interactable = true;
            buttonText.text = "Finalizar Turno";
            SetCardsDragState(true);
        }
    }

    private void SetCardsDragState(bool state)
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
