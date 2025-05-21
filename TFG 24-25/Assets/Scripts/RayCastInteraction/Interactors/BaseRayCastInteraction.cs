using Photon.Pun;
using UnityEngine;

public class BaseRayCastInteraction : MonoBehaviour
{
    protected bool isBluePlayer;
    protected PhotonView photonView;

    protected PlayerInputs playerInputs;
    protected TurnManagerScript turnManagerScript;
    protected CardHold cardHold;

    protected Camera playerCamera;

    protected DragableGameObject lastInteractedObject;
    protected GameObject lastInteractedRaycastGameObject;
    protected RayCastTile rayCastTile;

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
