using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTile : MonoBehaviour
{
    [SerializeField] protected LayerMask mask;
    protected Tile previousTile;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        previousTile = GameObject.Find("Cell: 0-0").GetComponent<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayCast();
    }

    public virtual void UpdateRayCast()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3, mask)) // Layer 6 = Tile
        {
            Tile onTile = hit.transform.GetComponent<Tile>();
            if (onTile.tileId != previousTile.tileId)
            {
                previousTile.ExitRaycast();
                onTile.EnterRaycast();
                previousTile = onTile;
                transform.parent.GetComponent<DragableGameObject>().SetOnTileId(onTile.tileId);
            }
        }
    }
}
