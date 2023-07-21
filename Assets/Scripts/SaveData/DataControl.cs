using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataControl : Singleton<DataControl>
{
    //Difficulty
    public enum Difficulty
    {
        Pussycat = 0,
        Average = 1,
        Foolish = 2,
        Insane = 3
    }

    private Difficulty difficulty;

    //Enemies Values
    public int enemySpeed = 150;

    public int enemyBulletDamage = 10;
    public int enemyRocketDamage = 30;

    public int basic = 20;
    public int basicII = 40;
    public int shieldShip = 80;
    public int rocket = 80;

    public int shield = 40;


    //Player Values
    public int shieldPoints = 1;
    public int rocketPoints = 1;
    public int health = 600;

    private void Start()
    {
        //ResetData();
    }

    public void ResetData()
    {
        //Reset values
        difficulty = Difficulty.Pussycat;
        PlayerPrefs.SetInt("LevelToUnlock", 0);
        PlayerPrefs.DeleteKey("PussycatDone");
        PlayerPrefs.DeleteKey("AverageDone");
        PlayerPrefs.DeleteKey("FoolishDone");
        PlayerPrefs.DeleteKey("InsaneDone");
        SaveValues();
        Debug.Log("DATA SAVED");
    }

    public void SetPussycatDifficultyData()
    {
        //Enemies Values
        enemySpeed = 150;

        enemyBulletDamage = 10;
        enemyRocketDamage = 30;

        basic = 20;
        basicII = 40;
        shieldShip = 80;
        rocket = 80;

        shield = 80;

        //Player Values
        shieldPoints = 1;
        rocketPoints = 1;
        health = 600;

        //Set values
        SaveValues();
    }

    public void SetAverageDifficultyData()
    {
        //Enemies Values
        enemySpeed = 120;

        enemyBulletDamage = 15;
        enemyRocketDamage = 45;

        basic = 60;
        basicII = 80;
        shieldShip = 100;
        rocket = 100;

        shield = 120;

        //Player Values
        health = 600;

        //Set values
        SaveValues();
        Debug.Log("SAVING DATA");
    }

    public void SetFoolishDifficultyData()
    {
        //Enemies Values
        enemySpeed = 100;

        enemyBulletDamage = 20;
        enemyRocketDamage = 60;

        basic = 80;
        basicII = 100;
        shieldShip = 120;
        rocket = 120;

        shield = 140;

        //Player Values
        health = 600;

        //Set values
        SaveValues();
        Debug.Log("SAVING DATA");
    }

    public void SetInsaneDifficultyData()
    {
        //Enemies Values
        enemySpeed = 90;

        enemyBulletDamage = 25;
        enemyRocketDamage = 75;

        basic = 100;
        basicII = 120;
        shieldShip = 140;
        rocket = 140;

        shield = 160;

        //Player Values
        health = 600;

        //Set values
        SaveValues();
        Debug.Log("SAVING DATA");
    }

    private void SaveValues()
    {
        //Enemy values
        PlayerPrefs.SetInt("EnemySpeed", enemySpeed);
        PlayerPrefs.SetInt("EnemyBulletDamage", enemyBulletDamage);
        PlayerPrefs.SetInt("EnemyRocketDamage", enemyRocketDamage);
        PlayerPrefs.SetInt("BHealth", basic);
        PlayerPrefs.SetInt("BIIHealth", basicII);
        PlayerPrefs.SetInt("ShieldShipHealth", shieldShip);
        PlayerPrefs.SetInt("ShieldValue", shield);
        PlayerPrefs.SetInt("RocketHealth", rocket);

        //Player values
        PlayerPrefs.SetInt("ShieldPoints", shieldPoints);
        PlayerPrefs.SetInt("RocketPoints", rocketPoints);
        PlayerPrefs.SetInt("PlayerHealth", health);
    }
}
