using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemiesAI : MonoBehaviour
{
    public enum enemyShip
    {
        Basic = 0,
        BasicII = 1,
        Rocket = 2,
        Shield = 3
    }

    public enemyShip enemy;

    //All Enemies Health Set Up
    public int healthI = 20;
    private int healthII = 40;
    private int healthIII = 80;
    private int healthIV = 160;

    //Current Enemy Health
    public int currentHealth;

    //Enemies Tags
    private string[] enemiesTags = {"IBasic", "IIMisile", "IIShield", "IIIMisile", "IIIShield", "Boss"};

    //Shield Value
    internal int shield = 80;


    //Components
    internal Animator enemiesAnim;
    private AudioSource audioSourceEnemies;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip basicShootSound;
    [SerializeField] private AudioClip misileSound;
    [SerializeField] private AudioClip enemiesDeath;

    [Header("SHOT POSITION INSTANCE")]
    [SerializeField] private Transform firePositionEnemies;

    [Header("ENEMY WEAPONS INSTANCES")]
    [SerializeField] private GameObject bulletEnemiesPrefab;
    [SerializeField] private GameObject misileEnemiesPrefab;

    [Header("ENEMY WEAPONS SPEED")]
    [SerializeField] private float bulletForce = 5;
    [SerializeField] private float misileForce = 2.5f;

    [Header("SHIELD")]
    public GameObject shieldObject;

    //public GameObject group;

    [Header("DEATH EXPLOSION")]
    public GameObject deathAnimation;

    //CURRENT POSITION
    private Vector3 currentPosition;

    //Enemies Booleans
    private bool canAttack = false;
    internal bool enableAttack = false;
    bool canBeAttacked = false;
    bool shieldDisable = false;
    internal bool enableCollision = false;
    private bool canMissile = false;
    private bool canBasic = false;
    //public bool isPlayerDead = false;

    //External Scripts
    PlayerHealth playerScript;

    private float rotateSpeed = 2f;
    private float radius = 0.75f;
    private float angle;
    private Vector2 center;
    private float counterZero = 0;
    private float secToAttack;

    private void Awake()
    {
        /*healthI = PlayerPrefs.GetInt("BHealth", 40);
        healthII = PlayerPrefs.GetInt("RocketHealth", 80);
        shield = PlayerPrefs.GetInt("ShieldValue", 60);*/
    }

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemiesAnim = GetComponent<Animator>();
        audioSourceEnemies = GetComponent<AudioSource>();

        //canAttack = true;
        center = transform.localPosition;

        LoadShipsAttacks();

        StartCoroutine(EnemyDeathCheck());
        StartCoroutine(PlayerDead());
    }

    private void LoadShipsAttacks()
    {
        switch (enemy)
        {
            case enemyShip.Basic:
                currentHealth = PlayerPrefs.GetInt("BHealth", 40);
                StartCoroutine(WaitingForAimBasicShoot(2, 8));
                return;
            case enemyShip.BasicII:
                currentHealth = PlayerPrefs.GetInt("BIIHealth", 60);
                StartCoroutine(WaitingForAimBasicShoot(1, 5));
                return;
            case enemyShip.Rocket:
                currentHealth = PlayerPrefs.GetInt("RocketHealth", 80);
                StartCoroutine(WaitingForAimMisile());
                return;
            case enemyShip.Shield:
                currentHealth = PlayerPrefs.GetInt("RocketHealth", 80);
                return;
            default:
                return;
        }
    }

    private void Update()
    {
        if (canAttack)
        {
            BasicShoot();
            MisileShoot();
        }

        currentPosition = transform.position;
    }

    private IEnumerator PlayerDead()
    {
        yield return new WaitUntil(() => PlayerHealth.instance.playerHealth <= 0);
        StopAllCoroutines();
    }

    //Enemy health check
    private IEnumerator EnemyDeathCheck()
    {
        yield return new WaitUntil(() => currentHealth < 1);
        Debug.LogWarning("Enemy death");
        transform.parent = null;
        GameObject explotionClone = Instantiate(deathAnimation, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }

    //Assignment for Aim&Shoot
    IEnumerator WaitingForAimBasicShoot(int rangeA, int rangeB)
    {
        yield return new WaitUntil(() => canAttack);

        while (true)
        {
            yield return new WaitUntil(() => transform.parent.parent.position.y < 5.5);
            yield return new WaitForSeconds(Random.Range(2, 8));
            canBasic = true;
        }
    }

    //Set the enemy to aim to the player
    IEnumerator WaitingForAimMisile()
    {
        yield return new WaitUntil(() => canAttack);

        while (true)
        {
            secToAttack = Random.Range(6, 20);
            yield return new WaitForSeconds(secToAttack);
            firePositionEnemies.up = (GameObject.FindGameObjectWithTag("Player").transform.position - firePositionEnemies.position) * -1;
            Debug.LogError(gameObject.tag);
            //MisileShoot();
            canMissile = true;
        }
    }

    //Sets the enemy basic shoot instance
    void BasicShoot()
    {
        if (enableAttack && canBasic)
        {
            GameObject bullet = Instantiate(bulletEnemiesPrefab, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(-transform.up * bulletForce, ForceMode2D.Impulse);
            audioSourceEnemies.Stop();
            audioSourceEnemies.PlayOneShot(basicShootSound);
            canBasic = false;
        }
    }

    //Sets the enemy missile shot instance 
    void MisileShoot()
    {
        if (canMissile)
        {
            canMissile = false;
            GameObject misile = Instantiate(misileEnemiesPrefab, firePositionEnemies.position, firePositionEnemies.rotation);
            Rigidbody2D rb = misile.GetComponent<Rigidbody2D>();
            rb.AddForce(-firePositionEnemies.up * misileForce, ForceMode2D.Impulse);
            audioSourceEnemies.Stop();
            audioSourceEnemies.PlayOneShot(misileSound);
            Debug.LogError(gameObject.tag);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "CollisionChecker")
        {
            enableCollision = true;
            canAttack = true;
        }

        if(collision.tag == "Player")
        {
            if(PlayerHealth.instance.playerHealth > 0)
            {
                PlayerHealth.instance.playerHealth = 0;
            }
        }
    }





}
