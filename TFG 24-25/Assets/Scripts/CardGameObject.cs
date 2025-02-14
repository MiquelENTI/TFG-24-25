using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CardGameObject : DragableGameObject
{
    //Plane plane = new Plane(Vector3.up, Vector3.up);

    CardHold cardHold;

    GameObject spawnTileCollider;


    void Start()
    {
        plane = new Plane(Vector3.up, Vector3.up);
        cardHold = transform.parent.parent.GetComponent<CardHold>();
        spawnTileCollider = transform.parent.GetChild(1).gameObject;
    }

    

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
        CellNodeManager.Instance.togglePossibleSpawnTiles.Invoke(cardHold.GetTeamType());
    }

    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
        spawnTileCollider.transform.position = transform.position;
    }

    protected override void OnMouseUp()
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
}
