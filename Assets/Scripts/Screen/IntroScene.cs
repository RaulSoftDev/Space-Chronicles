using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private bool skipLogo = false;

    private void Start()
    {
        PlayerPrefs.SetInt("SkipLogo", skipLogo ? 1 : 0);
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(5);
        //Load Main Menu
        MenuScript.Instance.MainMenu(1);
    }
}
