using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ActionAnimation : MonoBehaviour
{
    float lifeTime = 1.0f;
    float timeElapsed = 0.0f;

    float speed = 0.1f;

    bool noDelete = false;

    float offset = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= lifeTime)
        {
            if (noDelete) { return; }
            PhotonNetwork.Destroy(gameObject);
            Destroy(gameObject);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y + offset + speed * Time.deltaTime, transform.position.z);
    }

    public void SetOffsetAndDuration(float offset, float duration)
    {
        lifeTime = duration;
        this.offset = offset;
    }
}
