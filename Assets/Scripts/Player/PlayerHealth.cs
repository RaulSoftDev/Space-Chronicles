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
    public float hueValue = 124;
    public float saturationValue = 100;
    public float brightnessValue = 100;
    private float redBGAlpha = 0.1f;
    private float redGradientAlpha = 0.85f;

    public Animator healthBar;

    //Health Bar Color
    public Image healthColor;

    public float enemyBulletValue = 1;
    public float enemyRocketValue = 3;

    public GameObject playerExplosion;

    AudioSource playerAudiosource;
    public AudioClip rocketAvailable;
    public AudioClip shieldAvailable;

    //Red Background Variables
    public Image redBG;
    public Image redGradient;
    private float desiredDurationBG = 2f;
    private float desiredDuration = 2f;
    private float elapsedTimeBG;
    private float elapsedTime;
    private float percentageCompleteBG;
    private float percentageComplete;
    private bool restartPercentage = false;
    private bool isLooping = true;
    private bool isPlayerDead = false;
    private Color redColor1;
    private Color redColor2;

    //Death Screen Variables
    public GameObject deathScreen;
    public GameObject pauseButton;

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
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //Load scene
        //MenuScript.Instance.StartMenu(4)
        StartCoroutine(LerpColorOnDeath());
        pauseButton.SetActive(false);
        deathScreen.SetActive(true);
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
        
        if(healthSlider.value < (healthSlider.maxValue * 50) / 100)
        {
            elapsedTimeBG += Time.deltaTime;
            percentageCompleteBG = elapsedTimeBG / desiredDurationBG;
            redBG.color = Color.Lerp(Color.clear, redColor1, percentageCompleteBG);
        }

        if (healthSlider.value < (healthSlider.maxValue * 25) / 100)
        {
            desiredDuration = 1;
        }
    }

    private void HealthSliderValues()
    {
        healthSlider.value = playerHealth;
    }

    public void HealthBarColor()
    {
        healthColor.color = Color.Lerp(Color.red, new Color(0, 1, 0.06043053f, 1), healthSlider.value / 500);
        redColor1 = Color.Lerp(new Color(1, 1, 1, 0.15f), new Color(1, 1, 1, 0.01f), healthSlider.value / 500);
    }

    private void loadPercentage()
    {
        elapsedTime += Time.deltaTime;

        if (restartPercentage)
        {
            elapsedTime = 0;
            restartPercentage = false;
        }
    }

    private void RedColorAnimation()
    {
        float percentageComplete = elapsedTime / desiredDuration;
        Debug.Log(percentageComplete);

        redBG.color = Color.Lerp(Color.clear, Color.white, percentageComplete);
    }

    private IEnumerator LerpColorOn()
    {
        while (percentageComplete < 1)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            redGradient.color = Color.Lerp(Color.clear, new Color(1, 1, 1, redGradientAlpha), percentageComplete);
            yield return null;
        }
        if(playerHealth <= 0)
        {
            yield break;
        }
        elapsedTime = 0;
        percentageComplete = 0;
        StartCoroutine(LerpColorOff());
    }

    private IEnumerator LerpColorOnDeath()
    {
        Color currentColor = redBG.color;
        //Color currentGradientColor = redGradient.color;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            redBG.color = Color.Lerp(new Color(1, 1, 1, 0.15f), currentColor, percentageComplete);
            //redGradient.color = Color.Lerp(new Color(1, 1, 1, 0.85f), currentGradientColor, percentageComplete);
            yield return null;
        }
    }

    private IEnumerator LerpColorOff()
    {
        while (percentageComplete < 1)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            redGradient.color = Color.Lerp(new Color(1, 1, 1, redGradientAlpha), Color.clear, percentageComplete);
            yield return null;
        }
        elapsedTime = 0;
        percentageComplete = 0;
        StartCoroutine(LerpColorOn());
    }

    public void ColorLerpLoop()
    {
        if(healthSlider.value < (healthSlider.maxValue * 20) / 100 && isLooping)
        {
            StartCoroutine(LerpColorOn());
            isLooping = false;
        }
    }
}
