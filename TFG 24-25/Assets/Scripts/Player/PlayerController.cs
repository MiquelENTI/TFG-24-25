using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public GameObject playerCamera;
    public GameObject menu;
    public GameObject hand;

    public bool IsBlue;

    private GameObject playerHead;

    private Vector3 PC_WizardHeadOffset = new Vector3(0, -0.515f, -0.7f);
    private Vector3 PC_KnightHeadOffset = new Vector3(0, -0.515f, 0.7f);

    private Vector3 VR_WizardHeadOffset;
    private Vector3 VR_KnightHeadOffset;

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

        if (!photonView.IsMine) { return; }

        if (TurnManagerScript.Instance.isPCVersion)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerHead = PhotonNetwork.Instantiate("WizardHead", playerCamera.transform.position + PC_WizardHeadOffset, Quaternion.identity);
                playerHead.transform.parent = gameObject.transform.GetChild(0);
            }
            else
            {
                playerHead = PhotonNetwork.Instantiate("KnightHead", playerCamera.transform.position + PC_KnightHeadOffset, Quaternion.Euler(0,180,0));
                playerHead.transform.parent = gameObject.transform.GetChild(0);
            }

        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerHead = PhotonNetwork.Instantiate("WizardHead", Vector3.zero, Quaternion.identity);
                playerHead.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
            }
            else
            {
                playerHead = PhotonNetwork.Instantiate("KnightHead", Vector3.zero, Quaternion.identity);
                playerHead.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
            }
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
}
