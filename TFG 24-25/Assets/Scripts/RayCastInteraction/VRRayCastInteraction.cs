using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public enum HandState { IDLE, POINTING, GRABBING, INTERACT }

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
        #region RIGHT HAND

        // Weird Button

        playerInputs.XRIRightHandInteraction.Select.started += _ => { RightHand_SelectInteractionDown(); };
        playerInputs.XRIRightHandInteraction.Select.canceled += _ => { RightHand_SelectInteractionUp(); };

        // Trigger
        playerInputs.XRIRightHandInteraction.Activate.started += _ => { RightHand_ActivateInteractionDown(); };
        playerInputs.XRIRightHandInteraction.Activate.canceled += _ => { RightHand_ActivateInteractionUp(); };
        playerInputs.XRIRightHand.ThumbStickClicked.started += _ => { GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold").GetComponent<CardHold>().ReorganizeCards(); };

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
            rightHandUpdate = rightController.GetComponent<UpdateHandVR>();
        }
        if(leftController == null)
        {
            leftController = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(4).gameObject;
            leftHandUpdate = leftController.GetComponent<UpdateHandVR>();
        }
    }

    #region RIGHT HAND
    protected virtual void RightHand_SelectInteractionDown()
    {
        if (!photonView.IsMine)
        { return; }

        Debug.Log("ACTIVATE DOWN");
        
        Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);

        Debug.Log("RIGHT HAND POS: " + rightController.transform.position + " RIGHT HAND DIRECTION: " + -rightController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider == null)
            {
                Right_ChangeHandState(HandState.IDLE);
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
                        CellNodeManager.Instance.InvokeShowPossibleSpawnTiles(right_lastInteractedObject.GetIsBlue());
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
                    Right_ChangeHandState(HandState.GRABBING);
                    right_blockChangeAnimation = true;
                }
            }
        }
    }
    protected virtual void RightHand_SelectInteractionUp()
    {
        Debug.Log("ACTIVATE UP");
        right_blockChangeAnimation = false;
        Right_ChangeHandState(HandState.POINTING);

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
        if (!photonView.IsMine)
        { return; }

        Ray ray = new Ray(rightController.transform.position, rightController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            {
                Right_ChangeHandState(HandState.POINTING);
                return; 
            }

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                case "CardGameObject":
                {
                    right_lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                    right_lastInteractedObject.SeeTokenCard();
                    right_lastInteractedObject.ToggleCardToDisplay(true);
                    right_lastInteractedObject.RotateCardToDisplayVR();
                    displayingCard = true;

                    Right_ChangeHandState(HandState.INTERACT);
                    right_blockChangeAnimation = true;
                    break;
                }
                case "RaycastInteractable":
                {
                    lastInteractedRaycastGameObject = hit.transform.gameObject;
                    lastInteractedRaycastGameObject.GetComponent<RayCastEndTurn>().Interact(isBluePlayer);

                    Right_ChangeHandState(HandState.INTERACT);
                    right_blockChangeAnimation = true;
                    break;
                }
            }
        }
    }

    protected virtual void RightHand_ActivateInteractionUp()
    {
        right_blockChangeAnimation = false;
        Right_ChangeHandState(HandState.POINTING);

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
        if (!photonView.IsMine)
        { return; }

        Ray ray = new Ray(leftController.transform.position, leftController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            {
                Left_ChangeHandState(HandState.IDLE);
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
                    CellNodeManager.Instance.InvokeShowPossibleSpawnTiles(left_lastInteractedObject.GetIsBlue());
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
                    Left_ChangeHandState(HandState.GRABBING);
                    left_blockChangeAnimation = true;
                }
            }
        }
    }
    protected virtual void LeftHand_SelectInteractionUp()
    {
        left_blockChangeAnimation = false;
        Left_ChangeHandState(HandState.POINTING);

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
        if (!photonView.IsMine)
        { return; }

        Ray ray = new Ray(leftController.transform.position, leftController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            {
                Left_ChangeHandState(HandState.POINTING);
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

                        Left_ChangeHandState(HandState.INTERACT);
                        left_blockChangeAnimation = true;
                        break;
                    }
                case "RaycastInteractable":
                {
                    lastInteractedRaycastGameObject = hit.transform.gameObject;
                    lastInteractedRaycastGameObject.GetComponent<RayCastEndTurn>().Interact(isBluePlayer);

                    Left_ChangeHandState(HandState.INTERACT);
                    left_blockChangeAnimation = true;
                    break;
                }
            }
        }
    }

    protected virtual void LeftHand_ActivateInteractionUp()
    {
        left_blockChangeAnimation = false;
        Left_ChangeHandState(HandState.POINTING);

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
                Right_ChangeHandState(HandState.IDLE);
                return;
            }

            //Debug.Log(hit.collider.name);

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                case "CardGameObject":
                case "RaycastInteractable":
                {
                        Right_ChangeHandState(HandState.POINTING);
                    break;
                }
                default:
                {
                        Right_ChangeHandState(HandState.IDLE);
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
                Left_ChangeHandState(HandState.IDLE);
                return;
            }

            //Debug.Log(hit.collider.name);

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                case "CardGameObject":
                case "RaycastInteractable":
                {
                    Left_ChangeHandState(HandState.POINTING);
                    break;
                }
                default:
                {
                    Left_ChangeHandState(HandState.IDLE);
                    break;
                }
            }
        }
    }

    protected void Right_ChangeHandState(HandState newHandState)
    {
        if (right_handState == newHandState || right_blockChangeAnimation) { return; }

        Debug.Log("RIGHT CHANGING TO: " + newHandState.ToString());

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

    protected void Left_ChangeHandState(HandState newHandState)
    {
        if (left_handState == newHandState || left_blockChangeAnimation) { return; }

        Debug.Log("LEFT CHANGING TO: " + newHandState.ToString());

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
