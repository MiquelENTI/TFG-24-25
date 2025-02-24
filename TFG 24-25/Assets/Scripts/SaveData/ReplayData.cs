using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ReplayData
{
    public Queue<string> inputs;

    public ReplayData(Queue<string> actionsToSave)
    {
        inputs = new Queue<string>();

        inputs = actionsToSave;
    }
}
