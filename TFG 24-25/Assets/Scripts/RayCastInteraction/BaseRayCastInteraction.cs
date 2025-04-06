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

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        turnManagerScript = GameObject.Find("TurnManager").GetComponent<TurnManagerScript>();
        cardHold = GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold").GetComponent<CardHold>();
        playerCamera = transform.GetChild(0).GetComponent<Camera>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
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
