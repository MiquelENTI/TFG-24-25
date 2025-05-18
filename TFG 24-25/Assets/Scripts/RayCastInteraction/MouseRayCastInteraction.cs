using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class MouseRayCastInteraction : BaseRayCastInteraction
{
    protected Plane plane;

    private bool isDragging = false;

    private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    void Start()
    {
        playerInputs.Mouse.LeftClick.started += _ => LeftMouseDownAction();
        playerInputs.Mouse.LeftClick.canceled += _ => LeftMouseUpAction();

        playerInputs.Mouse.RightClick.started += _ => RightClickDownAction();
        playerInputs.Mouse.RightClick.canceled += _ => RightClickUpAction();
    }

    void Update()
    {
        //if (playerInputs.Mouse.Position.ReadValue<Vector2>().y < 360.0f)
        //{
        //    cardHold.LookToPlayerCam();
        //}
        //else
        //{
        //    cardHold.DefaultCardRotation();
        //}
    }

    protected virtual void LeftMouseDownAction()
    {
        if (!photonView.IsMine)
        { return; }

        Ray ray = playerCamera.ScreenPointToRay(playerInputs.Mouse.Position.ReadValue<Vector2>());

        RaycastHit hit;

        // Debug.Log("1");

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("2");
            if (hit.collider == null)
            { return; }

            switch (hit.collider.tag)
            {
                case "TokenGameObject":
                {
                    lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                    TokenGameObject temp = (TokenGameObject)lastInteractedObject;
                    CellNodeManager.Instance.showPossibleMovements.Invoke(temp.GetCharacter().GetOnTileId());
                    break;
                }
                case "CardGameObject":
                {
                    lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                    CellNodeManager.Instance.showPossibleSpawnTiles.Invoke(lastInteractedObject.GetIsBlue() ? TeamType.BLUE : TeamType.RED);
                    Debug.Log("ENTERED CARDGO");
                    break;
                }
                case "RaycastInteractable":
                {
                        Debug.Log("CLICKED BELL");
                    lastInteractedRaycastGameObject = hit.transform.gameObject;
                    lastInteractedRaycastGameObject.GetComponent<RayCastEndTurn>().Interact(isBluePlayer);
                    break;
                }
                default:
                {
                        //Debug.Log("MouseUp Hit Collider Default: " + hit.collider.name);
                        //Debug.Log("MouseUp Hit Collider Default: " + hit.collider.GetComponent<DragableGameObject>().GetCharacterStats().name);
                    break;
                }
            }
            
            if (hit.collider.tag == "TokenGameObject" || hit.transform.tag == "CardGameObject")
            {
                if ((lastInteractedObject.GetIsBlue() == turnManagerScript.GetIsTurnBlue()) && hit.transform.GetComponent<PhotonView>().IsMine)
                {
                    //Debug.Log("4");
                    StartCoroutine(DragUpdate(hit.collider.gameObject));
                }
            }
        }
    }

    
    protected virtual void LeftMouseUpAction()
    {
        isDragging = false;

        if (lastInteractedObject == null)
        { return; }

        switch (lastInteractedObject.tag)
        {
            case "TokenGameObject":
                {
                    lastInteractedObject.CheckIsOutsideBoard();

                    lastInteractedObject = null;
                    break;
                }
            case "CardGameObject":
                {
                    lastInteractedObject.CheckIsOutsideBoard();
                    lastInteractedObject = null;
                    break;
                }
            default:
                {
                    break;
                }
        }

        EffectsManager.Instance.RemoveTeamEffects();
    }

    protected virtual void RightClickDownAction()
    {
        if (!photonView.IsMine)
        { return; }

        // Canviar Camera.Main a la camera del jugador
        Ray ray = playerCamera.ScreenPointToRay(playerInputs.Mouse.Position.ReadValue<Vector2>());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider == null)
            { return; }

            if (hit.transform.tag == "CardGameObject" || hit.transform.tag == "TokenGameObject")
            {
                lastInteractedObject = hit.transform.GetComponent<DragableGameObject>();
                lastInteractedObject.SeeTokenCard();
                lastInteractedObject.ToggleCardToDisplay(true);
                displayingCard = true;
            }
        }
    }

    protected virtual void RightClickUpAction()
    {
        if (lastInteractedObject == null)
        { return; }

        if (displayingCard)
        {
            lastInteractedObject.GetComponent<DragableGameObject>().ToggleCardToDisplay(false);
            displayingCard = false;
        }
    }

    public virtual IEnumerator DragUpdate(GameObject clickedGameObject)
    {
        Vector3 mousePos = Vector3.zero;

        isDragging = true;

        plane = lastInteractedObject.GetPlane();

        rayCastTile = clickedGameObject.transform.GetChild(1).GetComponent<RayCastTile>();

        while (playerInputs.Mouse.LeftClick.ReadValue<float>() != 0)
        {
            Ray ray = playerCamera.ScreenPointToRay(playerInputs.Mouse.Position.ReadValue<Vector2>());

            if (plane.Raycast(ray, out var enter))
            {
                mousePos = ray.GetPoint(enter);
                mousePos.y = lastInteractedObject.GetObjectDisplacement();

                lastInteractedObject.GetComponent<DragableGameObject>().UpdateObjectPosition(mousePos, lastInteractedObject.transform.rotation);

                rayCastTile.UpdateRayCast();

                yield return waitForFixedUpdate;
            }
        }
    }
}
