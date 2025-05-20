using UnityEngine;

public class floating : MonoBehaviour
{
    public float amplitude = 10f;   
    public float frequency = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = startPosition + new Vector3(0, yOffset, 0);
    }
}
