using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public enum HandState { IDLE = 0, POINTING = 1, GRABBING = 2, INTERACT = 3 }

public class VRRayCastInteraction : BaseRayCastInteraction
{
    private WaitForFixedUpdate right_waitForFixedUpdate = new WaitForFixedUpdate();
    private WaitForFixedUpdate left_waitForFixedUpdate = new WaitForFixedUpdate();

    private DragableGameObject right_lastInteractedObject;
    private DragableGameObject left_lastInteractedObject;

    private RayCastTile right_rayCastTile;
    private RayCastTile left_rayCastTile;

    private GameObject rightController;
    private GameObject leftController;

    [SerializeField] private Animator rightHandAnimator;
    HandState right_handState = HandState.IDLE;
    bool right_blockChangeAnimation = false;

    [SerializeField] private Animator leftHandAnimator;
    HandState left_handState = HandState.IDLE;
    bool left_blockChangeAnimation = false;

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

        if (rightController != null || leftController != null) 
        {
            Right_HoverUpdate();
            Left_HoverUpdate();
            return; 
        }

        if (rightController == null)
        {
            rightController = transform.GetChild(2).GetChild(0).GetChild(2).GetChild(4).gameObject;
            rightHandUpdate = transform.GetChild(2).GetChild(0).GetChild(2).GetComponent<UpdateHandVR>();
        }
        if(leftController == null)
        {
            leftController = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(4).gameObject;
            leftHandUpdate = transform.GetChild(2).GetChild(0).GetChild(1).GetComponent<UpdateHandVR>();
        }
    }

    #region RIGHT HAND
    protected virtual void RightHand_SelectInteractionDown()
    {
        Debug.Log("ACTIVATE DOWN");
        
        Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);

        Debug.Log("RIGHT HAND POS: " + rightController.transform.position + " RIGHT HAND DIRECTION: " + -rightController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider == null)
            {
                photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.IDLE, right_blockChangeAnimation);
                Right_ChangeHandState((int)HandState.IDLE, right_blockChangeAnimation);
                Debug.Log("ENTERED RETURN DOWN");
                return; }

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
                        Debug.Log("ENTERED HERE");
                        break;
                    }
            }

            if (hit.collider.tag == "TokenGameObject" || hit.transform.tag == "CardGameObject")
            {
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
    }
    protected virtual void RightHand_SelectInteractionUp()
    {
        EffectsManager.Instance.RemoveTeamEffects();
        right_blockChangeAnimation = false;
        photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, right_blockChangeAnimation);
        Right_ChangeHandState((int)HandState.POINTING, right_blockChangeAnimation);
        if (right_lastInteractedObject == null)
        { return; }

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
        Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            {
                photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, right_blockChangeAnimation);
                Right_ChangeHandState((int)HandState.POINTING, right_blockChangeAnimation);
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
                    lastInteractedRaycastGameObject.GetComponent<RayCastEndTurn>().Interact(isBluePlayer);

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
        right_blockChangeAnimation = false;
        photonView.RPC("Right_ChangeHandState", RpcTarget.Others, (int)HandState.POINTING, right_blockChangeAnimation);
        Right_ChangeHandState((int)HandState.POINTING, right_blockChangeAnimation);

        if (right_lastInteractedObject == null)
        { return; }

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
        Ray ray = new Ray(leftController.transform.position, leftController.transform.forward);

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
            }
        }
    }
    protected virtual void LeftHand_SelectInteractionUp()
    {
        EffectsManager.Instance.RemoveTeamEffects();
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
        Ray ray = new Ray(leftController.transform.position, leftController.transform.forward);

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
                    lastInteractedRaycastGameObject.GetComponent<RayCastEndTurn>().Interact(isBluePlayer);

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
        right_rayCastTile = clickedGameObject.transform.GetChild(1).GetComponent<RayCastTile>();

        while (playerInputs.XRIRightHandInteraction.Select.ReadValue<float>() != 0)
        {
            right_rayCastTile.UpdateRayCast();
            yield return right_waitForFixedUpdate;
        }
    }

    public virtual IEnumerator Left_DragUpdate(GameObject clickedGameObject)
    {
        left_rayCastTile = clickedGameObject.transform.GetChild(1).GetComponent<RayCastTile>();

        while (playerInputs.XRILeftHandInteraction.Select.ReadValue<float>() != 0)
        {
            left_rayCastTile.UpdateRayCast();

            yield return left_waitForFixedUpdate;
        }
    }

    public virtual void Right_HoverUpdate()
    {
        rightHandUpdate.HandsPositionUpdating(rightController.transform.position, rightController.transform.rotation);

        Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);
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
        leftHandUpdate.HandsPositionUpdating(leftController.transform.position, leftController.transform.rotation);
        Ray ray = new Ray(leftController.transform.position, leftController.transform.forward);
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
    protected void Right_ChangeHandState(int HandStateTemp, bool blockAnimation)
    {
        HandState newHandState = (HandState)HandStateTemp;
        

        if (photonView.IsMine)
        {
            if (right_handState == newHandState || blockAnimation) { return; }

            switch (newHandState)
            {
                case HandState.IDLE:
                    {
                        rightHandAnimator.SetBool("RightIndexPoint", false);
                        rightHandAnimator.SetBool("RightIndexInteract", false);
                        rightHandAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.POINTING:
                    {
                        rightHandAnimator.SetBool("RightIndexPoint", true);
                        rightHandAnimator.SetBool("RightIndexInteract", false);
                        rightHandAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.GRABBING:
                    {

                        rightHandAnimator.SetBool("RightIndexPoint", true);
                        rightHandAnimator.SetBool("RightIndexInteract", false);
                        rightHandAnimator.SetBool("RightHandGrab", true);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.INTERACT:
                    {
                        rightHandAnimator.SetBool("RightIndexPoint", true);
                        rightHandAnimator.SetBool("RightIndexInteract", true);
                        rightHandAnimator.SetBool("RightHandGrab", false);
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
                        leftHandAnimator.SetBool("LeftIndexPoint", false);
                        leftHandAnimator.SetBool("LeftIndexInteract", false);
                        leftHandAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.POINTING:
                    {
                        leftHandAnimator.SetBool("LeftIndexPoint", true);
                        leftHandAnimator.SetBool("LeftIndexInteract", false);
                        leftHandAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.GRABBING:
                    {

                        leftHandAnimator.SetBool("LeftIndexPoint", true);
                        leftHandAnimator.SetBool("LeftIndexInteract", false);
                        leftHandAnimator.SetBool("LeftHandGrab", true);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.INTERACT:
                    {
                        leftHandAnimator.SetBool("LeftIndexPoint", true);
                        leftHandAnimator.SetBool("LeftIndexInteract", true);
                        leftHandAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
            }
        }
    }

    [PunRPC]
    protected void Left_ChangeHandState(int HandStateTemp, bool blockAnimation)
    {
        HandState newHandState = (HandState)HandStateTemp;
        
        if (photonView.IsMine)
        {
            if (left_handState == newHandState || blockAnimation) { return; }

            switch (newHandState)
            {
                case HandState.IDLE:
                    {
                        leftHandAnimator.SetBool("LeftIndexPoint", false);
                        leftHandAnimator.SetBool("LeftIndexInteract", false);
                        leftHandAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.POINTING:
                    {
                        leftHandAnimator.SetBool("LeftIndexPoint", true);
                        leftHandAnimator.SetBool("LeftIndexInteract", false);
                        leftHandAnimator.SetBool("LeftHandGrab", false);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.GRABBING:
                    {

                        leftHandAnimator.SetBool("LeftIndexPoint", true);
                        leftHandAnimator.SetBool("LeftIndexInteract", false);
                        leftHandAnimator.SetBool("LeftHandGrab", true);
                        left_handState = newHandState;
                        break;
                    }
                case HandState.INTERACT:
                    {
                        leftHandAnimator.SetBool("LeftIndexPoint", true);
                        leftHandAnimator.SetBool("LeftIndexInteract", true);
                        leftHandAnimator.SetBool("LeftHandGrab", false);
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
                        rightHandAnimator.SetBool("RightIndexPoint", false);
                        rightHandAnimator.SetBool("RightIndexInteract", false);
                        rightHandAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.POINTING:
                    {
                        rightHandAnimator.SetBool("RightIndexPoint", true);
                        rightHandAnimator.SetBool("RightIndexInteract", false);
                        rightHandAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.GRABBING:
                    {

                        rightHandAnimator.SetBool("RightIndexPoint", true);
                        rightHandAnimator.SetBool("RightIndexInteract", false);
                        rightHandAnimator.SetBool("RightHandGrab", true);
                        right_handState = newHandState;
                        break;
                    }
                case HandState.INTERACT:
                    {
                        rightHandAnimator.SetBool("RightIndexPoint", true);
                        rightHandAnimator.SetBool("RightIndexInteract", true);
                        rightHandAnimator.SetBool("RightHandGrab", false);
                        right_handState = newHandState;
                        break;
                    }
            }
        }
        
    }
}
