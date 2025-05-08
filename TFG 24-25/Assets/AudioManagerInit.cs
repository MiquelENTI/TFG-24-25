using UnityEngine;

public class AudioManagerInit : MonoBehaviour
{
    public GameObject audioManagerPrefab;

    void Awake()
    {
        if (AudioManager.Instance == null)
        {
            Instantiate(audioManagerPrefab);
        }
    }
}
