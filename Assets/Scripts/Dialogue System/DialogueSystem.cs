using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    public Animator continueButton;
    private Animator dialogueAnimator;
    public bool isDialogueClosed = false;

    private void OnEnable()
    {
        StartCoroutine(ShowButton());
    }

    void Start()
    {
        dialogueAnimator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        /*if(textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }*/

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
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void NextSentence()
    {
        HideButton();

        if (index < sentences.Length - 1)
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

    IEnumerator ShowButton()
    {
        yield return new WaitUntil(() => textDisplay.text == sentences[index]);
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
