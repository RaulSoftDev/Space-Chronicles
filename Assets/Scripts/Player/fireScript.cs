using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class fireScript : MonoBehaviour
{
    public static fireScript instance;

    AudioSource audioSourcePlayer;
    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float bulletForce = 5;
    [SerializeField] private float rocketForce = 5;
    [SerializeField] private AudioClip bulletSound;
    [SerializeField] private AudioClip rocketSound;
    [SerializeField] private Image rocketButtonBackground;
    [SerializeField] private Button rocketButton;
    [SerializeField] private Button basicAttackButton;
    public bool canAttack = false;
    private InputManager inputManager;


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
        inputManager = InputManager.Instance;

        StartCoroutine(ShootBullet());
    }


    private void Update()
    {
        SaveSlideValue();

        CheckRocketAvailability();

        if (!inputManager.joystickMode)
        {
            if (SwipeDetection.Instance.touchStart && !EventSystem.current.IsPointerOverGameObject()) canAttack = true;
            else canAttack = false;
        }
    }


    //Set the player health slider current value
    private void SaveSlideValue()
    {
        PlayerHealth.instance.powerSlider.value = PlayerHealth.instance.playerPoints / 10;
    }

    public void ShootingButtonMode()
    {
        //canAttack = true;
    }

    //Instantiate the basic bullet
    private IEnumerator ShootBullet()
    {
        while (true)
        {
            yield return new WaitUntil(() => canAttack && transform.position.y > -3.5f);
            GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePosition.up * bulletForce, ForceMode2D.Impulse);
            audioSourcePlayer.PlayOneShot(bulletSound);
            Debug.Log("ShootOut");
            yield return new WaitForSeconds(0.3f);
        }
    }

    //Set the rocket current status
    public void CheckRocketAvailability()
    {
        switch (PlayerHealth.instance.powerSlider.value)
        {
            case 0:
                rocketButtonBackground.fillAmount = 0f;
                rocketButton.interactable = false;
                break;
            case 1:
                rocketButtonBackground.fillAmount = 0.33f;
                //PlayerHealth.instance.shoot1.color = Color.HSVToRGB(0, 0, 1);
                rocketButton.interactable = true;
                return;
            case 2:
                rocketButtonBackground.fillAmount = 0.66f;
                //PlayerHealth.instance.shoot2.color = Color.HSVToRGB(0, 0, 1);
                return;
            case 3:
                rocketButtonBackground.fillAmount = 1f;
                //PlayerHealth.instance.shoot3.color = Color.HSVToRGB(0, 0, 1);
                return;
        }
    }

    public void RocketManagement()
    {
        switch (PlayerHealth.instance.powerSlider.value)
        {
            case 1:
                LaunchRocket();
                PlayerHealth.instance.playerPoints = 0;
                break;
            case 2:
                LaunchRocket();
                PlayerHealth.instance.playerPoints = 10;
                break;
            case 3:
                LaunchRocket();
                PlayerHealth.instance.playerPoints = 20;
                break;
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
