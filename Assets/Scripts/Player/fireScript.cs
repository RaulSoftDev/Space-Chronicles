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

    [Header("AUDIO SOURCE")]
    AudioSource audioSourcePlayer;

    [Header("PLAYER SHOT POSITION")]
    [SerializeField] private Transform firePosition;

    [Header("WEAPONS INSTANCES")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject rocketPrefab;

    [Header("WEAPONS SPEED")]
    [SerializeField] private float bulletForce = 5;
    [SerializeField] private float rocketForce = 5;

    [Header("WEAPONS DAMAGE")]
    public int bulletDamage = 10;
    public int rocketDamage = 30;

    [Header("WEAPONS IMAGES")]
    [SerializeField] private Image rocketButtonBackground;
    [SerializeField] internal Button rocketButton;
    [SerializeField] private Image[] rocketLoadButtons;
    [SerializeField] private Sprite rocketOn;
    [SerializeField] private Sprite rocketOff;
    [SerializeField] public Button triggerButton;

    [Header("ROCKET SLIDER")]
    public Slider powerSlider;
    internal int playerPoints = 0;

    [Header("POINTS PER SHOT")]
    [SerializeField] public int shieldPointsPerShot = 1;
    [SerializeField] public int rocketPointsPerShot = 1;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip bulletSound;
    [SerializeField] private AudioClip rocketSound;
    [SerializeField] private AudioClip activateRocket;

    //Bullet count
    private float currentBullet = 0;

    [Header("BOOLEANS")]
    public bool canAttack = false;
    public bool enableAttack = false;
    public bool disableRocket = false;

    //Input Manager
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

        SetPlayerPoints();

        StartCoroutine(ShootBlast());
        LoadRocketSlots();
        StartCoroutine(DisableAttacksOnDeath());

        canAttack = true;
    }

    private void SetPlayerPoints()
    {
        /*PlayerPrefs.SetInt("ShieldPoints", shieldPointsPerShot);
        PlayerPrefs.SetInt("RocketPoints", rocketPointsPerShot);*/
        shieldPointsPerShot = PlayerPrefs.GetInt("ShieldPoints");
        rocketPointsPerShot = PlayerPrefs.GetInt("RocketPoints");
        Debug.Log("POINTS SAVED");
    }


    private void Update()
    {
        SetSliderValue();
        SetRocketThreshold();
        OnPlayerDeath();
    }

    private void OnPlayerDeath()
    {
        if (PlayerHealth.instance.playerHealth <= 0)
        {
            StopAllCoroutines();
            rocketButton.image.sprite = rocketOff;
            rocketButton.interactable = false;
            triggerButton.interactable = false;
            powerSlider.value = 0;
        }
    }

    private void SetRocketThreshold()
    {
        if (playerPoints < 0)
        {
            playerPoints = 0;
        }

        if (playerPoints > 30)
        {
            playerPoints = 30;
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

    private IEnumerator DisableAttacksOnDeath()
    {
        yield return new WaitUntil(() => PlayerHealth.instance.playerHealth <= 0);
        triggerButton.interactable = false;
        canAttack = false;
        StopAllCoroutines();
    }


    //Set the player health slider current value
    private void SetSliderValue()
    {
        powerSlider.value = playerPoints / 10;
    }

    //Instantiate the basic bullet
    private IEnumerator ShootBullet()
    {
        canAttack = false;
        GameObject bullet = BulletsManager.Instance.GetPooledObject();
        bullet.GetComponent<Bullet>().damageLevel = bulletDamage;
        bullet.transform.position = firePosition.position;
        bullet.transform.rotation = firePosition.rotation;
        bullet.SetActive(true);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePosition.up * bulletForce, ForceMode2D.Impulse);
        audioSourcePlayer.volume = 0.30f;
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

    //Set the rocket button current status
    /*public void CheckRocketAvailability()
    {
        if (!disableRocket)
        {
            switch (powerSlider.value)
            {
                case 0:
                    //Rocket Button Slots Empty
                    rocketLoadButtons[0].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[1].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
                    rocketButton.image.sprite = rocketOff;
                    rocketButton.interactable = false;
                    break;
                case 1:
                    //Rocket Button Slot 1 Available
                    rocketLoadButtons[1].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[0].color = Color.white;
                    rocketButton.image.sprite = rocketOn;
                    rocketButton.interactable = true;
                    return;
                case 2:
                    //Rocket Button Slot 2 Available
                    rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
                    rocketLoadButtons[1].color = Color.white;
                    return;
                case 3:
                    //Rocket Button Slot 3 Available
                    rocketLoadButtons[2].color = Color.white;
                    return;
            }
        }
    }*/

    private IEnumerator UnloadRocket()
    {
        yield return new WaitUntil(() => powerSlider.value == 0 && !disableRocket);
        rocketLoadButtons[0].color = new Color(1, 1, 1, 0);
        rocketLoadButtons[1].color = new Color(1, 1, 1, 0);
        rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
        rocketButton.image.sprite = rocketOff;
        rocketButton.interactable = false;
        StartCoroutine(UnloadRocket());
    }

    private void LoadRocketSlots()
    {
        StartCoroutine(UnloadRocket());
        StartCoroutine(LoadRocket01());
        StartCoroutine(LoadRocket02());
        StartCoroutine(LoadRocket03());
    }

    private IEnumerator LoadRocket01()
    {
        yield return new WaitUntil(() => powerSlider.value == 1 && !disableRocket);
        rocketLoadButtons[1].color = new Color(1, 1, 1, 0);
        rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
        rocketLoadButtons[0].color = Color.white;
        audioSourcePlayer.volume = 1f;
        audioSourcePlayer.PlayOneShot(activateRocket);
        rocketButton.image.sprite = rocketOn;
        rocketButton.interactable = true;
        yield return new WaitUntil(() => powerSlider.value != 1 || disableRocket);
        StartCoroutine(LoadRocket01());
    }

    private IEnumerator LoadRocket02()
    {
        yield return new WaitUntil(() => powerSlider.value == 2 && !disableRocket);
        rocketLoadButtons[2].color = new Color(1, 1, 1, 0);
        rocketLoadButtons[1].color = Color.white;
        audioSourcePlayer.volume = 1f;
        audioSourcePlayer.PlayOneShot(activateRocket);
        yield return new WaitUntil(() => powerSlider.value != 2 || disableRocket);
        StartCoroutine(LoadRocket02());
    }

    private IEnumerator LoadRocket03()
    {
        yield return new WaitUntil(() => powerSlider.value == 3 && !disableRocket);
        rocketLoadButtons[2].color = Color.white;
        audioSourcePlayer.volume = 1f;
        audioSourcePlayer.PlayOneShot(activateRocket);
        yield return new WaitUntil(() => powerSlider.value != 3 || disableRocket);
        StartCoroutine(LoadRocket03());
    }

    public void RocketManagement()
    {
        switch (powerSlider.value)
        {
            case 1:
                LaunchRocket();
                playerPoints = 0;
                break;
            case 2:
                LaunchRocket();
                playerPoints = 10;
                break;
            case 3:
                LaunchRocket();
                playerPoints = 20;
                break;
        }
    }

    //Instantiate the rocket shot
    void LaunchRocket()
    {
        GameObject rocket = Instantiate(rocketPrefab, firePosition.position, firePosition.rotation);
        rocket.GetComponent<Rocket>().rocketDamage = rocketDamage;
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        rb.AddForce(firePosition.up * bulletForce, ForceMode2D.Impulse);
        audioSourcePlayer.volume = 1f;
        audioSourcePlayer.PlayOneShot(rocketSound);
    }
}
