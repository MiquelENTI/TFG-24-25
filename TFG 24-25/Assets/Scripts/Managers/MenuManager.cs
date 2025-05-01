using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int roomSelectionSceneIndex = 1;

    public int gameSceneIndex = 2;

    public int creditsSceneIndex = 3;

    public int mainMenuSceneIndex = 0;

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