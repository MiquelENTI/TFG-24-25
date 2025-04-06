using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastSpawnTile : RayCastTile
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRayCast();
    }

    public override void UpdateRayCast()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5, mask)) // Layer 6 = Tile
        {
            Tile onTile = hit.transform.GetComponent<Tile>();

            //Debug.Log(onTile.gameObject.name + " : ");
            //Debug.Log(onTile.gameObject.name + " : " + previousTile.gameObject.name);

            if (onTile.tileId != previousTile.tileId)
            {
                previousTile.ExitRaycast();
                onTile.EnterRaycast();
                previousTile = onTile;
                transform.parent.GetComponent<DragableGameObject>().SetOnTileId(onTile.tileId);
                //transform.parent.localRotation = Quaternion.Euler(90, 0, 0);
                Debug.Log("ON TILE: " + onTile.tileId);
            }
        }
    }
}
