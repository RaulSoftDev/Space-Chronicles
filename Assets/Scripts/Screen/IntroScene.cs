using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class IntroScene : MonoBehaviour
{
    private bool skipLogo = false;

    private void Start()
    {
        //CleanStart();
        LoadData();
        StartCoroutine(LoadMainMenu());
    }

    private void CleanStart()
    {
        PlayerPrefs.DeleteAll();
    }

    private void LoadData()
    {
        PlayerPrefs.SetInt("SkipLogo", skipLogo ? 1 : 0);

        //Enemy values
        PlayerPrefs.SetInt("EnemySpeed", 150);
        PlayerPrefs.SetInt("EnemyBulletDamage", 10);
        PlayerPrefs.SetInt("EnemyRocketDamage", 30);
        PlayerPrefs.SetInt("BHealth", 20);
        PlayerPrefs.SetInt("BIIHealth", 40);
        PlayerPrefs.SetInt("ShieldShipHealth", 80);
        PlayerPrefs.SetInt("ShieldValue", 100);
        PlayerPrefs.SetInt("RocketHealth", 80);

        //Player values
        PlayerPrefs.SetInt("ShieldPoints", 1);
        PlayerPrefs.SetInt("RocketPoints", 1);
        PlayerPrefs.SetInt("PlayerHealth", 600);
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(7);
        //Load Main Menu
        MenuScript.Instance.MainMenu(1);
    }
}
