using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("FMOD Music Events")]
    public EventReference menuMusicEvent;
    public EventReference gameplayMusicEvent;

    private EventInstance menuMusicInstance;
    private EventInstance gameplayMusicInstance;

    private Dictionary<int, string> sfxDictionary;
    private Dictionary<int, string> musicDictionary;
    private Dictionary<int, string> ambientDictionary;
    private Dictionary<int, string> voDictionary;
    private Dictionary<int, string> uibuttonDictionary;

    private GameObject board;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionaries();
            board = GameObject.Find("Board");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Escena cargada: " + scene.name);
        StopAllMusic();

        if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        else if (scene.name == "Unified Scene")
        {
            PlayGameplayMusic();
        }
    }
    
    private void InitializeDictionaries()
    {
        musicDictionary = new()
        {
            { 002000001, "event:/MUSIC/GAMEPLAY/Taverna" },
            { 002000002, "event:/MUSIC/MENU/Menu" },
        };
    }

    // ---- MÚSICA PERSISTENTE ----
    public void PlayMenuMusic()
    {
        StopAllMusic();
        menuMusicInstance = RuntimeManager.CreateInstance(menuMusicEvent);
        menuMusicInstance.start();
    }

    public void PlayGameplayMusic()
    {
        StopMenuMusic();
        gameplayMusicInstance = RuntimeManager.CreateInstance(gameplayMusicEvent);
        gameplayMusicInstance.start();
    }

    public void StopMenuMusic()
    {
        if (menuMusicInstance.isValid())
        {
            menuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            menuMusicInstance.release();
        }
    }

    public void StopAllMusic()
    {
        StopMenuMusic();
        if (gameplayMusicInstance.isValid())
        {
            gameplayMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            gameplayMusicInstance.release();
        }
    }

    public void PlayMusic(int id)
    {
        if (musicDictionary.ContainsKey(id))
            RuntimeManager.PlayOneShot(musicDictionary[id], board ? board.transform.position : Vector3.zero);
    }
    public void SetRondaParameter(int rondaValue)
    {
        if (gameplayMusicInstance.isValid())
        {
            gameplayMusicInstance.setParameterByName("RONDA", rondaValue);
        Debug.Log("Paràmetre RONDA establert a: " + rondaValue);
        }
}
}

