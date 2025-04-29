using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Windows;
using System;

public class ReplayManager: MonoBehaviour
{
    SpawnCardController controller;

    bool blueTurn;

    public ReplayData replayData;
    
    void Start()
    {
        replayData = new ReplayData();
        GetReplayMatchData(1);

        blueTurn = true;

        controller = GameObject.Find("SpawnManager").GetComponent<SpawnCardController>();
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKeyUp(KeyCode.S))
        {
            StartReplay();
        }

        if (UnityEngine.Input.GetKeyUp(KeyCode.N))
        {
            NextMove();
        }
    }

    private void GetReplayMatchData(int matchNum)
    {
        replayData = SaveData.Instance.LoadReplay(matchNum);
    }

    private void StartReplay()
    {

        Debug.Log("DECK:");
        Debug.Log(replayData.inputs.Count.ToString());

        foreach (string input in replayData.inputs)
        {
            Debug.Log(input);
        };

        DeckManager.Instance.SetReplayDeck(replayData.deck);

        for (int i = 0; i <= 4; i++)
        {
            DeckManager.Instance.ReplayDrawCard(true);
        }

        for (int i = 0; i <= 4; i++)
        {
            DeckManager.Instance.ReplayDrawCard(false);
        }
    }

    private void NextMove()
    {
        if (replayData.inputs.Count <= 0)
            return;

        string newInput = replayData.inputs.Dequeue();

        char nextChar = newInput.ToCharArray()[0];

        newInput = newInput.Substring(1);

        Debug.Log(replayData.inputs.Count);
        Debug.Log(newInput);


        switch (nextChar)
        {
            case 'T':
                {
                    nextChar = newInput.ToCharArray()[0];

                    bool turn = nextChar == 1;

                    TurnManagerScript.Instance.UpdateTurn(turn);

                    DeckManager.Instance.ReplayDrawCard(turn);

                    blueTurn = !blueTurn;

                    break;
                }
            case 'M':
                {
                    string[] movementIds = newInput.Split('/');

                    Debug.Log(movementIds[0] + "cometes" + movementIds[1]);

                    MyEventHandler.Instance.InvokeMoveToken(Int32.Parse(movementIds[0]), Int32.Parse(movementIds[1]));
                    break;
                }
            case 'S':
                {
                    string[] spawnIds = newInput.Split('/');

                    Debug.Log(spawnIds[0] + "cometes" + spawnIds[1]);

                    controller.InstantiateCharacterTokenReplay(Int32.Parse(spawnIds[1]), Int32.Parse(spawnIds[0]), false, blueTurn);
                    break;
                }
        }
    }
}
