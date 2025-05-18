using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int tileId;


    GameObject tileHighlight;
    [SerializeField] Material nonSelectedMaterial;
    [SerializeField] Material selectedMaterial;

    private void Start()
    {
        tileHighlight = transform.GetChild(0).gameObject;
    }

    public void EnterRaycast()
    {
        tileHighlight.GetComponent<Renderer>().material = selectedMaterial;
    }

    public void ExitRaycast()
    {
        tileHighlight.GetComponent<Renderer>().material = nonSelectedMaterial;
    }

    public void SetTileId(int id)
    { tileId = id; }
    public int GetTileId()
    { return tileId; }
}
