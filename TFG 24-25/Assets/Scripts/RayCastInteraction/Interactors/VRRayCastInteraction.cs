using System.Collections;
using Photon.Pun;
using UnityEngine;

public enum HandState { IDLE = 0, POINTING = 1, GRABBING = 2, INTERACT = 3 }

public class VRRaycastInteraction : BaseRaycastInteraction
{
    private GameObject right_controller;
    private GameObject left_controller;

    private DragableGameObject right_lastInteractedObject;
    private DragableGameObject left_lastInteractedObject;

    private RaycastTile right_rayCastTile;
    private RaycastTile left_rayCastTile;

    [SerializeField] private Animator right_handAnimator;
    private HandState right_handState = HandState.IDLE;
    private bool right_blockChangeAnimation = false;

    [SerializeField] private Animator left_handAnimator;
    private HandState left_handState = HandState.IDLE;
    private bool left_blockChangeAnimation = false;

    private WaitForFixedUpdate right_waitForFixedUpdate = new WaitForFixedUpdate();
    private WaitForFixedUpdate left_waitForFixedUpdate = new WaitForFixedUpdate();

    UpdateHandVR rightHandUpdate;
    UpdateHandVR leftHandUpdate;
    
    private void Start()
    {

        if (!photonView.IsMine)
        {
            return;
        }
        #region RIGHT HAND

        // Weird Button

        playerInputs.XRIRightHandInteraction.Select.started += _ => { RightHand_SelectInteractionDown(); };
        playerInputs.XRIRightHandInteraction.Select.canceled += _ => { RightHand_SelectInteractionUp(); };

        // Trigger
        playerInputs.XRIRightHandInteraction.Activate.started += _ => { RightHand_ActivateInteractionDown(); };
        playerInputs.XRIRightHandInteraction.Activate.canceled += _ => { RightHand_ActivateInteractionUp(); };
        playerInputs.XRIRightHand.ThumbStickClicked.started += _ => { GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold").GetComponent<CardHold>().ReorganizeCards(); };
        
        /* 
        playerInputs.XRIRightHand.ThumbStickClicked.started += _ => {
            photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.GRABBING, right_blockChangeAnimation);
            Right_ChangeHandState((int)HandState.GRABBING, right_blockChangeAnimation);
            right_blockChangeAnimation = true;
        };
        playerInputs.XRIRightHand.ThumbStickClicked.canceled += _ => {

            right_blockChangeAnimation = false;
            photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, right_blockChangeAnimation);
            Right_ChangeHandState((int)HandState.IDLE, right_blockChangeAnimation);
        };
        */

        #endregion

        #region LEFT HAND

        // Weird Button
        playerInputs.XRILeftHandInteraction.Select.started += _ => { LeftHand_SelectInteractionDown(); };
        playerInputs.XRILeftHandInteraction.Select.canceled += _ => { LeftHand_SelectInteractionUp(); };

        // Trigger
        playerInputs.XRILeftHandInteraction.Activate.started += _ => { LeftHand_ActivateInteractionDown(); };
        playerInputs.XRILeftHandInteraction.Activate.canceled += _ => { LeftHand_ActivateInteractionUp(); };


        //InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        //InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
        #endregion
    }

    private void Update()
    {
        
        if (right_controller != null || left_controller != null) 
        {
            Right_HoverUpdate();
            Left_HoverUpdate();
            return; 
        }

        if (right_controller == null)
        {
            right_controller = transform.GetChild(2).GetChild(0).GetChild(2).GetChild(4).gameObject;
            rightHandUpdate = transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<UpdateHandVR>();
        }
        if(left_controller == null)
        {
            left_controller = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(4).gameObject;
            leftHandUpdate = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<UpdateHandVR>();
        }
        Debug.Log("WORK");
    }

    #region RIGHT HAND
    protected virtual void RightHand_SelectInteractionDown()
    {
        Ray ray = new Ray(right_controller.transform.position, right_controller.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider == null)
            {
                photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, right_blockChangeAnimation);
                Right_ChangeHandState((int)HandState.IDLE, right_blockChangeAnimation);
                Debug.Log("ENTERED RETURN DOWN");
                return; 
            }

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                {
                    right_lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                    TokenGameObject temp = (TokenGameObject)right_lastInteractedObject;
                    CellNodeManager.Instance.showPossibleMovements.Invoke(temp.GetCharacter().GetOnTileId());
                    break;
                }
                case "CardGameObject":
                {
                    right_lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                    CellNodeManager.Instance.showPossibleSpawnTiles.Invoke(right_lastInteractedObject.GetIsBlue() ? TeamType.BLUE : TeamType.RED);
                    Debug.Log("ENTERED CARDGO");
                    break;
                }
                default:
                {
                    lastInteractedRaycastGameObject.GetComponent<RaycastInteractable>().Inspect();
                    Debug.Log("ENTERED HERE");
                    return;
                }
            }

