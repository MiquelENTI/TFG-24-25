using UnityEngine;

public class RollingCredits : MonoBehaviour
{
    public float speed = 50f;
    public float endY = 1000f;

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (rectTransform.anchoredPosition.y < endY)
        {
            rectTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
        }
    }

    public void RestartCredits()
    {
        rectTransform.anchoredPosition = startPos;
    }
}
