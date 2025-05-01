using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomSelectionManager : MonoBehaviourPunCallbacks
{
    [Tooltip("Referencia al TMP_InputField donde el usuario escribe el nombre de la sala.")]
    public TMP_InputField roomNameInputField;

    [Tooltip("Índice de la escena del juego en Build Settings.")]
    public int gameSceneIndex = 2;

    private MatchManager matchManager;

    void Start()
    {
        matchManager = FindObjectOfType<MatchManager>();
        if (matchManager == null)
        {
            Debug.LogError("No se encontró una instancia de MatchManager en la escena.");
        }
    }

    public void JoinRandomRoom()
    {
        if (matchManager != null)
        {
            PhotonNetwork.LoadLevel(gameSceneIndex);
        }
        else
        {
            Debug.LogError("MatchManager no está inicializado.");
        }
    }

    public void JoinRoomByName()
    {
        if (matchManager != null && roomNameInputField != null && !string.IsNullOrEmpty(roomNameInputField.text))
        {
            PhotonNetwork.LoadLevel(gameSceneIndex);
            matchManager.JoinOrCreateRoomByName(roomNameInputField.text);
        }
        else
        {
            Debug.LogWarning("Por favor, introduce un nombre de sala.");
        }
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}