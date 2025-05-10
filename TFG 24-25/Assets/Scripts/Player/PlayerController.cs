using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public GameObject playerCamera;
    public GameObject menu;
    public GameObject hand;

    public bool IsBlue;

    private GameObject wizardHead;
    private GameObject knightHead;

    void Start()
    {
        if (TurnManagerScript.Instance.isPCVersion)
        {
            knightHead = gameObject.transform.GetChild(0).GetChild(1).gameObject;
            wizardHead = gameObject.transform.GetChild(0).GetChild(2).gameObject;
        }
        else
        {
            knightHead = gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(1).gameObject;
            wizardHead = gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).gameObject;
        }

        photonView.RPC("ActivatePlayerHeads", RpcTarget.AllBuffered);

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
    }

    [PunRPC]
    public void SetPlayerColor(bool isBlue)
    {
        IsBlue = isBlue;
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

    [PunRPC]
    public void ActivatePlayerHeads()
    {
        if (IsBlue)
        {
            knightHead.SetActive(false);
            wizardHead.SetActive(true);
        }
        else
        {
            knightHead.SetActive(true);
            wizardHead.SetActive(false);
        }
    }
}
