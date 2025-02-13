using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DragableGameObject : MonoBehaviour
{
    Vector3 mouseDownPos;
    bool isDragging = false;
    Plane plane = new Plane(Vector3.up, Vector3.up);

    CardHold cardHold;

    GameObject spawnTileCollider;

    public bool isInside = false;

    int tileHovering;
    bool isOutsideBoard = true;


    void Start()
    {
        cardHold = transform.parent.parent.GetComponent<CardHold>();

        spawnTileCollider = transform.parent.GetChild(1).gameObject;
    }

    Vector3 GetMouseWorldPos()
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

    private void OnMouseDown()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
            return;



        mouseDownPos = GetMouseWorldPos();
        CellNodeManager.Instance.togglePossibleSpawnTiles.Invoke(cardHold.GetTeamType());
    }

    private void OnMouseDrag()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
            return;

        Vector3 newMousePos = GetMouseWorldPos();

        if (mouseDownPos != newMousePos)
        {
            isDragging = true;
        }

        transform.position = newMousePos;
        spawnTileCollider.transform.position = transform.position;
    }

    private void OnMouseUp()
    {
        if (isOutsideBoard)
        {
            cardHold.ReorganizeCards();
        }
        else
        {
            // Spawn Token in tileHovering
        }
        
        CellNodeManager.Instance.togglePossibleSpawnTiles.Invoke(cardHold.GetTeamType());
    }

    public void SetOnTileId(int tileId)
    {
        tileHovering = tileId;
    }

    public void SetOutsideBoard(bool isOutside)
    {
        isOutsideBoard = isOutside;
    }
}
