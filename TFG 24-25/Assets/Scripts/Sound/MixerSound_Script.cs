using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerSound_Script : MonoBehaviour
{
public static MixerSound_Script Instance;
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
    /* Start is called before the first frame update
    
    void Start()
    {
        /*if (GameObject.Find("Mixer") != this) 
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
