using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataControl : Singleton<DataControl>
{
    //Enemies Values
    public int basic = 2;
    public int basicII = 4;
    public int shieldShip = 6;
    public int rocket = 8;

    public int shield = 3;

    //Player Values
    public int health = 500;

    private void Awake()
    {
        ResetData();
    }

    public void ResetData()
    {
        //Enemies Values
        basic = 2;
        basicII = 4;
        shieldShip = 6;
        rocket = 8;

        shield = 3;

        //Player Values
        health = 500;

        //Set values
        SaveValues();
    }

    public void SetPussycatDifficultyData()
    {
        ResetData();

        //Set values
        SaveValues();
    }

    public void SetAverageDifficultyData()
    {
        //Enemies Values
        basic *= 2;
        basicII *= 2;
        shieldShip *= 2;
        rocket *= 2;

        shield *= 2;

        //Player Values
        health = 400;

        //Set values
        SaveValues();
        Debug.Log("SAVING DATA");
    }

    public void SetFoolishDifficultyData()
    {
        //Enemies Values
        basic *= 3; ;
        basicII *= 3;
        shieldShip *= 3;
        rocket *= 3;

        shield *= 3;

        //Player Values
        health = 300;

        //Set values
        SaveValues();
        Debug.Log("SAVING DATA");
    }

    public void SetInsaneDifficultyData()
    {
        //Enemies Values
        basic *= 5; ;
        basicII *= 5;
        shieldShip *= 5;
        rocket *= 5;

        shield *= 5;

        //Player Values
        health = 150;

        //Set values
        SaveValues();
        Debug.Log("SAVING DATA");
    }

    private void SaveValues()
    {
        PlayerPrefs.SetInt("BHealth", basic);
        PlayerPrefs.SetInt("BIIHealth", basicII);
        PlayerPrefs.SetInt("ShieldShipHealth", shieldShip);
        PlayerPrefs.SetInt("ShieldValue", shield);
        PlayerPrefs.SetInt("RocketHealth", rocket);
        PlayerPrefs.SetInt("PlayerHealth", health);
    }
}
