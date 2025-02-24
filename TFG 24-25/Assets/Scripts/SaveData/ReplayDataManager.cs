using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;

public class RankingManager : MonoBehaviour
{
    public Queue<string> actions;


    void Start()
    {
        actions = new Queue<string>();
    }

    public void SaveMatchData(int matchNum)
    {
        SaveData.Save(actions, matchNum);
    }
}
