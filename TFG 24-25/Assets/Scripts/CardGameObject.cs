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

    public GameObject spawnTileCollider;

    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        base.Start();

        plane = new Plane(Vector3.up, Vector3.up);
        cardHold = transform.parent.parent.GetComponent<CardHold>();
        spawnTileCollider = transform.parent.GetChild(1).gameObject;
    }


    protected override void LeftMouseDownAction()
    {
        base.LeftMouseDownAction();


        // When Dragging, the GameObject (Visible Card) Updates its Position and its Sibling, the Collider that Detects on Which Tile is it Hovering On.
        GetMouseWorldPos("CardGameObject");

        if (isDragging)
        {
            CellNodeManager.Instance.showPossibleSpawnTiles.Invoke(cardHold.GetTeamType());
        }
    }

    protected override void LeftMouseUpAction()
    {
        base.LeftMouseUpAction();

        if (isOutsideBoard)
        {
            // Returns to The Hand
            cardHold.ReorganizeCards();
        }
        else
        {
            // Spawn Token in tileHovering
        }
        
        CellNodeManager.Instance.hidePossibleSpawnTiles.Invoke(cardHold.GetTeamType());
    }

    protected override IEnumerator DragUpdate(GameObject clickedGameObject)
    {
        Vector3 mousePos = Vector3.zero;

        isDragging = true;

        while (playerInputs.Gameplay.MouseLeftClick.ReadValue<float>() != 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(playerInputs.Gameplay.MousePosition.ReadValue<Vector2>());

            if (plane.Raycast(ray, out var enter))
            {
                mousePos = ray.GetPoint(enter);
                mousePos.y = 1.5f;

                clickedGameObject.transform.position = mousePos;
                clickedGameObject.GetComponent<CardGameObject>().GetSpawnTileCollider().transform.position = mousePos;

                yield return waitForFixedUpdate;
            }
        }
    }

    public GameObject GetSpawnTileCollider()
    { return spawnTileCollider; }
}
