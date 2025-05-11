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

    private Vector3 VR_WizardHeadOffset = new Vector3(0f, -0.155f, -0.25f);
    private Vector3 VR_KnightHeadOffset = new Vector3(0f, -0.155f, 0.25f);

    bool headCreated = false;


    private void Awake()
    {
       
    }

    void Start()
    {
        if (!photonView.IsMine)
        {
            playerCamera.SetActive(false);
            return;
        }
        else
        {
            playerCamera.SetActive(true);
        }

        if (TurnManagerScript.Instance.isPCVersion)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerHead = PhotonNetwork.Instantiate("WizardHead", playerCamera.transform.position + PC_WizardHeadOffset, Quaternion.identity);
                playerHead.transform.parent = gameObject.transform.GetChild(0);
            }
            else
            {
                playerHead = PhotonNetwork.Instantiate("KnightHead", playerCamera.transform.position + PC_KnightHeadOffset, Quaternion.Euler(0, 180, 0));
                playerHead.transform.parent = gameObject.transform.GetChild(0);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerHead = PhotonNetwork.Instantiate("WizardHead", playerCamera.transform.position + VR_WizardHeadOffset, Quaternion.identity);
                playerHead.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
            }
            else
            {
                playerHead = PhotonNetwork.Instantiate("KnightHead", playerCamera.transform.position + VR_KnightHeadOffset, Quaternion.Euler(0, 180, 0));
                playerHead.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
            }
        }

        headCreated = true;

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
}
