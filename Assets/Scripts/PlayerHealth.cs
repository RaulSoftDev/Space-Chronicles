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
    public Image shoot1;
    public Image shoot2;
    public Image shoot3;
    public Image shield1;

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
        playerAudiosource = GetComponent<AudioSource>();
        GetDamage = true;
        StartCoroutine(PlayerStats());
    }

    //Load death scene if player dies
    private IEnumerator PlayerStats()
    {
        while (true)
        {
            yield return new WaitUntil(() => playerHealth <= 0);
            SceneManager.LoadScene(4);
        }
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
                        playerHealth -= 1;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
                    case "Bullet_B":
                        playerHealth -= 1;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
                    case "BulletIII":
                        playerHealth -= 1;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
                    case "BulletBoss":
                        playerHealth -= 1;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
                    case "Missile":
                        playerHealth -= 3;
                        gameObject.GetComponent<Animator>().SetTrigger("DamagePlayerOn");
                        return;
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
    }

    private void HealthSliderValues()
    {
        healthSlider.value = playerHealth;
    }

    void ShieldActivation()
    {
        if (playerShieldPoints >= 20)
        {
            playerShieldPoints = 20;
        }

        if (playerShieldPoints <= 0)
        {
            playerShieldPoints = 0;
        }

        if (playerShieldPoints == 20)
        {
            shield1.color = Color.HSVToRGB(0, 0, 1);
        }

        if (playerShieldPoints == 0)
        {
            shield1.color = Color.HSVToRGB(0, 0, 0);
        }
    }
}
