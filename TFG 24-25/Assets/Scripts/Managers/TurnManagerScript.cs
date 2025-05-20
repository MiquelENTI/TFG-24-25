using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class TurnManagerScript : Singleton<TurnManagerScript>
{
    [SerializeField] private bool IsTurnBlue = true;
    int turnCounter = 1;

    [SerializeField] public GameObject player1GameObject;
    [SerializeField] public GameObject player2GameObject;

    [SerializeField] private int[] startTurnAudioCodesForAll = { 003002001, 003002002, 003002003 };
    [SerializeField] private int[] startYourTurnAudioCodes = { 004000000 };
    [SerializeField] private int[] startEnemyTurnAudioCodes = { 003004001, 003004002 };


    //[SerializeField] private AudioClip winAudioClip;
    //[SerializeField] private AudioClip loseAudioClip;


    private bool player1Registered = false;
    private bool player2Registered = false;

    PhotonView photonView;

    int drawCardsStart = 4;

    public ScoreManager scoreManager; 
    public GameObject finalCanvas;
    public GameObject redWinGO;
    public GameObject blueWinGO;


    public bool isPCVersion = true;

    [SerializeField] private bool turnBypass;

    [SerializeField] private float turnTimeLimit = 60f;
    private float currentTurnTime;
    private bool isTurnTimerActive = false;

    [SerializeField] private int timeOutAudioCode = 004000002;

    private void Awake()
    {
        if (!TryGetComponent<PhotonView>(out photonView))
        {
            Debug.LogError("PhotonView no encontrado en el GameObject.");
        }
    }

    private void Start()
    {
        if (!photonView)
        {
            Debug.LogError("El PhotonView no est� asignado correctamente en el GameObject");
            return;
        }

        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager == null)
            {
                Debug.LogError("ScoreManager no encontrado en la escena.");
            }
        }

        if (finalCanvas == null)
        {
            finalCanvas = GameObject.Find("Final Canvas");
            if (finalCanvas == null)
            {
                Debug.LogError("Final Canvas no encontrado en la escena.");
            }
        }

        if (redWinGO != null) redWinGO.SetActive(false);
        if (blueWinGO != null) blueWinGO.SetActive(false);

        StartTurnTimer();
    }

    private void Update()
    {
        if (isTurnTimerActive)
        {
            currentTurnTime += Time.deltaTime;
            if (currentTurnTime >= turnTimeLimit)
            {
                TurnTimeOut();
            }
        }
    }

    private void StartTurnTimer()
    {
        isTurnTimerActive = true;
        currentTurnTime = 0f;
    }

    private void StopTurnTimer()
    {
        isTurnTimerActive = false;
        currentTurnTime = 0f;
    }

    private void TurnTimeOut()
    {
        Debug.LogWarning("¡Tiempo de turno agotado para el jugador " + (IsTurnBlue ? "Azul" : "Rojo") + "!");
        photonView.RPC("PlayTimeOutAudioRPC", RpcTarget.MasterClient, IsTurnBlue);
        StartTurnTimer();
    }

    [PunRPC]
    private void PlayTimeOutAudioRPC(bool isBlueTimedOut)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (isBlueTimedOut && player1GameObject != null && player1GameObject.GetComponent<PhotonView>().IsMine)
            {
                PlayAudio(004000002);
            }
            else if (!isBlueTimedOut && player2GameObject != null && player2GameObject.GetComponent<PhotonView>().IsMine)
            {
                PlayAudio(004000002);
            }
        }
    }

    private void PlayAudio(int audioCode)
    {
        if (SoundManager.Instance != null && player1GameObject != null)
        {
            SoundManager.Instance.PlayVOIndication(audioCode, player1GameObject.transform.position);
        }
        else
        {
            Debug.LogWarning("SoundManager no encontrado o no hay jugadores para reproducir el audio de tiempo de espera.");
        }
    }


    public void RegisterPlayer(GameObject playerGameObject)
    {
        if (!player1Registered)
        {
            player1GameObject = playerGameObject;
            player1Registered = true;
            // photonView.RPC("PlayStartTurnAudioForAll", RpcTarget.All);
            Debug.Log("Player 1 registrado en TurnManager: " + playerGameObject.name);
            DrawCards(drawCardsStart);
        }
        else if (!player2Registered)
        {
            player2GameObject = playerGameObject;
            player2Registered = true;
            Debug.Log("Player 2 registrado en TurnManager: " + playerGameObject.name);
        }
        else
        {
            Debug.LogWarning("Ya se han registrado Player 1 y Player 2. No se pueden registrar m�s jugadores a trav�s de este TurnManager.");
        }
    }


    public void TurnManager()
    {
        if (photonView == null)
        {
            Debug.LogError("PhotonView no est� asignado correctamente.");
            return;
        }

        IsTurnBlue = !IsTurnBlue;

        // photonView.RPC("PlayStartTurnAudioForAll", RpcTarget.All);
        photonView.RPC("PlayStartTurnAudioForColor", RpcTarget.All, IsTurnBlue);

        photonView.RPC("UpdateTurn", RpcTarget.AllBuffered, IsTurnBlue);
        photonView.RPC("DrawCards", RpcTarget.OthersBuffered, 1);
        MyEventHandler.Instance.RemoveCharacters();

    }

    [PunRPC]
    public void UpdateTurn(bool newIsTurnBlue)
    {
        SaveData.Instance.SaveNewAction("T");

        IsTurnBlue = newIsTurnBlue;
        Debug.Log("Turno cambiado. Ahora es turno " + (IsTurnBlue ? "Azul (Player 1)" : "Rojo (Player 2)"));

        CandleBehavior.Instance.ChangeCandleColor(IsTurnBlue);

        if (newIsTurnBlue)
        {
            CandleBehavior.Instance.AddCandle();
            CandleBehavior.Instance.ChangeCandleColor(IsTurnBlue);

            Debug.Log("isBlueTurn: " + newIsTurnBlue);

            turnCounter++;


            int fmodroundvalue = 0;

            if (turnCounter > 0 && turnCounter < 4)
            {
               fmodroundvalue = 0;
            }
            else if (turnCounter > 5 && turnCounter < 9)
            {
               fmodroundvalue = 1;
            }
            else if (turnCounter >= 10)
            {
               fmodroundvalue = 2;
            }
            

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("RONDA", fmodroundvalue);
            Debug.Log("ronda parameter updated to: " + fmodroundvalue);


            if (turnCounter > 10)
            {
                if (SceneManager.GetActiveScene().name != "ReplayScene")
                {
                    SaveData.Instance.Save();
                }

                
                Debug.Log("Blue score is: " + scoreManager.BlueScore);
                Debug.Log("Red score is: " + scoreManager.RedScore);

                finalCanvas.SetActive(true);


                if (scoreManager.BlueScore > scoreManager.RedScore)
                {
                    if (blueWinGO != null) blueWinGO.SetActive(true);
                    if (redWinGO != null) redWinGO.SetActive(false);

                    photonView.RPC("PlayEndGameAudio", RpcTarget.All, true);
                }
                else if (scoreManager.RedScore > scoreManager.BlueScore)
                {
                    if (redWinGO != null) redWinGO.SetActive(true);
                    if (blueWinGO != null) blueWinGO.SetActive(false);

                    photonView.RPC("PlayEndGameAudio", RpcTarget.All, false);
                }

            }
        }

        CharactersManager.Instance.ActivateEndTurnCharactersByColor(IsTurnBlue);
        CharactersManager.Instance.ActivateStartTurnCharactersByColor(IsTurnBlue);

        PlayerStats.Instance.ResetMana();
    }

    [PunRPC]
    void PlayEndGameAudio(bool blueWon)
    {
        Transform playerTransform = null;

        bool isLocalBlue = player1GameObject.GetComponent<PlayerController>().IsBlue && player1GameObject.GetComponent<PhotonView>().IsMine
                        || player2GameObject.GetComponent<PlayerController>().IsBlue && player2GameObject.GetComponent<PhotonView>().IsMine;

        if (player1GameObject.GetComponent<PhotonView>().IsMine)
        {
            playerTransform = player1GameObject.GetComponent<Transform>();
        }
        else if (player2GameObject.GetComponent<PhotonView>().IsMine)
        {
            playerTransform = player2GameObject.GetComponent<Transform>();
        }

        if ((isLocalBlue && blueWon) || (!isLocalBlue && !blueWon))
        {
            SoundManager.Instance.PlayVOIndication(004000004, playerTransform.position);
        }
        else
        {
            SoundManager.Instance.PlayVOIndication(004000005, playerTransform.position);
        }
    }

    [PunRPC]
    void PlayStartTurnAudioForAll()
    {
        if (startTurnAudioCodesForAll.Length > 0)
        {
            int randomIndex = Random.Range(0, startTurnAudioCodesForAll.Length);
            int randomAudioCode = startTurnAudioCodesForAll[randomIndex];

            if (player1GameObject != null)
            {
                SoundManager.Instance.PlayVOIndication(randomAudioCode, player1GameObject.transform.position);
            }
            if (player2GameObject != null)
            {
                SoundManager.Instance.PlayVOIndication(randomAudioCode, player2GameObject.transform.position);
            }
            else
            {
                Debug.LogWarning("Uno o ambos jugadores no han sido registrados, o sus GameObjects son nulos al intentar reproducir el audio de inicio de turno para todos.");
            }
        }
        else
        {
            Debug.LogWarning("No hay c�digos de audio definidos para PlayStartTurnAudioForAll.");
        }
    }

    [PunRPC]
    void PlayStartTurnAudioForColor(bool isBlueTurn)
    {
        Transform playerTransform = null;
        int randomAudioCode = -1;
        int[] audioCodesToUse = null;

        if (isBlueTurn)
        {
            audioCodesToUse = startYourTurnAudioCodes;
            if (player1GameObject != null && player1GameObject.GetComponent<PhotonView>().IsMine)
            {
                playerTransform = player1GameObject.transform;
            }
            else if (player2GameObject != null && player2GameObject.GetComponent<PlayerController>().IsBlue && player2GameObject.GetComponent<PhotonView>().IsMine)
            {
                playerTransform = player2GameObject.transform;
            }
        }
        else
        {
            audioCodesToUse = startEnemyTurnAudioCodes;
            if (player2GameObject != null && player2GameObject.GetComponent<PhotonView>().IsMine)
            {
                playerTransform = player2GameObject.transform;
            }
            else if (player1GameObject != null && !player1GameObject.GetComponent<PlayerController>().IsBlue && player1GameObject.GetComponent<PhotonView>().IsMine)
            {
                playerTransform = player1GameObject.transform;
            }
        }

        if (audioCodesToUse != null && audioCodesToUse.Length > 0 && playerTransform != null)
        {
            int randomIndex = Random.Range(0, audioCodesToUse.Length);
            randomAudioCode = audioCodesToUse[randomIndex];
            SoundManager.Instance.PlayVOIndication(randomAudioCode, playerTransform.position);
        }
    }


    public bool GetIsTurnBlue()
    {
        return IsTurnBlue;
    }

    public int GetIsTurnBlueInt()
    { return IsTurnBlue ? 1 : 0; }

    [PunRPC]
    public void DrawCards(int amount)
    {
        if (SceneManager.GetActiveScene().name != "ReplayScene")
        {
            for (int i = 0; i < amount; i++)
            {
                DeckManager.Instance.DrawCard(IsTurnBlue);
            }
            
        }
    }

    public void SetIsPCVersion(bool state)
    {
        isPCVersion = state;
    }

    public bool GetTurnBypass()
    {
        return turnBypass;
    }
}