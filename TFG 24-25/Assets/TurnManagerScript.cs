using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class TurnManagerScript : Singleton<TurnManagerScript>
{
    [SerializeField] private bool IsBlue = true;
    int turnCounter = 1;

    [SerializeField] public GameObject player1GameObject;
    [SerializeField] public GameObject player2GameObject;

    [SerializeField] private int[] startTurnAudioCodesForAll = { 003002001, 003002002, 003002003 };
    [SerializeField] private int[] startYourTurnAudioCodes = { 003003001, 003003002 };
    [SerializeField] private int[] startEnemyTurnAudioCodes = { 003004001, 003004002 };


    //[SerializeField] private AudioClip winAudioClip;
    //[SerializeField] private AudioClip loseAudioClip;


    private bool player1Registered = false;
    private bool player2Registered = false;

    PhotonView photonView;
    public TMP_Text turnCounterElement;

    int drawCardsStart = 2;

    int blueMana;
    int redMana;


    public ScoreManager scoreManager; 
    public GameObject finalCanvas;
    public TMP_Text winnerText;

    public bool isPCVersion = true;

    [SerializeField] private bool turnBypass;

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
            Debug.LogError("El PhotonView no estï¿½ asignado correctamente en el GameObject");
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

        if (winnerText == null && finalCanvas != null)
        {
            winnerText = finalCanvas.transform.Find("Winner").GetComponent<TMP_Text>();
            if (winnerText == null)
            {
                Debug.LogError("Winner Text no encontrado como hijo de Final Canvas.");
            }
        }
    }

    public void RegisterPlayer(GameObject playerGameObject)
    {
        if (!player1Registered)
        {
            player1GameObject = playerGameObject;
            player1Registered = true;
            Debug.Log("Player 1 registrado en TurnManager: " + playerGameObject.name);
            DrawCards(5);
        }
        else if (!player2Registered)
        {
            player2GameObject = playerGameObject;
            player2Registered = true;
            Debug.Log("Player 2 registrado en TurnManager: " + playerGameObject.name);
        }
        else
        {
            Debug.LogWarning("Ya se han registrado Player 1 y Player 2. No se pueden registrar mï¿½s jugadores a travï¿½s de este TurnManager.");
        }
    }


    public void TurnManager()
    {
        if (photonView == null)
        {
            Debug.LogError("PhotonView no estï¿½ asignado correctamente.");
            return;
        }

        IsBlue = !IsBlue;

        // photonView.RPC("PlayStartTurnAudioForAll", RpcTarget.All);
        photonView.RPC("PlayStartTurnAudioForColor", RpcTarget.All, IsBlue);

        photonView.RPC("UpdateTurn", RpcTarget.AllBuffered, IsBlue);
        photonView.RPC("DrawCards", RpcTarget.OthersBuffered, 1);
    }

    [PunRPC]
    public void UpdateTurn(bool newIsBlue)
    {
        SaveData.Instance.SaveNewAction("T");

        IsBlue = newIsBlue;
        Debug.Log("Turno cambiado. Ahora es turno " + (IsBlue ? "Azul (Player 1)" : "Rojo (Player 2)"));

        if (newIsBlue)
        {
            Debug.Log("isBlueTurn: " + newIsBlue);
            //PlayerStats.Instance.IncreaseTotalMana(1);
            turnCounter++;
            turnCounterElement.text = "Turn Number: " + turnCounter;

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
                    winnerText.text = "Blue Wins!";
                    winnerText.color = Color.blue;
                    photonView.RPC("PlayEndGameAudio", RpcTarget.All, true);
                }
                else if (scoreManager.RedScore > scoreManager.BlueScore)
                {
                    winnerText.text = "Red Wins!";
                    winnerText.color = Color.red;
                    photonView.RPC("PlayEndGameAudio", RpcTarget.All, false);
                }
            }
        }

        CharactersManager.Instance.ActivateEndTurnCharactersByColor(IsBlue);
        CharactersManager.Instance.ActivateStartTurnCharactersByColor(IsBlue);

        Debug.Log("TO RESET MANA");

        PlayerStats.Instance.ResetMana();

        UpdatePlayerCanvas();
    }

    [PunRPC]
    void PlayEndGameAudio(bool blueWon)
    {
        Transform playerTransform = null;

        bool isLocalBlue = player1GameObject.GetComponent<PlayerController>().IsBlue && player1GameObject.GetComponent<PhotonView>().IsMine;
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
            SoundManager.Instance.PlayVO(003001001, playerTransform.position);
        }
        else
        {
            SoundManager.Instance.PlayVO(003010001, playerTransform.position);
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
                SoundManager.Instance.PlayVO(randomAudioCode, player1GameObject.transform.position);
            }
            if (player2GameObject != null)
            {
                SoundManager.Instance.PlayVO(randomAudioCode, player2GameObject.transform.position);
            }
            else
            {
                Debug.LogWarning("Uno o ambos jugadores no han sido registrados, o sus GameObjects son nulos al intentar reproducir el audio de inicio de turno para todos.");
            }
        }
        else
        {
            Debug.LogWarning("No hay códigos de audio definidos para PlayStartTurnAudioForAll.");
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
            SoundManager.Instance.PlayVO(randomAudioCode, playerTransform.position);
        }
    }

    private void UpdatePlayerCanvas()
    {
        if (player1GameObject != null && player2GameObject != null)
        {
            Canvas canvasPlayer1 = player1GameObject.GetComponentInChildren<Canvas>();
            Canvas canvasPlayer2 = player2GameObject.GetComponentInChildren<Canvas>();

            if (canvasPlayer1 != null && canvasPlayer2 != null)
            {
                if (IsBlue)
                {
                    canvasPlayer1.enabled = true;
                    canvasPlayer2.enabled = false;
                    Debug.Log("Turno de Player 1. Canvas de Player 1 activado, Canvas de Player 2 desactivado.");
                }
                else
                {
                    canvasPlayer2.enabled = true;
                    canvasPlayer1.enabled = false;
                    Debug.Log("Turno de Player 2. Canvas de Player 2 activado, Canvas de Player 1 desactivado.");
                }
            }
            else
            {
                Debug.LogWarning("Uno o ambos jugadores no tienen un Canvas en sus GameObjects hijos.");
            }
        }
        else
        {
            Debug.LogWarning("Player 1 o Player 2 no han sido registrados todavï¿½a en TurnManager.");
        }
    }


    public bool getIsBlue()
    {
        return IsBlue;
    }

    public int GetIsBlueInt()
    { return IsBlue ? 1 : 0; }

    [PunRPC]
    public void DrawCards(int amount)
    {
        if (SceneManager.GetActiveScene().name != "ReplayScene")
        {
            DeckManager.Instance.DrawCard(IsBlue);
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