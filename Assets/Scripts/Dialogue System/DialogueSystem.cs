using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public DialogueIndex textIndex;
    private int index;
    public float typingSpeed;
    public Animator continueButton;
    private Animator dialogueAnimator;
    public bool isDialogueClosed = false;

    private void OnEnable()
    {
        //SetDialogue();
        StartCoroutine(ShowButton());
    }

    void Start()
    {
        dialogueAnimator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Close Dialogue"))
        {
            float ATime = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
            if(ATime > 1 && ATime <= 3)
            {
                CloseWindow();
                isDialogueClosed = true;
            }
            else
            {
                isDialogueClosed = false;
            }
        }
        else
        {
            isDialogueClosed = false;
        }
    }

    IEnumerator Type()
    {
        foreach (char letter in textIndex.DialogueOutput[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        HideButton();

        if (index < textIndex.DialogueOutput.Count - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
            StartCoroutine(ShowButton());
        }
        else
        {
            //textDisplay.text = "";
            dialogueAnimator.SetTrigger("CloseDialogue");
            //MenuScript.Instance.MainMenu(0);
        }
    }

    public void CloseWindow()
    {
        textDisplay.text = "";
        index = 0;
    }

    public void OpenDialogue()
    {
        index = 0;
        StartCoroutine(Type());
    }

    private void SetDialogue()
    {
        textIndex.DialogueOutput.Clear();
        textIndex.Dialogue_System1_Intro();
    }

    IEnumerator ShowButton()
    {
        yield return new WaitUntil(() => textDisplay.text == textIndex.DialogueOutput[index]);
        continueButton.SetTrigger("ShowButton");
    }

    public void HideButton()
    {
        continueButton.SetTrigger("HideButton");
    }

    private void OnDisable()
    {
        isDialogueClosed = false;
    }
}