            if ((right_lastInteractedObject.GetIsBlue() == turnManagerScript.GetIsTurnBlue()) && hit.transform.GetComponent<PhotonView>().IsMine)
            {
                Debug.Log("ENTERED RIGHT DRAG UPDATE");
                StartCoroutine(Right_DragUpdate(hit.collider.gameObject));
                photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.GRABBING, right_blockChangeAnimation);
                Right_ChangeHandState((int)HandState.GRABBING, right_blockChangeAnimation);
                right_blockChangeAnimation = true;
            }
        }
    }
    protected virtual void RightHand_SelectInteractionUp()
    {
        VFXManager.Instance.RemoveTeamHighlights();
        
        if (right_lastInteractedObject == null)
        { return; } 

        right_blockChangeAnimation = false;
        photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, right_blockChangeAnimation);
        Right_ChangeHandState((int)HandState.POINTING, right_blockChangeAnimation);

        switch (right_lastInteractedObject.tag)
        {
            case "TokenGameObject":
            {
                right_lastInteractedObject.CheckIsOutsideBoard();

                right_lastInteractedObject = null;
                break;
            }
            case "CardGameObject":
            {
                right_lastInteractedObject.CheckIsOutsideBoard();

                right_lastInteractedObject = null;

                    Debug.Log("ENTERED SPAWN");

                break;
            }
            default:
            {
                break;
            }
        }
    }

    protected virtual void RightHand_ActivateInteractionDown()
    {
        Ray ray = new Ray(right_controller.transform.position, right_controller.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            {
                photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, right_blockChangeAnimation);
                Right_ChangeHandState((int)HandState.IDLE, right_blockChangeAnimation);
                return; 
            }

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                case "CardGameObject":
                {
                        Debug.Log("ENTERED INSPECT");
                    right_lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                    right_lastInteractedObject.SeeTokenCard();
                    right_lastInteractedObject.ToggleCardToDisplay(true);
                    right_lastInteractedObject.RotateCardToDisplayVR();
                    displayingCard = true;

                    photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.INTERACT, right_blockChangeAnimation);
                    Right_ChangeHandState((int)HandState.INTERACT, right_blockChangeAnimation);
                    right_blockChangeAnimation = true;
                    break;
                }
                case "RaycastInteractable":
                {
                    lastInteractedRaycastGameObject = hit.transform.gameObject;

                    if (lastInteractedRaycastGameObject.name == "DingDongEndTurn")
                    { lastInteractedRaycastGameObject.GetComponent<RaycastInteractable>().Interact(isBluePlayer); }
                    else
                    { lastInteractedRaycastGameObject.GetComponent<RaycastInteractable>().Interact(); }

                    photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.INTERACT, right_blockChangeAnimation);
                    Right_ChangeHandState((int)HandState.INTERACT, right_blockChangeAnimation);
                    right_blockChangeAnimation = true;
                    break;
                }
            }
        }
    }

    protected virtual void RightHand_ActivateInteractionUp()
    {
        if (right_lastInteractedObject == null)
        { return; }

        right_blockChangeAnimation = false;
        photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, right_blockChangeAnimation);
        Right_ChangeHandState((int)HandState.POINTING, right_blockChangeAnimation);

        if (displayingCard)
        {
            right_lastInteractedObject.GetComponent<DragableGameObject>().ToggleCardToDisplay(false);
            displayingCard = false;
        }
    }


    #endregion

    #region LEFT HAND

    protected virtual void LeftHand_SelectInteractionDown()
    {
        Ray ray = new Ray(left_controller.transform.position, left_controller.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            {
                photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, left_blockChangeAnimation);
                Left_ChangeHandState((int)HandState.IDLE, left_blockChangeAnimation);

                return; 
            }

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                {
                    left_lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                    TokenGameObject temp = (TokenGameObject)left_lastInteractedObject;
                    CellNodeManager.Instance.showPossibleMovements.Invoke(temp.GetCharacter().GetOnTileId());
                    break;
                }
                case "CardGameObject":
                {
                    left_lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                    CellNodeManager.Instance.showPossibleSpawnTiles.Invoke(right_lastInteractedObject.GetIsBlue() ? TeamType.BLUE : TeamType.RED);
                    Debug.Log("ENTERED CARDGO");
                    break;
                }
                default:
                {
                    Debug.Log("ENTERED HERE");
                    break;
                }
            }

            if (hit.collider.tag == "TokenGameObject" || hit.transform.tag == "CardGameObject")
            {
                if ((left_lastInteractedObject.GetIsBlue() == turnManagerScript.GetIsTurnBlue()) && hit.transform.GetComponent<PhotonView>().IsMine)
                {
                    
                    StartCoroutine(Left_DragUpdate(hit.collider.gameObject));
                    photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.GRABBING, left_blockChangeAnimation);
                    Left_ChangeHandState((int)HandState.GRABBING, left_blockChangeAnimation);
                    left_blockChangeAnimation = true;
                }
                return;
            }

            if (hit.collider.tag == "RaycastInteractable")
            { lastInteractedRaycastGameObject.GetComponent<RaycastInteractable>().Inspect(); }
        }
    }
    protected virtual void LeftHand_SelectInteractionUp()
    {
        VFXManager.Instance.RemoveTeamHighlights();
        left_blockChangeAnimation = false;
        photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, left_blockChangeAnimation);
        Left_ChangeHandState((int)HandState.POINTING, left_blockChangeAnimation);

        if (left_lastInteractedObject == null)
        { return; }

        switch (left_lastInteractedObject.tag)
        {
            case "TokenGameObject":
            {
                left_lastInteractedObject.CheckIsOutsideBoard();

                left_lastInteractedObject = null;
                break;
            }
            case "CardGameObject":
            {
                left_lastInteractedObject.CheckIsOutsideBoard();

                left_lastInteractedObject = null;
                break;
            }
            default:
            {
                break;
            }
        }
    }

    protected virtual void LeftHand_ActivateInteractionDown()
    {
        Ray ray = new Ray(left_controller.transform.position, left_controller.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            {
                photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, left_blockChangeAnimation);
                Left_ChangeHandState((int)HandState.POINTING, left_blockChangeAnimation);
                return; 
            }

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                case "CardGameObject":
                    {
                        left_lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                        left_lastInteractedObject.SeeTokenCard();
                        left_lastInteractedObject.ToggleCardToDisplay(true);
                        left_lastInteractedObject.RotateCardToDisplayVR();
                        displayingCard = true;

                        photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.INTERACT, left_blockChangeAnimation);
                        Left_ChangeHandState((int)HandState.INTERACT, left_blockChangeAnimation);
                        left_blockChangeAnimation = true;
                        break;
                    }
                case "RaycastInteractable":
                {
                    lastInteractedRaycastGameObject = hit.transform.gameObject; 

                    if (lastInteractedRaycastGameObject.name == "DingDongEndTurn")
                    { lastInteractedRaycastGameObject.GetComponent<RaycastInteractable>().Interact(isBluePlayer); }
                    else
                    { lastInteractedRaycastGameObject.GetComponent<RaycastInteractable>().Interact(); }

                    photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.INTERACT, left_blockChangeAnimation);
                    Left_ChangeHandState((int)HandState.INTERACT, left_blockChangeAnimation);
                    left_blockChangeAnimation = true;

                    break;
                }
            }
        }
    }

    protected virtual void LeftHand_ActivateInteractionUp()
    {
        left_blockChangeAnimation = false;
        photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, left_blockChangeAnimation);
        Left_ChangeHandState((int)HandState.POINTING, left_blockChangeAnimation);

        if (left_lastInteractedObject == null)
        { return; }

        if (displayingCard)
        {
            left_lastInteractedObject.GetComponent<DragableGameObject>().ToggleCardToDisplay(false);
            displayingCard = false;
        }
    }
    #endregion

    public virtual IEnumerator Right_DragUpdate(GameObject clickedGameObject)
    {
        right_rayCastTile = clickedGameObject.transform.GetChild(1).GetComponent<RaycastTile>();

        while (playerInputs.XRIRightHandInteraction.Select.ReadValue<float>() != 0)
        {
            right_rayCastTile.UpdateRaycast();
            yield return right_waitForFixedUpdate;
        }
    }

    public virtual IEnumerator Left_DragUpdate(GameObject clickedGameObject)
    {
        left_rayCastTile = clickedGameObject.transform.GetChild(1).GetComponent<RaycastTile>();

        while (playerInputs.XRILeftHandInteraction.Select.ReadValue<float>() != 0)
        {
            left_rayCastTile.UpdateRaycast();

            yield return left_waitForFixedUpdate;
        }
    }

    public virtual void Right_HoverUpdate()
    {
        rightHandUpdate.HandsPositionUpdating(right_controller.transform.position, right_controller.transform.rotation);

        Ray ray = new Ray(right_controller.transform.position, right_controller.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider == null)
            {
                photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, right_blockChangeAnimation);
                Right_ChangeHandState((int)HandState.IDLE, right_blockChangeAnimation);
                return;
            }

            //Debug.Log(hit.collider.name);

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                case "CardGameObject":
                case "RaycastInteractable":
                {
                        photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, right_blockChangeAnimation);
                        Right_ChangeHandState((int)HandState.POINTING, right_blockChangeAnimation);
                        break;
                }
                default:
                {
                        photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, right_blockChangeAnimation);
                        Right_ChangeHandState((int)HandState.IDLE, right_blockChangeAnimation);
                        break;
                }
            }
        }
    }

    public virtual void Left_HoverUpdate()
    {
        leftHandUpdate.HandsPositionUpdating(left_controller.transform.position, left_controller.transform.rotation);
        Ray ray = new Ray(left_controller.transform.position, left_controller.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider == null)
            {
                photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, left_blockChangeAnimation);
                Left_ChangeHandState((int)HandState.IDLE, left_blockChangeAnimation);
                return;
            }

            //Debug.Log(hit.collider.name);

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                case "CardGameObject":
                case "RaycastInteractable":
                {
                    photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, left_blockChangeAnimation);
                        Left_ChangeHandState((int)HandState.POINTING, left_blockChangeAnimation);
                        break;
                }
                default:
                {
                    photonView.RPC("Left_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, left_blockChangeAnimation);
                        Left_ChangeHandState((int)HandState.IDLE, left_blockChangeAnimation);
                        break;
                }
            }
        }
    }

    [PunRPC]
    protected virtual void Right_ChangeHandState(int HandStateTemp, bool blockAnimation)
    {
        HandState newHandState = (HandState)HandStateTemp;
        

        if (photonView.IsMine)
        {
            if (right_handState == newHandState || blockAnimation) { return; }

            switch (newHandState)
            {
                case HandState.IDLE:
                    {
                        right_handAnimator.SetBool("RightIndexPoint", false);
                        right_handAnimator.SetBool("RightIndexInteract", false);
                        right_handAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.POINTING:
                    {
                        right_handAnimator.SetBool("RightIndexPoint", true);
                        right_handAnimator.SetBool("RightIndexInteract", false);
                        right_handAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.GRABBING:
                    {

                        right_handAnimator.SetBool("RightIndexPoint", true);
                        right_handAnimator.SetBool("RightIndexInteract", false);
                        right_handAnimator.SetBool("RightHandGrab", true);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.INTERACT:
                    {
                        right_handAnimator.SetBool("RightIndexPoint", true);
                        right_handAnimator.SetBool("RightIndexInteract", true);
                        right_handAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
            }
        }
        else
        {
            if (left_handState == newHandState || blockAnimation) { return; }

            switch (newHandState)
            {
                case HandState.IDLE:
                    {
                        left_handAnimator.SetBool("LeftIndexPoint", false);
                        left_handAnimator.SetBool("LeftIndexInteract", false);
                        left_handAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.POINTING:
                    {
                        left_handAnimator.SetBool("LeftIndexPoint", true);
                        left_handAnimator.SetBool("LeftIndexInteract", false);
                        left_handAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.GRABBING:
                    {

                        left_handAnimator.SetBool("LeftIndexPoint", true);
                        left_handAnimator.SetBool("LeftIndexInteract", false);
                        left_handAnimator.SetBool("LeftHandGrab", true);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.INTERACT:
                    {
                        left_handAnimator.SetBool("LeftIndexPoint", true);
                        left_handAnimator.SetBool("LeftIndexInteract", true);
                        left_handAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
            }
        }
    }

    [PunRPC]
    protected virtual void Left_ChangeHandState(int HandStateTemp, bool blockAnimation)
    {
        HandState newHandState = (HandState)HandStateTemp;
        
        if (photonView.IsMine)
        {
            if (left_handState == newHandState || blockAnimation) { return; }

            switch (newHandState)
            {
                case HandState.IDLE:
                    {
                        left_handAnimator.SetBool("LeftIndexPoint", false);
                        left_handAnimator.SetBool("LeftIndexInteract", false);
                        left_handAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.POINTING:
                    {
                        left_handAnimator.SetBool("LeftIndexPoint", true);
                        left_handAnimator.SetBool("LeftIndexInteract", false);
                        left_handAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.GRABBING:
                    {

                        left_handAnimator.SetBool("LeftIndexPoint", true);
                        left_handAnimator.SetBool("LeftIndexInteract", false);
                        left_handAnimator.SetBool("LeftHandGrab", true);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.INTERACT:
                    {
                        left_handAnimator.SetBool("LeftIndexPoint", true);
                        left_handAnimator.SetBool("LeftIndexInteract", true);
                        left_handAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
            }
        }
        else
        {
            if (right_handState == newHandState || blockAnimation) { return; }

            switch (newHandState)
            {
                case HandState.IDLE:
                    {
                        right_handAnimator.SetBool("RightIndexPoint", false);
                        right_handAnimator.SetBool("RightIndexInteract", false);
                        right_handAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.POINTING:
                    {
                        right_handAnimator.SetBool("RightIndexPoint", true);
                        right_handAnimator.SetBool("RightIndexInteract", false);
                        right_handAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.GRABBING:
                    {

                        right_handAnimator.SetBool("RightIndexPoint", true);
                        right_handAnimator.SetBool("RightIndexInteract", false);
                        right_handAnimator.SetBool("RightHandGrab", true);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.INTERACT:
                    {
                        right_handAnimator.SetBool("RightIndexPoint", true);
                        right_handAnimator.SetBool("RightIndexInteract", true);
                        right_handAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
            }
        }
        
    }
}
