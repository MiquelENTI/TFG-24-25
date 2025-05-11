using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RT60Manager : MonoBehaviour
{
    public static RT60Manager Instance;
     private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
