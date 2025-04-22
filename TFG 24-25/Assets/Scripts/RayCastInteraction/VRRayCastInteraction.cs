using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class VRRayCastInteraction : BaseRayCastInteraction
{
    private WaitForFixedUpdate right_waitForFixedUpdate = new WaitForFixedUpdate();
    private WaitForFixedUpdate left_waitForFixedUpdate = new WaitForFixedUpdate();

    private DragableGameObject right_lastInteractedObject;
    private DragableGameObject left_lastInteractedObject;

    private RayCastTile right_rayCastTile;
    private RayCastTile left_rayCastTile;

    //[SerializeField] private InputActionReference right_position;
    //[SerializeField] private InputActionReference right_rotation;

    [SerializeField] private InputActionReference left_position;
    [SerializeField] private InputActionReference left_rotation;

    private GameObject rightController;
    private GameObject leftController;


    
private void Start()
    {
        #region RIGHT HAND

        // Weird Button

        playerInputs.XRIRightHandInteraction.Select.started += _ => { RightHand_SelectInteractionDown(); };
        playerInputs.XRIRightHandInteraction.Select.canceled += _ => { RightHand_SelectInteractionUp(); };

        // Trigger
        playerInputs.XRIRightHandInteraction.Activate.started += _ => { RightHand_ActivateInteractionDown(); };
        //playerInputs.XRIRightHandInteraction.Activate.canceled += _ => { RightHand_ActivateInteractionUp(); };
        playerInputs.XRIRightHand.ThumbStickClicked.started += _ => { GameObject.FindGameObjectWithTag(PhotonNetwork.IsMasterClient ? "BlueHold" : "RedHold").GetComponent<CardHold>().ReorganizeCards(); };

        #endregion

        #region LEFT HAND

        // Weird Button
        playerInputs.XRILeftHandInteraction.Select.started += _ => { LeftHand_SelectInteractionDown(); };
        playerInputs.XRILeftHandInteraction.Select.canceled += _ => { LeftHand_SelectInteractionUp(); };

        // Trigger
        playerInputs.XRILeftHandInteraction.Activate.started += _ => { LeftHand_ActivateInteractionDown(); };
        //playerInputs.XRILeftHandInteraction.Activate.canceled += _ => { LeftHand_ActivateInteractionUp(); };

        #endregion
    }

    private void Update()
    {
        if (rightController != null || leftController != null) { return; }

        if (rightController == null)
        {
            rightController = transform.GetChild(2).GetChild(0).GetChild(2).GetChild(4).gameObject;
        }
        if(leftController == null)
        {
            leftController = transform.GetChild(2).GetChild(0).GetChild(1).GetChild(4).gameObject;
        }
    }

    #region RIGHT HAND
    protected virtual void RightHand_SelectInteractionDown()
    {
        if (!photonView.IsMine)
        { return; }

        Debug.Log("ACTIVATE DOWN");
        
        Ray ray = new Ray(rightController.transform.position, -rightController.transform.forward);

        Debug.Log("RIGHT HAND POS: " + rightController.transform.position + " RIGHT HAND DIRECTION: " + -rightController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider == null)
            {
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
                if ((right_lastInteractedObject.GetIsBlue() == turnManagerScript.getIsBlue()) && hit.transform.GetComponent<PhotonView>().IsMine)
                {
                    Debug.Log("ENTERED RIGHT DRAG UPDATE");
                    StartCoroutine(Right_DragUpdate(hit.collider.gameObject));
                }
            }
        }
    }
    protected virtual void RightHand_SelectInteractionUp()
    {
        Debug.Log("ACTIVATE UP");
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

        Ray ray = new Ray(rightController.transform.position, -rightController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            { return; }

            switch (hit.collider.tag)
            {
                case "RaycastInteractable":
                    {
                        lastInteractedRaycastGameObject = hit.transform.gameObject;
                        lastInteractedRaycastGameObject.GetComponent<RayCastEndTurn>().Interact();
                        break;
                    }
            }
        }
    }


    #endregion

    #region LEFT HAND

    protected virtual void LeftHand_SelectInteractionDown()
    {
        if (!photonView.IsMine)
        { return; }

        Ray ray = new Ray(leftController.transform.position, -leftController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            { return; }

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
                if ((left_lastInteractedObject.GetIsBlue() == turnManagerScript.getIsBlue()) && hit.transform.GetComponent<PhotonView>().IsMine)
                {
                    
                    StartCoroutine(Left_DragUpdate(hit.collider.gameObject));
                }
            }
        }
    }
    protected virtual void LeftHand_SelectInteractionUp()
    {
        if (!photonView.IsMine)
        { return; }

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

        Ray ray = new Ray(leftController.transform.position, -leftController.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            { return; }

            switch (hit.collider.tag)
            {
                case "RaycastInteractable":
                    {
                        lastInteractedRaycastGameObject = hit.transform.gameObject;
                        lastInteractedRaycastGameObject.GetComponent<RayCastEndTurn>().Interact();
                        break;
                    }
            }
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
}
