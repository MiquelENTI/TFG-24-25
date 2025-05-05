using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class CandleBehavior : Singleton<CandleBehavior>
{
    [SerializeField] List<GameObject> candles = new List<GameObject>();
    int numOfCandlesOn = 1;


    public void ChangeCandleColor(bool isBlueFire)
    {
        if (numOfCandlesOn > 10) { return; }
        for (int i = 0; i < numOfCandlesOn; i++)
        {
            candles[i].transform.GetChild(0).gameObject.SetActive(isBlueFire);
            candles[i].transform.GetChild(1).gameObject.SetActive(!isBlueFire);
        }
    }

    public void AddCandle()
    {
        numOfCandlesOn++;
    }
}
