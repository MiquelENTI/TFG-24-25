using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private int mainMenuSceneIndex = 0;

    private int gameSceneIndex = 1;

    private int creditsSceneIndex = 2;

    // private int roomSelectionSceneIndex = 1;

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(creditsSceneIndex);
    }

    public void ExitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}