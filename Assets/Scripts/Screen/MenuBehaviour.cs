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
    private bool enableMenuButtons = false;
    private bool enterDifficultyMenu = false;
    private bool enterCampaignMenu = false;
    private bool enableDifficultyButtons = false;
    private bool disableDifficultyButtons = false;
    private bool enableCampaignButtons = false;
    private bool disableCampaignButtons = false;

    private void Start()
    {
        StartCoroutine(EnterMainMenuButtons());
    }

    private void Update()
    {
        //Main Menu Checker
        AnimatorStateInfo mainMenuAnimatorStateInfo = mainMenuButtons[mainMenuButtons.Length - 1].GetCurrentAnimatorStateInfo(0);
        float NTime = mainMenuAnimatorStateInfo.normalizedTime;

        if (mainMenuAnimatorStateInfo.IsName("Enter") || mainMenuAnimatorStateInfo.IsName("Back") && NTime > 1.0f)
        {
            enableMenuButtons = true;
        }

        if (mainMenuAnimatorStateInfo.IsName("Exit") && NTime > 1.0f)
        {
            enterDifficultyMenu = true;
        }

        //Difficulty Menu Checker
        AnimatorStateInfo difficultyMenuAnimatorStateInfo = difficultyMenuButtons[difficultyMenuButtons.Length - 1].GetCurrentAnimatorStateInfo(0);
        float DTime = difficultyMenuAnimatorStateInfo.normalizedTime;

        if (difficultyMenuAnimatorStateInfo.IsName("Enter") || difficultyMenuAnimatorStateInfo.IsName("Back") && DTime > 1.0f)
        {
            enableDifficultyButtons = true;
        }

        if (difficultyMenuAnimatorStateInfo.IsName("Exit") && NTime > 1.0f)
        {
            enterCampaignMenu = true;
        }

        if (difficultyMenuAnimatorStateInfo.IsName("Hide") && DTime > 1.0f)
        {
            disableDifficultyButtons = true;
        }

        //Campaign Menu Checker
        AnimatorStateInfo campaignMenuAnimatorStateInfo = campaignButtons[campaignButtons.Length - 1].GetCurrentAnimatorStateInfo(0);
        float CTime = campaignMenuAnimatorStateInfo.normalizedTime;

        if (campaignMenuAnimatorStateInfo.IsName("Enter") && CTime > 1.0f)
        {
            enableCampaignButtons = true;
        }

        if (campaignMenuAnimatorStateInfo.IsName("Hide") && DTime > 1.0f)
        {
            disableCampaignButtons = true;
        }
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
                case "Back_D":
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
                case "Back_D":
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
}
