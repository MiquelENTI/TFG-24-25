using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleAnimation : MonoBehaviour
{
    float speed;

    void Start()
    {
        speed = Random.Range(0.1f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //float y = Mathf.PingPong(Time.time * speed, 1);
        //transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
