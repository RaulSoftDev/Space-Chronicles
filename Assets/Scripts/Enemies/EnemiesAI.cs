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

    //FX Sounds
    [SerializeField] private AudioClip[] enemyExplosionsFX;

    //Components
    internal Animator enemiesAnim;
    private AudioSource audioSourceEnemies;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip basicShootSound;
    [SerializeField] private AudioClip basicIIShotSound;
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

    private GameObject rocketInstance;

    //CURRENT POSITION
    private Vector3 currentPosition;

    //Enemies Booleans
    internal bool canAttack = false;
    internal bool enableAttack = false;
    bool canBeAttacked = false;
    bool shieldDisable = false;
    internal bool enableCollision = false;
    private bool canMissile = false;
    private bool canBasic = false;
    private bool dead = false;
    //public bool isPlayerDead = false;

    //External Scripts
    PlayerHealth playerScript;

    private float rotateSpeed = 2f;
    private float radius = 0.75f;
    private float angle;
    private Vector2 center;
    private float counterZero = 0;
    private float secToAttack;
    private float time = 0;
    private float volume = 1;

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
                StartCoroutine(Shot(5, 10));
                return;
            case enemyShip.BasicII:
                currentHealth = PlayerPrefs.GetInt("BIIHealth", 60);
                StartCoroutine(Shot(3, 8));
                return;
            case enemyShip.Rocket:
                currentHealth = PlayerPrefs.GetInt("RocketHealth", 80);
                StartCoroutine(MisileShoot(7, 16));
                return;
            case enemyShip.Shield:
                currentHealth = PlayerPrefs.GetInt("RocketHealth", 80);
                shield = PlayerPrefs.GetInt("ShieldValue", 100);
                return;
            default:
                return;
        }
    }

    private void Update()
    {
        /*if (canAttack)
        {
            BasicShoot();
            MisileShoot();
        }*/

        //LookAtPlayer();
        HideEnemy();

        currentPosition = transform.position;
    }

    private void LookAtPlayer()
    {
        if(rocketInstance != null)
        {
            Vector3 look = rocketInstance.transform.InverseTransformPoint(fireScript.instance.gameObject.transform.position);
            float Angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90;
            rocketInstance.transform.Rotate(0, 0, Angle);
        }
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
        DeactivateEnemy();
        dead = true;
        StopAllCoroutines();
    }

    private void DeactivateEnemy()
    {
        enableAttack = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        enemiesAnim.SetTrigger("Death");
        if(enemy == enemyShip.Shield)
        {
            shieldObject.SetActive(false);
        }
        LoadCurrentFX();
        Debug.LogWarning("Enemy death");
        transform.parent = null;
        GameObject explotionClone = Instantiate(deathAnimation, transform.position, transform.rotation, transform.parent);
    }

    private void HideEnemy()
    {
        if (dead)
        {
            time += Time.deltaTime;
            if (time > 6)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void LoadCurrentFX()
    {
        switch (enemy)
        {
            case enemyShip.Basic:
                audioSourceEnemies.volume = 0.55f;
                audioSourceEnemies.PlayOneShot(enemyExplosionsFX[0]);
                return;
            case enemyShip.BasicII:
                audioSourceEnemies.volume = 0.60f;
                audioSourceEnemies.PlayOneShot(enemyExplosionsFX[1]);
                return;
            case enemyShip.Rocket:
                audioSourceEnemies.volume = 0.3f;
                audioSourceEnemies.PlayOneShot(enemyExplosionsFX[2]);
                return;
            case enemyShip.Shield:
                audioSourceEnemies.volume = 1;
                audioSourceEnemies.PlayOneShot(enemyExplosionsFX[3]);
                return;
            default:
                audioSourceEnemies.volume = 1;
                audioSourceEnemies.PlayOneShot(enemyExplosionsFX[0]);
                return;
        }
    }

    //Assignment for Aim&Shoot
    IEnumerator WaitingForAimBasicShoot(int rangeA, int rangeB)
    {
        yield return new WaitUntil(() => canAttack);

        while (true)
        {
            yield return new WaitUntil(() => transform.parent.parent.position.y < 5.5);
            yield return new WaitForSeconds(Random.Range(rangeA, rangeB));
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
            GameObject bullet = Instantiate(bulletEnemiesPrefab, new Vector3(transform.localPosition.x, firePositionEnemies.position.y, transform.position.z), transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(-bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            audioSourceEnemies.Stop();

            switch (enemy)
            {
                case enemyShip.Basic:
                    audioSourceEnemies.PlayOneShot(basicShootSound);
                    break;
                case enemyShip.BasicII:
                    audioSourceEnemies.PlayOneShot(basicIIShotSound);
                    break;
                default:
                    audioSourceEnemies.PlayOneShot(basicShootSound);
                    break;
            }
            canBasic = false;
        }
    }

    IEnumerator Shot(int rangeA, int rangeB)
    {
        //yield return new WaitUntil(() => enableAttack && canAttack);
        yield return new WaitForSeconds(Random.Range(rangeA, rangeB));

        float currentPositionX;

        if (transform.parent.transform.parent.GetComponent<RowManager>().left)
        {
            currentPositionX = transform.position.x - 0.15f;
        }
        else
        {
            currentPositionX = transform.position.x + 0.15f;
        }

        if(enableAttack && canAttack)
        {
            GameObject bullet = Instantiate(bulletEnemiesPrefab, new Vector3(currentPositionX, firePositionEnemies.position.y, transform.position.z), transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(-bullet.transform.up * bulletForce, ForceMode2D.Impulse);
            audioSourceEnemies.Stop();

            CurrentBasicVolumes();

            switch (enemy)
            {
                case enemyShip.Basic:
                    audioSourceEnemies.PlayOneShot(basicShootSound);
                    break;
                case enemyShip.BasicII:
                    audioSourceEnemies.PlayOneShot(basicIIShotSound);
                    break;
                default:
                    audioSourceEnemies.PlayOneShot(basicShootSound);
                    break;
            }
        }

        StartCoroutine(Shot(rangeA, rangeB));
    }

    private void CurrentBasicVolumes()
    {
        switch (enemy)
        {
            case enemyShip.Basic:
                volume = 0.5f;
                audioSourceEnemies.volume = volume;
                break;
            case enemyShip.BasicII:
                volume = 0.55f;
                audioSourceEnemies.volume = volume;
                break;
            default:
                break;
        }
    }

    //Sets the enemy missile shot instance 
    IEnumerator MisileShoot(int rangeA, int rangeB)
    {
        yield return new WaitUntil(() => enableAttack && canAttack);
        yield return new WaitForSeconds(Random.Range(rangeA, rangeB));

        canMissile = false;
        rocketInstance = Instantiate(misileEnemiesPrefab, firePositionEnemies.position, firePositionEnemies.rotation);
        Rigidbody2D rb = rocketInstance.GetComponent<Rigidbody2D>();
        audioSourceEnemies.Stop();
        volume = 0.75f;
        audioSourceEnemies.volume = volume;
        audioSourceEnemies.PlayOneShot(misileSound);
        Debug.LogError(gameObject.tag);

        StartCoroutine(MisileShoot(rangeA, rangeB));
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
