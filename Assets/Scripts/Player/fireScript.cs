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
using UnityEngine.SceneManagement;

public class fireScript : MonoBehaviour
{
    public static fireScript instance;

    AudioSource audioSourcePlayer;
    [SerializeField] private Transform firePosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float bulletForce = 5;
    [SerializeField] private float rocketForce = 5;
    public int bulletDamage = 1;
    [SerializeField] private AudioClip bulletSound;
    [SerializeField] private AudioClip rocketSound;
    [SerializeField] private Image rocketButtonBackground;
    [SerializeField] private Button rocketButton;
    [SerializeField] private Button basicAttackButton;
    [SerializeField] private Image[] rocketLoadButtons;
    [SerializeField] private Sprite rocketOn;
    [SerializeField] private Sprite rocketOff;
    [SerializeField] private Button triggerButton;
    private float currentBullet = 0;
    public bool canAttack = false;
    public bool enableAttack = false;
    public bool disableRocket = false;
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
        PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);

        enableAttack = false;

        audioSourcePlayer = GetComponent<AudioSource>();
        inputManager = InputManager.Instance;

        StartCoroutine(ShootBlast());
        StartCoroutine(LoadRocketOff());
        StartCoroutine(LoadRocket01());
        StartCoroutine(LoadRocket02());
        StartCoroutine(LoadRocket03());

        StartCoroutine(StopAttacksOnDeath());

        canAttack = true;
    }


    private void Update()
    {

        SaveSlideValue();
        //Debug.Log(currentBullet);

        //CheckRocketAvailability();

        if (!inputManager.joystickMode)
        {
            /*if (SwipeDetection.Instance.touchStart && !EventSystem.current.IsPointerOverGameObject()) canAttack = true;
            else canAttack = false;*/
        }

        if(PlayerHealth.instance.playerHealth <= 0)
        {
            StopAllCoroutines();
            rocketButton.image.sprite = rocketOff;
            rocketButton.interactable = false;
            triggerButton.interactable = false;
            PlayerHealth.instance.powerSlider.value = 0;
        }


    }

    public void HideRocketButton()
    {
        rocketButton.image.sprite = rocketOff;
    }

    public void ShowRocketButton()
    {
        rocketButton.image.sprite = rocketOn;
    }

    private IEnumerator StopAttacksOnDeath()
    {
        yield return new WaitUntil(() => PlayerHealth.instance.playerHealth <= 0);
        triggerButton.interactable = false;
        canAttack = false;
        StopAllCoroutines();
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
    public IEnumerator ShootBullet()
    {
        canAttack = false;
        //GameObject bullet = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
        GameObject bullet = BulletsManager.Instance.GetPooledObject();
        bullet.GetComponent<Bullet>().damageLevel = bulletDamage;
        bullet.transform.position = firePosition.position;
        bullet.transform.rotation = firePosition.rotation;
        bullet.SetActive(true);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePosition.up * bulletForce, ForceMode2D.Impulse);
        audioSourcePlayer.PlayOneShot(bulletSound);
        Debug.Log("ShootOut");
        currentBullet++;
        yield return new WaitForSeconds(0.2f);
        canAttack = true;
    }

    IEnumerator ShootBlast()
    {
        while(Application.isPlaying)
        {
            yield return new WaitUntil(() => currentBullet > 2);
            yield return new WaitForSeconds(0.5f);
            currentBullet = 0;
        }
    }

    public void ShootBulletOnButton()
    {
        if (enableAttack && canAttack)
        {
            if(currentBullet < 3)
            {
                StartCoroutine(ShootBullet());
            }
        }
    }

    //Set the rocket current status
    public void CheckRocketAvailability()
    {
        if (!disableRocket)
        {
            switch (PlayerHealth.instance.powerSlider.value)
            {
                case 0:
                    //rocketButtonBackground.fillAmount = 0f;
                    rocketLoadButtons[0].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[1].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
                    rocketButton.image.sprite = rocketOff;
                    rocketButton.interactable = false;
                    break;
                case 1:
                    rocketLoadButtons[1].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[0].color = Color.white;
                    rocketButton.image.sprite = rocketOn;
                    //rocketButtonBackground.fillAmount = 0.33f;
                    //PlayerHealth.instance.shoot1.color = Color.HSVToRGB(0, 0, 1);
                    rocketButton.interactable = true;
                    return;
                case 2:
                    rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[1].color = Color.white;
                    //rocketButtonBackground.fillAmount = 0.66f;
                    //PlayerHealth.instance.shoot2.color = Color.HSVToRGB(0, 0, 1);
                    return;
                case 3:
                    rocketLoadButtons[2].color = Color.white;
                    //rocketButtonBackground.fillAmount = 1f;
                    //PlayerHealth.instance.shoot3.color = Color.HSVToRGB(0, 0, 1);
                    return;
            }
        }
    }

    private IEnumerator LoadRocketOff()
    {
        while (Application.isPlaying)
        {
            yield return new WaitUntil(() => PlayerHealth.instance.powerSlider.value == 0 && !disableRocket);
            rocketLoadButtons[0].color = new Color(1, 1, 1, 0);
            rocketLoadButtons[1].color = new Color(1, 1, 1, 0);
            rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
            rocketButton.image.sprite = rocketOff;
            rocketButton.interactable = false;
        }
    }

    private IEnumerator LoadRocket01()
    {
        while (Application.isPlaying)
        {
            yield return new WaitUntil(() => PlayerHealth.instance.powerSlider.value == 1 && !disableRocket);
            rocketLoadButtons[1].color = new Color(1, 1, 1, 0);
            rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
            rocketLoadButtons[0].color = Color.white;
            rocketButton.image.sprite = rocketOn;
            rocketButton.interactable = true;
        }
    }

    private IEnumerator LoadRocket02()
    {
        while (Application.isPlaying)
        {
            yield return new WaitUntil(() => PlayerHealth.instance.powerSlider.value == 2 && !disableRocket);
            rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
            rocketLoadButtons[1].color = Color.white;
        }
    }

    private IEnumerator LoadRocket03()
    {
        while (Application.isPlaying)
        {
            yield return new WaitUntil(() => PlayerHealth.instance.powerSlider.value == 3 && !disableRocket);
            rocketLoadButtons[2].color = Color.white;
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
