using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ActionAnimation : MonoBehaviour
{
    private float lifeTime = 1.0f;
    private float timeElapsed = 0.0f;

    private float speed = 0.1f;

    private bool noDelete = false;

    private float offset = 0;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
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
