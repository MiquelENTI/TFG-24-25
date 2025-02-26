using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;

public class DragableGameObject : MonoBehaviour
{
    protected Plane plane;
    protected Vector3 mouseDownPos;
    protected bool isDragging = false;
    protected int tileHovering = -1;
    
    protected bool isOutsideBoard = true;

    protected PlayerInputs playerInputs;
    protected WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected GameObject gameObjectSelected; // Warrada, mira de ferho millor

    protected virtual void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    protected virtual void Start()
    {
        playerInputs.Gameplay.MouseLeftClick.started += _ => LeftMouseDownAction();
        playerInputs.Gameplay.MouseLeftClick.canceled += _ => LeftMouseUpAction();
    }

    // Detect and Calculate Mouse World Position to Move Inside a Plane
    protected virtual void GetMouseWorldPos(string colliderTag)
    {
        Ray ray = Camera.main.ScreenPointToRay(playerInputs.Gameplay.MousePosition.ReadValue<Vector2>());
        
        RaycastHit hit;

        Debug.Log("1");

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("2");
            if (hit.collider != null && hit.collider.tag == colliderTag)
            {
                Debug.Log("3");
                StartCoroutine(DragUpdate(hit.collider.gameObject));
                gameObjectSelected = hit.collider.gameObject;
            }
        }
    }

    // When mouse is pressed down get an initial Mouse World Pos
    protected virtual void LeftMouseDownAction()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
            return;
    }

    // Cancel Dragging
    protected virtual void LeftMouseUpAction()
    {
        isDragging = false;
    }

    // Assign On Which Board Tile is the GameObject hovering
    public void SetOnTileId(int tileId)
    {
        tileHovering = tileId;
    }

    // Bool that Indicates if the GameObject is inside the Board Space
    public void SetOutsideBoard(bool isOutside)
    {
        isOutsideBoard = isOutside;
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    protected virtual IEnumerator DragUpdate(GameObject clickedGameObject)
    {
        Vector3 mousePos = Vector3.zero;

        isDragging = true;

        while (playerInputs.Gameplay.MouseLeftClick.ReadValue<float>() != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(playerInputs.Gameplay.MousePosition.ReadValue<Vector2>());

            if (plane.Raycast(ray, out var enter))
            {
                mousePos = ray.GetPoint(enter);
                mousePos.y = 0.5f;
                clickedGameObject.transform.position = mousePos;
                yield return waitForFixedUpdate;
            }
        }
    }
}
