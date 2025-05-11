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



    private void Awake()
    {
       
    }

    void Start()
    {
        if (!photonView.IsMine)
        {
            ToggleCameraGameObject(false);
            return;
        }
        else
        {
            ToggleCameraGameObject(true);
        }

        if (TurnManagerScript.Instance.isPCVersion)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerHead = PhotonNetwork.Instantiate("WizardHead", playerCamera.transform.position + PC_WizardHeadOffset, Quaternion.identity);
                PhotonView.FindObjectOfType<ChangeParentPhoton>().gameObject.transform.parent = gameObject.transform.GetChild(0);
                //playerHead.transform.parent = gameObject.transform.GetChild(0);
            }
            else
            {
                playerHead = PhotonNetwork.Instantiate("KnightHead", playerCamera.transform.position + PC_KnightHeadOffset, Quaternion.Euler(0, 180, 0));
                PhotonView.FindObjectOfType<ChangeParentPhoton>().gameObject.transform.parent = gameObject.transform.GetChild(0);
                //playerHead.transform.parent = gameObject.transform.GetChild(0);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerHead = PhotonNetwork.Instantiate("WizardHead", playerCamera.transform.position + VR_WizardHeadOffset, Quaternion.identity);
                PhotonView.FindObjectOfType<ChangeParentPhoton>().gameObject.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
                //playerHead.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
            }
            else
            {
                playerHead = PhotonNetwork.Instantiate("KnightHead", playerCamera.transform.position + VR_KnightHeadOffset, Quaternion.Euler(0, 180, 0));
                PhotonView.FindObjectOfType<ChangeParentPhoton>().gameObject.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
                //playerHead.transform.parent = gameObject.transform.GetChild(2).GetChild(0).GetChild(0);
            }
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

    private void ToggleCameraGameObject(bool state)
    {
        playerCamera.GetComponent<Camera>().enabled = state;
    }
}
