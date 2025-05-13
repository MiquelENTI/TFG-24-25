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
        if (photonView.IsMine)
        photonView.RPC("ChangeTagMasterOrClient", RpcTarget.AllBuffered, PhotonNetwork.IsMasterClient);
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

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("TRY");
            GameObject.FindGameObjectWithTag("WizardHead").GetComponent<ChangeParentPhoton>().ChangeParentWithTag("Master", PC_WizardHeadOffset, VR_WizardHeadOffset);
            Debug.Log("DONE");
            //playerHead.transform.parent = gameObject.transform.GetChild(0);
        }
        else
        {
            GameObject.FindGameObjectWithTag("KnightHead").GetComponent<ChangeParentPhoton>().ChangeParentWithTag("Client", PC_KnightHeadOffset, VR_KnightHeadOffset);
            //playerHead.transform.parent = gameObject.transform.GetChild(0);
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

    [PunRPC]
    public void ChangeTagMasterOrClient(bool master)
    {
        if (master)
        {
            gameObject.tag = "Master";
        }
        else
        {
            gameObject.tag = "Client";
        }
    }
}
