using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class TempCloseOnBoard : MonoBehaviour
{
    protected PlayerInputs playerInputs;

    [SerializeField] List<GameObject> onboardObjects = new List<GameObject>();

    protected virtual void Awake()
    {
    }

    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            ToggleOnboard(1);
        }
        if (Input.GetKeyUp(KeyCode.F2))
        { 
            ToggleOnboard(0); 
        }


    }

    protected void ToggleOnboard(int index)
    {
        onboardObjects[index].SetActive(!onboardObjects[index].activeSelf);
    }
}
