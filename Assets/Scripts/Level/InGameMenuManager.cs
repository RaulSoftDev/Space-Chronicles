using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{
    private bool skipLogo = false;
    public bool inGameMenuHide = false;
    private bool settingsClosed = false;
    private float inGameMenuTime = 0;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject inGameMenu;

    private void Update()
    {
        //IN GAME MENU
        if (inGameMenu.activeInHierarchy)
        {
            AnimatorStateInfo inGameMenuState = inGameMenu.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

            if (inGameMenuState.IsName("Exit Menu") || inGameMenuState.IsName("CloseMenu"))
            {
                inGameMenuTime = inGameMenuState.normalizedTime;
                if (inGameMenuTime > 1.0f)
                {
                    inGameMenuHide = true;
                }
            }
            else
            {
                inGameMenuTime = 0;
                inGameMenuHide = false;
            }
        }

        //SETTINGS MENU
        if (settingsMenu.activeInHierarchy)
        {
            AnimatorStateInfo settingsState = settingsMenu.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

            if (settingsState.IsName("Exit Settings"))
            {
                float settingsTime = settingsState.normalizedTime;
                if (settingsTime > 1.0f)
                {
                    settingsClosed = true;
                }
            }
        }
    }

    //MENU SETTINGS
    public void RestartScene()
    {
        BlockUIButtons();
        MenuScript.Instance.RestartPreviousScene();
    }

    private void BlockUIButtons()
    {
        Button[] UIButtons = FindObjectsOfType<Button>();

        foreach (Button button in UIButtons)
        {
            button.interactable = false;
        }
    }

    public void OpenInGameMenu()
    {
        Time.timeScale = 0;
        inGameMenu.SetActive(true);
        inGameMenu.gameObject.GetComponent<Animator>().SetTrigger("EnterMenu");
    }

    public void OpenSettings()
    {
        //Hide InGame Menu
        inGameMenu.GetComponent<Animator>().SetTrigger("ExitMenu");
        StartCoroutine(InGameMenuOut());
    }

    IEnumerator InGameMenuOut()
    {
        yield return new WaitUntil(() => inGameMenuHide);
        //inGameMenuHide = false;
        //Enter Animation
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<Animator>().SetTrigger("EnterMenu");
    }

    public void CloseSettings()
    {
        //Enter Animation
        settingsMenu.GetComponent<Animator>().SetTrigger("ExitMenu");
        StartCoroutine(SettingsOut());
    }

    IEnumerator SettingsOut()
    {
        yield return new WaitUntil(() => settingsClosed);
        settingsClosed = false;
        //Set Active Settings Off
        settingsMenu.SetActive(false);
        //Enter In Game Menu
        inGameMenu.GetComponent<Animator>().SetTrigger("BackMenu");
    }

    public void CloseInGameMenu()
    {
        //Enter Animation
        inGameMenu.GetComponent<Animator>().SetTrigger("CloseMenu");
        StartCoroutine(InGameMenuClosing());
    }

    IEnumerator InGameMenuClosing()
    {
        yield return new WaitUntil(() => inGameMenuHide);
        inGameMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitToMainMenu()
    {
        BlockUIButtons();
        Time.timeScale = 1;
        skipLogo = true;
        PlayerPrefs.SetInt("SkipLogo", (skipLogo ? 1 : 0));
        MenuScript.Instance.MainMenu(1);
    }
}
