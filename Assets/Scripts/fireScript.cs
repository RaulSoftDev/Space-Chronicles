using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class fireScript : MonoBehaviour
{
    public static fireScript instance;

    AudioSource audioSourcePlayer;
    public Transform firePosition;
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    public float bulletForce = 5;
    public float rocketForce = 5;
    public AudioClip bulletSound;
    public AudioClip rocketSound;

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
        audioSourcePlayer = GetComponent<AudioSource>();
    }


    private void Update()
    {
        SaveSlideValue();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }

        CheckRocketAvailability();
    }


    //Set the player health slider current value
    private void SaveSlideValue()
    {
        PlayerHealth.instance.powerSlider.value = PlayerHealth.instance.playerPoints / 10;
    }

    //Instantiate the basic bullet
    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePosition.up * bulletForce, ForceMode2D.Impulse);
        audioSourcePlayer.PlayOneShot(bulletSound);
    }

    //Set the rocket current status
    public void CheckRocketAvailability()
    {
        switch (PlayerHealth.instance.powerSlider.value)
        {
            case 1:
                PlayerHealth.instance.shoot1.color = Color.HSVToRGB(0, 0, 1);
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    LaunchRocket();
                    PlayerHealth.instance.shoot1.color = Color.HSVToRGB(0, 0, 0.24f);
                    PlayerHealth.instance.playerPoints = 0;
                }
                return;
            case 2:
                PlayerHealth.instance.shoot2.color = Color.HSVToRGB(0, 0, 1);
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    LaunchRocket();
                    PlayerHealth.instance.shoot2.color = Color.HSVToRGB(0, 0, 0.24f);
                    PlayerHealth.instance.playerPoints = 10;
                }
                return;
            case 3:
                PlayerHealth.instance.shoot3.color = Color.HSVToRGB(0, 0, 1);
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    LaunchRocket();
                    PlayerHealth.instance.shoot3.color = Color.HSVToRGB(0, 0, 0.24f);
                    PlayerHealth.instance.playerPoints = 20;
                }
                return;
        }
    }

    //Instantiate the rocket shot
    void LaunchRocket()
    {
        GameObject rocket = Instantiate(rocketPrefab, firePosition.position, firePosition.rotation);
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        rb.AddForce(firePosition.up * bulletForce, ForceMode2D.Impulse);
        audioSourcePlayer.volume = 0.7f;
        audioSourcePlayer.PlayOneShot(rocketSound);
    }
}
