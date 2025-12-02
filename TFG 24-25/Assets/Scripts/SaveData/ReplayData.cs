using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReplayData
{
    public Queue<int> deck;
    public Queue<string> inputs;

    public ReplayData()
    {
        deck = new Queue<int>();
        inputs = new Queue<string>();
    }
}
