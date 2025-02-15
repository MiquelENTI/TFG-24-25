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

    // Detect and Calculate Mouse World Position to Move Inside a Plane
    protected Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePos = Vector3.zero;
        if (plane.Raycast(ray, out var enter))
        {
            mousePos = ray.GetPoint(enter);
            mousePos.y = 1.5f;
        }
        return mousePos;
    }

    // When mouse is pressed down get an initial Mouse World Pos
    protected virtual void OnMouseDown()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
            return;

        mouseDownPos = GetMouseWorldPos();
    }

    // Calculate Mouse World Pos when dragging
    protected virtual void OnMouseDrag()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
            return;

        Vector3 newMousePos = GetMouseWorldPos();

        if (mouseDownPos != newMousePos)
        {
            isDragging = true;
        }

        transform.position = newMousePos;
    }
    
    // Cancel Dragging
    protected virtual void OnMouseUp()
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
}
