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

    protected virtual void OnMouseDown()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
            return;

        mouseDownPos = GetMouseWorldPos();
    }

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

    protected virtual void OnMouseUp()
    {
        isDragging = false;
    }

    public void SetOnTileId(int tileId)
    {
        tileHovering = tileId;
    }

    public void SetOutsideBoard(bool isOutside)
    {
        Debug.Log("Activated");
        isOutsideBoard = isOutside;
    }
}
