using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class GrassHopperMovement : MonoBehaviour
{
    [SerializeField] Vector3 centerPoint;
    [SerializeField] float range;

    [SerializeField] Vector3 destination;
    [SerializeField] Vector3 origin;

    [SerializeField] float duration = 1f;
    float currentDuration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        centerPoint = GameObject.Find("Board").transform.position;
        centerPoint = gameObject.transform.position = new Vector3(centerPoint.x, -0.48f + transform.localScale.y/2, centerPoint.z);
        origin = centerPoint;
        //Implementació del so

        SoundManager.Instance.PlayGrasshopperMovement(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        DetectReachedDestination();

        MoveToDirection();
    }

    void GenerateRandomPosition()
    {
        float xCoord = Random.Range(-range, range);
        float zCoord = Random.Range(-range, range);

        destination = new Vector3(xCoord, centerPoint.y, zCoord);

        currentDuration = 0;
    }

    void MoveToDirection()
    {
        currentDuration = Mathf.Min(currentDuration + Time.deltaTime, duration);

        float ratio = currentDuration / duration;

        transform.position = Vector3.Lerp(origin, destination, ratio);
    }

    void DetectReachedDestination()
    {
        if (transform.position == destination)
        {
            origin = destination;
            GenerateRandomPosition();
            
        }
    }

}
