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


    void Start()
    {
        
        cardHold = transform.parent.GetComponent<CardHold>();
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 mousePos = Vector3.zero;
        if (plane.Raycast(ray, out var enter))
        {
            mousePos = ray.GetPoint(enter);
            mousePos.y = 0.75f;
        }
        return mousePos;
    }

    private void OnMouseDown()
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;



        mouseDownPos = GetMouseWorldPos();
        CellNodeManager.Instance.togglePossibleSpawnTiles.Invoke(cardHold.GetTeamType());
    }

    private void OnMouseDrag()
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        Vector3 newMousePos = GetMouseWorldPos();

        if (mouseDownPos != newMousePos)
        {
            isDragging = true;
        }

        transform.position = newMousePos;
    }

    private void OnMouseUp()
    {

        
        CellNodeManager.Instance.togglePossibleSpawnTiles.Invoke(cardHold.GetTeamType());
    }
}
