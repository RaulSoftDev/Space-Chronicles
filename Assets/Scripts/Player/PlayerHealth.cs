using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public float playerHealth = 11;
    public bool GetDamage;
    public Slider healthSlider;
    public Slider powerSlider;
    public int playerPoints = 0;
    public int rocketPoints = 0;
    public float playerShieldPoints = 0;

    public Animator healthBar;

    public float enemyBulletValue = 1;
    public float enemyRocketValue = 3;

    public GameObject playerExplosion;

    AudioSource playerAudiosource;
    public AudioClip rocketAvailable;
    public AudioClip shieldAvailable;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        healthSlider.maxValue = playerHealth;
        playerAudiosource = GetComponent<AudioSource>();
        GetDamage = true;
        StartCoroutine(PlayerStats());
    }

    //Load death scene if player dies
    private IEnumerator PlayerStats()
    {
        yield return new WaitUntil(() => playerHealth < 1);
        GameObject explotionInstance = Instantiate(playerExplosion, transform.position, transform.rotation);
        explotionInstance.transform.parent = transform;
        //Load scene
        MenuScript.Instance.StartMenu(4);
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetDamage)
        {
            if(playerHealth >= 1)
            {
                switch (collision.tag)
                {
                    case "BulletIBasic":
                        playerHealth -= enemyBulletValue;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
                    case "Missile":
                        playerHealth -= enemyRocketValue;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
                    case "BulletIII":
                        playerHealth -= enemyBulletValue;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
                    case "BulletBoss":
                        playerHealth -= enemyBulletValue;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
                    /*case "Missile":
                        playerHealth -= 3;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;*/
                }
            }
        } 
    }

    private void Update()
    {
        if(playerPoints < 0)
        {
            playerPoints = 0;
        }

        if(playerPoints > 30)
        {
            playerPoints = 30;
        }

        if (playerShieldPoints < 0)
        {
            playerShieldPoints = 0;
        }

        if (playerShieldPoints > 20)
        {
            playerShieldPoints = 20;
        }

        HealthSliderValues();

        HealthBarColor();
    }

    private void HealthSliderValues()
    {
        healthSlider.value = playerHealth;
    }

    private void HealthBarColor()
    {
        if(playerHealth < 100)
        {
            healthBar.SetTrigger("OrangeOn");
        } 
    }
}
