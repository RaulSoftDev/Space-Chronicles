using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundScript : MonoBehaviour
{
    public static SoundScript instance;

    AudioSource audioSource;
    public AudioClip song;
    public AudioClip songMenu;
    public AudioClip victoryClip1;
    public AudioClip victoryClip2;
    public AudioClip deathClip;
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

    void ChangeSongMenu(Scene scene)
    {
        switch(scene.name)
        {
            case "Round1":
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.clip = song;
                audioSource.Play();
                break;
            case "Round1Joystick":
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.clip = song;
                audioSource.Play();
                break;
            case "MainMenu":
                if(audioSource.clip != songMenu)
                {
                    audioSource.Stop();
                    audioSource.loop = true;
                    audioSource.clip = songMenu;
                    audioSource.Play();
                }
                break;
            case "Victory":
                StartCoroutine(victoryClipAudio());
                break;
            case "Death":
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.clip = deathClip;
                audioSource.Play();
                break;
        }
    }

    IEnumerator victoryClipAudio()
    {
        audioSource.Stop();
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
