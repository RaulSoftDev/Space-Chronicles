using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    [Header("HEALTH")]
    public float playerHealth = 600;
    private float maxHealth;
    internal bool GetDamage;

    [Header("SLIDER INSTANCE")]
    [SerializeField] private Slider healthSlider;
    internal float playerShieldPoints = 0;
    private float redGradientAlpha = 0.85f;

    [Header("BAR COLOR")]
    [SerializeField] private Image healthColor;

    [Header("ENEMY WEAPONS DAMAGE")]
    [SerializeField] private float enemyBulletValue = 10;
    [SerializeField] private float enemyRocketValue = 30;

    [Header("DEATH EXPLOSION")]
    [SerializeField] private GameObject playerExplosion;
    [SerializeField] private AudioClip deathExplosion;

    //AUDIO SOURCE
    private AudioSource playerAudiosource;

    [Header("RED BACKGROUND")]
    [SerializeField] private Image redBG;
    [SerializeField] private Image redGradient;
    private float desiredDurationBG = 2f;
    private float desiredDuration = 2f;
    private float elapsedTimeBG;
    private float elapsedTime;
    private float percentageCompleteBG;
    private float percentageComplete;
    private bool isLooping = true;
    private Color redColor;

    [Header("DEATH SCREEN INSTANCES")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject pauseButton;

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

        playerHealth = PlayerPrefs.GetInt("PlayerHealth", 600);
        maxHealth = playerHealth;

        SetEnemyDamage();
    }

    private void SetEnemyDamage()
    {
        enemyBulletValue = PlayerPrefs.GetInt("EnemyBulletDamage", 10);
        enemyRocketValue = PlayerPrefs.GetInt("EnemyRocketDamage", 30);
    }

    private void Start()
    {
        healthSlider.maxValue = playerHealth;
        playerAudiosource = GetComponent<AudioSource>();
        GetDamage = true;
        StartCoroutine(PlayerIsDead());
    }

    //Load death scene if player dies
    private IEnumerator PlayerIsDead()
    {
        yield return new WaitUntil(() => playerHealth < 1);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //Set Background On Red
        StartCoroutine(LerpColorOnDeath());
        //If shield active then turn it off
        if (gameObject.GetComponent<ShieldController>().shieldActive)
        {
            gameObject.GetComponent<ShieldController>().TurnOffShield();
        }
        //Begins Explosion Sound
        gameObject.GetComponent<Animator>().SetTrigger("IsPlayerDead");
        playerAudiosource.Stop();
        playerAudiosource.volume = 0.35f;
        playerAudiosource.PlayOneShot(deathExplosion);
        //When first noise ends and hears sound explosion show it
        yield return new WaitForSecondsRealtime(1);
        GameObject explotionInstance = Instantiate(playerExplosion, transform.position, transform.rotation);
        explotionInstance.transform.parent = transform;
        gameObject.GetComponent<Animator>().SetBool("PlayerDisappears", true);
        yield return new WaitForSecondsRealtime(3);
        pauseButton.SetActive(false);
        //Show Death Screen
        deathScreen.SetActive(true);
        //Play Death Menu Music
        SoundScript.instance.gameObject.GetComponent<AudioSource>().volume = 0.40f;
        SoundScript.instance.PlayDeathMusic();
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetEnemyWeaponsDamage(collision, GetDamage);
    }

    private void SetEnemyWeaponsDamage(Collider2D collision, bool enableDamage)
    {
        if (enableDamage)
        {
            if (playerHealth >= 1)
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
                }
            }
        }
    }

    private void Update()
    {
        SetShieldThreshold();
        HealthSliderValues();
        SetRedScreenState();
    }

    private void SetShieldThreshold()
    {
        if (playerShieldPoints < 0)
        {
            playerShieldPoints = 0;
        }

        if (playerShieldPoints > 20)
        {
            playerShieldPoints = 20;
        }
    }

    private void SetRedScreenState()
    {
        if (healthSlider.value < (healthSlider.maxValue * 50) / 100)
        {
            elapsedTimeBG += Time.deltaTime;
            percentageCompleteBG = elapsedTimeBG / desiredDurationBG;
            redBG.color = Color.Lerp(Color.clear, redColor, percentageCompleteBG);
        }
        else if (healthSlider.value < (healthSlider.maxValue * 25) / 100)
        {
            desiredDuration = 1;
        }
        
        if(healthSlider.value <= 0)
        {
            redBG.color = new Color(1, 1, 1, 0.15f);
        }
    }

    //Set health bar values
    private void HealthSliderValues()
    {
        healthSlider.value = playerHealth;
    }

    //Health bar gradually change color from green to red while losing health points
    public void HealthBarColor()
    {
        healthColor.color = Color.Lerp(Color.red, new Color(0, 1, 0.06043053f, 1), healthSlider.value / maxHealth);
        redColor = Color.Lerp(new Color(1, 1, 1, 0.15f), new Color(1, 1, 1, 0.01f), healthSlider.value / maxHealth);
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
        while (true)
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            redBG.color = Color.Lerp(new Color(1, 1, 1, 0.15f), currentColor, percentageComplete);
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
