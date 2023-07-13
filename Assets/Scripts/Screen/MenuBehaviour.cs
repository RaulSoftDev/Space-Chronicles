using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
    [SerializeField] Animator[] mainMenuButtons;
    [SerializeField] Animator[] difficultyMenuButtons;
    [SerializeField] Animator[] campaignButtons;
    [SerializeField] Animator[] menuTitles;
    [SerializeField] GameObject disabledMenu;
    [SerializeField] Animator disabledMenuAnimator;
    [SerializeField] Animator disabledDifficultyMenuAnimator;
    [SerializeField] Button disabledMenuButton;
    [SerializeField] Animator titleScreen;
    [SerializeField] Animator dialogSystem;
    private bool enableMenuButtons = false;
    private bool enterDifficultyMenu = false;
    private bool enterCampaignMenu = false;
    private bool enableDifficultyButtons = false;
    private bool disableDifficultyButtons = false;
    private bool enableCampaignButtons = false;
    private bool disableCampaignButtons = false;
    private bool enableDisabledScreenButton = false;
    private bool disableDisabledScreenButton = false;
    private bool disableTapScreen = false;
    private int currentUnlockedDifficulty = 0;
    public bool skipLogo = false;

    private void Awake()
    {
        if (skipLogo)
        {
            UnlockNextDifficulty();
        }
    }

    private void Start()
    {
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitUntil(() => disableTapScreen);
        titleScreen.gameObject.SetActive(false);
        StartCoroutine(EnterMainMenuButtons());
    }

    #region Menu Loading Times
    private void LoadCurrentMenuTimes()
    {
        //Main Title Checker
        AnimatorStateInfo mainTitleAnimatorStateInfo = titleScreen.GetCurrentAnimatorStateInfo(0);

        if (mainTitleAnimatorStateInfo.IsName("Hide"))
        {
            float MTTime = mainTitleAnimatorStateInfo.normalizedTime;
            if (MTTime > 1)
            {
                disableTapScreen = true;
            }
        }

        //Main Menu Checker
        AnimatorStateInfo mainMenuAnimatorStateInfo = mainMenuButtons[mainMenuButtons.Length - 1].GetCurrentAnimatorStateInfo(0);

        if (mainMenuAnimatorStateInfo.IsName("Enter"))
        {
            float NTime = mainMenuAnimatorStateInfo.normalizedTime;
            if(NTime > 1)
            {
                enableMenuButtons = true;
            }
        }

        if (mainMenuAnimatorStateInfo.IsName("Back"))
        {
            float NTime = mainMenuAnimatorStateInfo.normalizedTime;
            if (NTime > 1)
            {
                enableMenuButtons = true;
                Debug.LogError("Menu");
            }
        }

        if (mainMenuAnimatorStateInfo.IsName("Exit"))
        {
            float NTime = mainMenuAnimatorStateInfo.normalizedTime;
            if(NTime > 1)
            {
                enterDifficultyMenu = true;
            }
        }

        //Difficulty Menu Checker
        AnimatorStateInfo difficultyMenuAnimatorStateInfo = difficultyMenuButtons[difficultyMenuButtons.Length - 1].GetCurrentAnimatorStateInfo(0);

        if (difficultyMenuAnimatorStateInfo.IsName("Enter"))
        {
            float DTime = difficultyMenuAnimatorStateInfo.normalizedTime;
            if(DTime > 1)
            {
                enableDifficultyButtons = true;
            }
        }

        if (difficultyMenuAnimatorStateInfo.IsName("Back"))
        {
            float DTime = difficultyMenuAnimatorStateInfo.normalizedTime;
            if (DTime > 1)
            {
                enableDifficultyButtons = true;
            }
        }

        if (difficultyMenuAnimatorStateInfo.IsName("Exit"))
        {
            float DTime = difficultyMenuAnimatorStateInfo.normalizedTime;
            if (DTime > 1.0f)
            {
                enterCampaignMenu = true;
            }
        }

        if (difficultyMenuAnimatorStateInfo.IsName("Hide"))
        {
            float DTime = difficultyMenuAnimatorStateInfo.normalizedTime;
            if(DTime > 1.0f)
            {
                disableDifficultyButtons = true;
            }
        }

        //Campaign Menu Checker
        AnimatorStateInfo campaignMenuAnimatorStateInfo = campaignButtons[campaignButtons.Length - 1].GetCurrentAnimatorStateInfo(0);

        if (campaignMenuAnimatorStateInfo.IsName("Enter"))
        {
            float CTime = campaignMenuAnimatorStateInfo.normalizedTime;
            if (CTime > 1.0f)
            {
                enableCampaignButtons = true;
            }
        }

        if (campaignMenuAnimatorStateInfo.IsName("Hide"))
        {
            float CTime = campaignMenuAnimatorStateInfo.normalizedTime;
            if (CTime > 1.0f)
            {
                disableCampaignButtons = true;
            }
        }

        //Disabled Menu Checker
        AnimatorStateInfo disabledMenuAnimatorStateInfo = disabledMenuAnimator.GetCurrentAnimatorStateInfo(0);

        if (disabledMenuAnimatorStateInfo.IsName("Disabled Screen On"))
        {
            float DSTime = disabledMenuAnimatorStateInfo.normalizedTime;
            if (DSTime > 1.0f)
            {
                enableDisabledScreenButton = true;
                disabledMenuButton.interactable = true;
            }
        }

        if (disabledMenuAnimatorStateInfo.IsName("Disabled Screen Off"))
        {
            float DSTime = disabledMenuAnimatorStateInfo.normalizedTime;
            if (DSTime > 1.0f)
            {
                enableDisabledScreenButton = false;
                disabledMenu.SetActive(false);
            }
        }

        //Disabled Difficulty Menu Checker
        AnimatorStateInfo disabledDifficultyMenuAnimatorStateInfo = disabledDifficultyMenuAnimator.GetCurrentAnimatorStateInfo(0);

        if (disabledDifficultyMenuAnimatorStateInfo.IsName("Disabled Screen On"))
        {
            float DDTime = disabledDifficultyMenuAnimatorStateInfo.normalizedTime;
            if (DDTime > 1.0f)
            {
                disabledMenuButton.interactable = true;
            }
        }

        if (disabledDifficultyMenuAnimatorStateInfo.IsName("Disabled Screen Off"))
        {
            float DDTime = disabledDifficultyMenuAnimatorStateInfo.normalizedTime;
            if (DDTime > 1.0f)
            {
                disabledMenu.SetActive(false);
            }
        }
    }
    #endregion

    public void HideTitle()
    {
        titleScreen.SetTrigger("Hide");
    }

    private void Update()
    {
        LoadCurrentMenuTimes();
    }

    #region Main Menu
    private IEnumerator EnterMainMenuButtons()
    {
        menuTitles[0].SetTrigger("TitleOn");

        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].SetTrigger("Enter");
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitUntil(() => enableMenuButtons);
        //mainMenuButtons[0].transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
        foreach (Animator button in mainMenuButtons)
        {
            switch (button.name)
            {
                case "Campaign":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "GOAT":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Hangar":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Tutorial":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Options":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Credits":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
            }
        }

        enableMenuButtons = false;
    }

    private IEnumerator BackMainMenuButtons()
    {
        menuTitles[0].SetTrigger("TitleOn");

        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].SetTrigger("Back");
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitUntil(() => enableMenuButtons);
        //mainMenuButtons[0].transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
        foreach (Animator button in mainMenuButtons)
        {
            switch (button.name)
            {
                case "Campaign":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "GOAT":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Hangar":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Tutorial":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Options":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Credits":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
            }
        }

        enableMenuButtons= false;
    }

    private void BackToMainMenu()
    {
        StartCoroutine(BackMainMenuButtons());
    }

    private IEnumerator ExitMainMenuButtons()
    {
        //mainMenuButtons[0].transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;

        foreach (Animator button in mainMenuButtons)
        {
            button.gameObject.GetComponent<Button>().interactable = false;
        }

        menuTitles[0].SetTrigger("TitleOff");

        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].SetTrigger("Exit");
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitUntil(() => enterDifficultyMenu);
        GetInDifficultyButtons();
    }

    public void MoveOutPreviousButtons()
    {
        StartCoroutine(ExitMainMenuButtons());
    }

    private IEnumerator EnterBackMainMenuButtons()
    {
        menuTitles[1].SetTrigger("TitleOff");

        foreach (Animator button in difficultyMenuButtons)
        {
            button.gameObject.GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < difficultyMenuButtons.Length; i++)
        {
            difficultyMenuButtons[i].SetTrigger("Hide");
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitUntil(() => disableDifficultyButtons);
        BackToMainMenu();
    }

    public void GetBackMainMenu()
    {
        StartCoroutine(EnterBackMainMenuButtons());
    }

    private void SkipLogo()
    {
        titleScreen.gameObject.SetActive(false);
        StartCoroutine(EnterMainMenuButtons());

    }
    #endregion

    #region Difficulty Menu
    private IEnumerator EnterDifficultyButtons()
    {
        menuTitles[1].SetTrigger("TitleOn");

        for (int i = 0; i < difficultyMenuButtons.Length; i++)
        {
            difficultyMenuButtons[i].SetTrigger("Enter");
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitUntil(() => enableDifficultyButtons);
        //difficultyMenuButtons[0].transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
        foreach (Animator button in difficultyMenuButtons)
        {
            switch(button.name)
            {
                case "Pussycat":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
            }
        }

        enableDifficultyButtons = false;
    }

    private void GetInDifficultyButtons()
    {
        StartCoroutine(EnterDifficultyButtons());
    }

    private IEnumerator ExitDifficultyMenuButtons()
    {
        //difficultyMenuButtons[0].transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;

        foreach (Animator button in difficultyMenuButtons)
        {
            button.gameObject.GetComponent<Button>().interactable = false;
        }

        menuTitles[1].SetTrigger("TitleOff");

        for (int i = 0; i < difficultyMenuButtons.Length; i++)
        {
            difficultyMenuButtons[i].SetTrigger("Exit");
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitUntil(() => enterCampaignMenu);
        GetInCampaignButtons();
    }

    public void MoveOutDifficultyButtons()
    {
        StartCoroutine(ExitDifficultyMenuButtons());
    }

    private IEnumerator BackDifficultyButtons()
    {
        menuTitles[1].SetTrigger("TitleOn");

        for (int i = 0; i < difficultyMenuButtons.Length; i++)
        {
            difficultyMenuButtons[i].SetTrigger("Back");
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitUntil(() => enableDifficultyButtons);
        //mainMenuButtons[0].transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
        foreach (Animator button in difficultyMenuButtons)
        {
            switch (button.name)
            {
                case "Pussycat":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
            }
        }

        enableDifficultyButtons = false;
    }

    private void BackToDifficultyMenu()
    {
        StartCoroutine(BackDifficultyButtons());
    }

    private IEnumerator EnterBackDifficultyButtons()
    {
        menuTitles[2].SetTrigger("TitleOff");

        foreach (Animator button in campaignButtons)
        {
            button.gameObject.GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < campaignButtons.Length; i++)
        {
            campaignButtons[i].SetTrigger("Hide");
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitUntil(() => disableCampaignButtons);
        BackToDifficultyMenu();
    }

    public void GetBackDifficulty()
    {
        StartCoroutine(EnterBackDifficultyButtons());
    }
    #endregion

    #region Campaign Menu
    private IEnumerator EnterCampaignButtons()
    {
        menuTitles[2].SetTrigger("TitleOn");

        for (int i = 0; i < campaignButtons.Length; i++)
        {
            campaignButtons[i].SetTrigger("Enter");
            yield return new WaitForSeconds(0.75f);
        }

        yield return new WaitUntil(() => enableCampaignButtons);
        //campaignButtons[0].transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
        foreach (Animator button in campaignButtons)
        {
            if(button.name == "System01_T" && button.name == "Back")
            {
                button.gameObject.GetComponent<Button>().interactable = true;
            }

            switch (button.name)
            {
                case "System01_T":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "System02 (1)":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "System03 (1)":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "System04 (1)":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "System05 (1)":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "System06 (1)":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "System07 (1)":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
                case "Back":
                    button.gameObject.GetComponent<Button>().interactable = true;
                    break;
            }
        }

        enableCampaignButtons = false;
    }

    private void GetInCampaignButtons()
    {
        StartCoroutine(EnterCampaignButtons());
    }
    #endregion

    #region Not Available Menu Screen

    //Enable Disabled Menu Screen
    public void OnDisableMenuButton()
    {
        disabledMenu.SetActive(true);
        disabledMenuAnimator.SetTrigger("DisabledScreenOn");
    }

    public void OffDisableMenuButton()
    {
        disabledMenuButton.interactable = false;
        if(disabledMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("Disabled Screen On"))
        {
            disabledMenuAnimator.SetTrigger("DisabledScreenOff");
        }
    }

    public void TurnOffDisabledScreen()
    {
        if (enableDisabledScreenButton)
        {
            OffDisableMenuButton();
        }
        else
        {
            OffDisableDifficultyMenuButton();
        }
    }

    private void OnDisableDifficultyMenuButton()
    {
        disabledMenu.SetActive(true);
        disabledDifficultyMenuAnimator.SetTrigger("DisabledScreenOn");
    }

    private void OffDisableDifficultyMenuButton()
    {
        disabledMenuButton.interactable = false;
        if (disabledDifficultyMenuAnimator.GetCurrentAnimatorStateInfo(0).IsName("Disabled Screen On"))
        {
            disabledDifficultyMenuAnimator.SetTrigger("DisabledScreenOff");
        }
    }
    #endregion

    #region Unlocked Difficulties
    private void UnlockNextDifficulty()
    {
        //Hide Title Screen
        titleScreen.gameObject.SetActive(false);
        //Show Difficulty Menu
        GetInDifficultyButtons();
        //Show unlocked difficulty dialogue
        StartCoroutine(NewDifficultyUnlocked());
    }

    IEnumerator NewDifficultyUnlocked()
    {
        yield return new WaitUntil(() => enableDifficultyButtons);
        dialogSystem.gameObject.SetActive(true);
        dialogSystem.SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogSystem.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        dialogSystem.gameObject.SetActive(false);
        currentUnlockedDifficulty++;
        difficultyMenuButtons[currentUnlockedDifficulty].GetComponent<Button>().interactable = true;
    }
    #endregion
}
