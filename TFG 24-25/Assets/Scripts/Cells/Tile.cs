using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int tileId;


    GameObject tileHighlight;
    [SerializeField] Material nonSelectedMaterial;
    [SerializeField] Material selectedMaterial;

    private void Start()
    {
        tileHighlight = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Token has an emptyObject with collision and its tag is TileCollider
        if (other.gameObject.tag == "TileCollider")
        {
            other.transform.parent.GetChild(0).GetComponent<TokenGameObject>().SetOnTileId(tileId);
            tileHighlight.GetComponent<Renderer>().material = selectedMaterial;
        }
        // Cards on Hand have an emptyObject with collision and its tag is SpawnTileCollider
        else if (other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetChild(0).rotation = Quaternion.Euler(90, 0, 0);
            other.transform.parent.GetChild(0).GetComponent<CardGameObject>().SetOnTileId(tileId);
            tileHighlight.GetComponent<Renderer>().material = selectedMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TileCollider") 
        {
            tileHighlight.GetComponent<Renderer>().material = nonSelectedMaterial;
        }
        else if (other.gameObject.tag == "SpawnTileCollider")
        {
            other.transform.parent.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
            tileHighlight.GetComponent<Renderer>().material = nonSelectedMaterial;
        }
    }
}
