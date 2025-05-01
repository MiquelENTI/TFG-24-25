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
        GetReplayMatchData(0);

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
        Debug.Log(replayData.deck.Count.ToString());

        foreach (int card in replayData.deck)
        {
            Debug.Log(card);
        };

        DeckManager.Instance.SetReplayDeck(replayData.deck);

        int amountToDraw = 1;

        for (int i = 0; i <= amountToDraw; i++)
        {
            DeckManager.Instance.ReplayDrawCard(true);
        }

        for (int i = 0; i <= amountToDraw; i++)
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

        //Debug.Log(replayData.inputs.Count);
        //Debug.Log(newInput);


        switch (nextChar)
        {
            case 'T':
                {

                    blueTurn = !blueTurn;

                    TurnManagerScript.Instance.UpdateTurn(blueTurn);

                    DeckManager.Instance.ReplayDrawCard(blueTurn);

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
