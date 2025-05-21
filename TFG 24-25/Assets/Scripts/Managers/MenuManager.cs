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

        PlayGameAudio();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);

        GoToMenuAudio();
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(creditsSceneIndex);

        GoToCreditsAudio();
    }

    public void ExitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();

        ExitGameAudio();
    }

    public void OptionGame()
    {

        OptionGameAudio();
    }

    //Implementació del so 
    public void PlayGameAudio()
    {
        SoundManager.Instance.PlayUibotton(005000003, transform.position);
    }

    //Implementació del so 
    public void GoToMenuAudio()
    {
        SoundManager.Instance.PlayUibotton(005000003, transform.position);
    }

    //Implementació del so 
    public void GoToCreditsAudio()
    {
        SoundManager.Instance.PlayUibotton(005000001, transform.position);
    }

    //Implementació del so 
    public void ExitGameAudio()
    {
        SoundManager.Instance.PlayUibotton(005000002, transform.position);
    }
    
    //Implementació del so 
    public void OptionGameAudio()
    {
        SoundManager.Instance.PlayUibotton(005000003, transform.position);
    }
}