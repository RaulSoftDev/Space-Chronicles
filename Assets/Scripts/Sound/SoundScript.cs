using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SoundScript : MonoBehaviour
{
    public static SoundScript instance;

    public AudioSource audioSource;
    public AudioClip song;
    public AudioClip songMenu;
    public AudioClip tutorialSong;
    public AudioClip victoryClip1;
    public AudioClip victoryClip2;
    public AudioClip deathClip;
    public AudioMixer mixer;
    public bool play;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);

        //PlayMusic(play, scene);
        ChangeSongMenu(scene);
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        LoadAudioSaveData();
    }

    private void LoadAudioSaveData()
    {
        mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", Mathf.Log10(0.5f) * 20));
        mixer.SetFloat("ButtonsVolume", PlayerPrefs.GetFloat("ButtonsVolume", Mathf.Log10(0.5f) * 20));
        mixer.SetFloat("FXVolume", PlayerPrefs.GetFloat("FXVolume", Mathf.Log10(0.5f) * 20));
    }

    void ChangeSongMenu(Scene scene)
    {
        switch (scene.name)
        {
            case "Studio_Intro":
                //NO MUSIC
                break;
            case "Round1":
                gameObject.GetComponent<Animator>().SetTrigger("FadeIn_System1");
                audioSource.loop = true;
                audioSource.volume = 0.8f;
                audioSource.clip = song;
                audioSource.Play();
                break;
            case "Tutorial":
                gameObject.GetComponent<Animator>().SetTrigger("FadeIn_Tutorial");
                audioSource.loop = true;
                audioSource.volume = 0.4f;
                audioSource.clip = tutorialSong;
                audioSource.Play();
                break;
            case "MainMenu":
                gameObject.GetComponent<Animator>().SetTrigger("FadeIn_MainMenu");
                audioSource.volume = 0.5f;
                if (audioSource.clip != songMenu)
                {
                    audioSource.loop = true;
                    audioSource.clip = songMenu;
                    audioSource.Play();
                }
                break;
        }
    }

    public void FadeOutMusic(Scene scene)
    {
        switch (scene.name)
        {
            case "Studio_Intro":
                //NO MUSIC
                break;
            case "Round1":
                gameObject.GetComponent<Animator>().SetTrigger("FadeOut_System1");
                break;
            case "Tutorial":
                gameObject.GetComponent<Animator>().SetTrigger("FadeOut_Tutorial");
                break;
            case "MainMenu":
                gameObject.GetComponent<Animator>().SetTrigger("FadeOut_MainMenu");
                break;
        }
    }

    public void PlayVictoryMusic()
    {
        audioSource.Stop();
        audioSource.clip = victoryClip1;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayDeathMusic()
    {
        audioSource.Stop();
        audioSource.clip = deathClip;
        audioSource.loop = false;
        audioSource.Play();
    }
}
