using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewOnboarding : MonoBehaviour
{
    int onboardingCounter = 0;
    [SerializeField] List<GameObject> images = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            if (onboardingCounter >= 5)
            {
                foreach (GameObject image in images)
                {
                    image.SetActive(false);
                }                    
                return;
            }
            onboardingCounter++;
            images[onboardingCounter].SetActive(true);
        }
    }
}
