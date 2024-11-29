using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableUIObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 startPoint;
    Vector2 endPoint;

    [SerializeField] GameObject token;
    bool tokenSpawn = false;

    Vector3 initialPosition;

    void Start()
    {
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        startPoint = eventData.pressPosition;
        initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        endPoint = eventData.position;

        transform.position = new Vector3(transform.position.x, endPoint.y, transform.position.z);

        
        if (transform.localPosition.y > -175.0f)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, -174.0f, transform.localPosition.z);
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        if (transform.localPosition.y > -175.0f)
        {
            
            Instantiate(token, new Vector3(2.16f, 0.75f, -3.95f), Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            transform.position = initialPosition;
        }
        
    }
}
