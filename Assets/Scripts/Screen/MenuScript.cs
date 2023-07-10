using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : Singleton<MenuScript>
{
    private float t = 0;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SController");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += StopTiming;
    }

    private void StopTiming(Scene scene, LoadSceneMode mode)
    {
        StopCoroutine("Timing");
    }

    public void MainMenu(int scene)
    {
        StartCoroutine(waitForScene(scene));
    }

    public void ControlsMenu(int scene)
    {
        StartCoroutine(waitForScene(scene));
    }

    public void StartMenu(int scene)
    {
        StartCoroutine(waitForScene(scene));
    }

    public void RestartPreviousScene()
    {
        StartCoroutine(LoadPreviousScene());
    }

    public void ExitMenu()
    {
        Application.Quit();
    }

    IEnumerator waitForScene(int scene)
    {
        //PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
        StartCoroutine("Timing");
        BlackScreenLoader.Instance.LoadBlackScreen();
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(scene);
    }

    IEnumerator Timing()
    {
        t = 0;
        while (true)
        {
            t += 0.5f * Time.deltaTime;
            SoundScript.instance.audioSource.volume = Mathf.Lerp(1, 0, t);
            yield return null;
        }
    }

    IEnumerator waitForExit()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    IEnumerator LoadPreviousScene()
    {
        BlackScreenLoader.Instance.LoadBlackScreen();
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastScene"));
    }

    private void SetCurrentButtonText()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            transform.GetChild(0).transform.GetComponent<Text>().color = new Color(0.2075472f, 0.2075472f, 0.2075472f, 1);
        }
        else
        {
            transform.GetChild(0).transform.GetComponent<Text>().color = new Color(0.2f, 1, 0, 1);
        }
    }
}
