using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MaterialController : MonoBehaviourPun
{
    [SerializeField] Material blueMaterial;
    [SerializeField] Material redMaterial;
    private Renderer tokenRenderer;

    void Start()
    {
        tokenRenderer = GetComponent<Renderer>();
    }

    [PunRPC]
    public void SyncMaterial(string materialColor)
    {
        if (materialColor == "blue")
        {
            tokenRenderer.material = blueMaterial;
        }
        else if (materialColor == "red")
        {
            tokenRenderer.material = redMaterial;
        }
    }
}
