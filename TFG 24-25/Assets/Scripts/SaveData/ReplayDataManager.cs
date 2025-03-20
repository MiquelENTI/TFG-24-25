using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;
using System;

public class RankingManager : MonoBehaviour
{
    public ReplayData replayData;
    
    void Start()
    {
        replayData = new ReplayData();
        GetReplayMatchData(0);

        for (int i = 0; i <= replayData.inputs.Count; i++)
        {
            Debug.Log(replayData.deck.Dequeue().ToString());
        }
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyUp(KeyCode.F1))
        {
            NextMove();
        }
    }

    public void GetReplayMatchData(int matchNum)
    {
        replayData = SaveData.Instance.LoadReplay(matchNum);

        DeckManager.Instance.SetReplayDeck(replayData.deck);
    }

    public void NextMove()
    {
        string newInput = replayData.inputs.Dequeue();

        char nextChar = newInput.ToCharArray()[0];

        switch (nextChar)
        {
            case 'T':
                nextChar = newInput.ToCharArray()[1];
                if(nextChar == 1)
                    TurnManagerScript.Instance.UpdateTurn(true);
                else
                    TurnManagerScript.Instance.UpdateTurn(false);
                break;
            case 'M':
                string[] movementIds = newInput.Split('/');
                MyEventHandler.Instance.RPC_MoveToken(Int32.Parse(movementIds[0]), Int32.Parse(movementIds[1]));
                break;
        }
    }
}
