using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSpawnTile : RaycastTile
{
    protected override void Start()
    {
        base.Start();
    }

    public override void UpdateRaycast()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5, mask)) // Layer 6 = Tile
        {
            Tile onTile = hit.transform.GetComponent<Tile>();
            
            if (onTile.GetTileId() != previousTile.GetTileId())
            {
                previousTile.ExitRaycast();
                onTile.EnterRaycast();
                previousTile = onTile;
                transform.parent.GetComponent<DragableGameObject>().SetOnTileId(onTile.GetTileId());
            }
            else if (onTile.GetTileId() == previousTile.GetTileId())
            {
                onTile.EnterRaycast();
                transform.parent.GetComponent<DragableGameObject>().SetOnTileId(onTile.GetTileId());
            }
        }
        else
        {
            previousTile.ExitRaycast();
        }
    }
}
