using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    Button endTurnButton;
    void Start()
    {
        endTurnButton = GetComponent<Button>();
        endTurnButton.onClick.AddListener(() =>
        {
            TurnManagerScript.Instance.TurnManager();
        });
    }

    public void EnableDisableInteractButton(bool state)
    {
        endTurnButton.interactable = state;
    }
}
