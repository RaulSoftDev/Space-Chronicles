using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private void Update()
    {
        SetCurrentButtonText();
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
        PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene);
    }

    IEnumerator waitForExit()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    IEnumerator LoadPreviousScene()
    {
        yield return new WaitForSeconds(1);
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
