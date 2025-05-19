using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class BaseRayCastInteraction : MonoBehaviour
{
    protected PlayerInputs playerInputs;
    protected TurnManagerScript turnManagerScript;
    protected CardHold cardHold;

    protected Camera playerCamera;

    protected DragableGameObject lastInteractedObject;

    protected GameObject lastInteractedRaycastGameObject;

    protected RayCastTile rayCastTile;

    protected bool isBluePlayer;
    protected PhotonView photonView;

    protected bool displayingCard = false;
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        turnManagerScript = TurnManagerScript.Instance;
        cardHold = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold").GetComponent<CardHold>();
        playerCamera = transform.GetChild(0).GetComponent<Camera>();

        isBluePlayer = PhotonNetwork.IsMasterClient;
        photonView = gameObject.GetComponent<PhotonView>();
    }
    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }
}
