using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectToken : MonoBehaviour
{
    float mouseZPos;
    Vector3 mouseOffset;

    Plane plane;
    Vector3 mousePosition2;

    int tileHovering = -1;


    // TEMP?
    Character character;



    void Start()
    {
        character = new(MovementType.Omni, TeamType.BLUE, transform.gameObject);
        plane = new Plane(Vector3.up, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
    }

    Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 mousePos = Vector3.zero;

        if (plane.Raycast(ray, out var enter))
        {
            mousePos = ray.GetPoint(enter);
            mousePos.y = 1.0f;

        }

        return mousePos;
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        if(tileHovering == -1) 
        {
            Debug.Log("TileID is -1");
            return; }


        MyEventHandler.Instance.moveToken.Invoke(tileHovering, character.GetId());
    }

    public void SetOnTileId(int tileId)
    {
        tileHovering = tileId;
    }
}
