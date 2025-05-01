using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    public GameObject playerCamera;
    public GameObject menu;

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
