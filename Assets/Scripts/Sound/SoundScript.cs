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
        audioSource.Stop();
        audioSource.volume = 1.0f;

        switch (scene.name)
        {
            case "Studio_Intro":
                //NO MUSIC
                break;
            case "Round1":
                audioSource.loop = true;
                audioSource.clip = song;
                audioSource.Play();
                break;
            case "Round1Joystick":
                audioSource.loop = true;
                audioSource.clip = song;
                audioSource.Play();
                break;
            case "MainMenu":
                if(audioSource.clip != songMenu)
                {
                    audioSource.loop = true;
                    audioSource.clip = songMenu;
                    audioSource.Play();
                }
                break;
            case "Victory":
                StartCoroutine(victoryClipAudio());
                break;
            case "Death":
                audioSource.loop = false;
                audioSource.clip = deathClip;
                audioSource.Play();
                break;
        }
    }

    IEnumerator victoryClipAudio()
    {
        audioSource.clip = victoryClip1;
        audioSource.loop = false;
        audioSource.Play();
        yield return new WaitForSeconds(victoryClip1.length);
        audioSource.Stop();
        audioSource.clip = victoryClip2;
        audioSource.Play();
        yield break;
    }
}
