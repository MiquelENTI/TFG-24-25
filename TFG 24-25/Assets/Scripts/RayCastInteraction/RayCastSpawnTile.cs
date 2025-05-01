using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastSpawnTile : RayCastTile
{
    protected override void Start()
    {
        base.Start();
    }

    public override void UpdateRayCast()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5, mask)) // Layer 6 = Tile
        {
            Tile onTile = hit.transform.GetComponent<Tile>();
            
            if (onTile.tileId != previousTile.tileId)
            {
                previousTile.ExitRaycast();
                onTile.EnterRaycast();
                previousTile = onTile;
                transform.parent.GetComponent<DragableGameObject>().SetOnTileId(onTile.tileId);
            }
            else if (onTile.tileId == previousTile.tileId)
            {
                onTile.EnterRaycast();
                transform.parent.GetComponent<DragableGameObject>().SetOnTileId(onTile.tileId);
            }
        }
        else
        {
            previousTile.ExitRaycast();
        }
    }
}
