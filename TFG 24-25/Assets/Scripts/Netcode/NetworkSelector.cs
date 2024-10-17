using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkSelector : MonoBehaviour
{
    [SerializeField] private Button startServerButton;
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    [SerializeField] TestRelay relayObject;

    [SerializeField] TMP_InputField joinCodeInputField;
    [SerializeField] TMP_Text relayJoinCode;

    private void Awake()
    {
        Cursor.visible = true;
    }

    void Update()
    {
        //playersConnectedText.text = $"Players Connected: {PlayersManager.Instance.PlayersConnected}";

        if (relayObject.relayCode != null) { relayJoinCode.text = relayObject.relayCode; }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleSelectorButtons();
        }
    }

    private void Start()
    {
        InitButtonOnClicks();

        joinCodeInputField.onEndEdit.AddListener((string joinCode) =>
        {
            relayObject.JoinRelay(joinCode);
            joinCodeInputField.gameObject.SetActive(false);
        });
    }

    void ToggleSelectorButtons()
    {
        //startServerButton.gameObject.SetActive(!startServerButton.gameObject.activeSelf);

        startHostButton.gameObject.SetActive(!startHostButton.gameObject.activeSelf);
        startClientButton.gameObject.SetActive(!startClientButton.gameObject.activeSelf);
        
    }

    void InitButtonOnClicks()
    {
        /*
        startServerButton.onClick.AddListener(() => {

            if (NetworkManager.Singleton.StartServer())
            {
                Logger.Instance.LogInfo("Server started");
            }
            else
            {
                Logger.Instance.LogInfo("Server could not be started");
            }

            ToggleSelectorButtons();
        });
        */

        startHostButton.onClick.AddListener(() => {
            relayObject.CreateRelay();

            ToggleSelectorButtons();

            relayJoinCode.gameObject.SetActive(true);
        });

        startClientButton.onClick.AddListener(() => {
            ToggleSelectorButtons();

            joinCodeInputField.gameObject.SetActive(true);
        });
    }
}
