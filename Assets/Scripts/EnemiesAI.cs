using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemiesAI : MonoBehaviour
{


    //Variables Assignment
    //Var Health
    int healthI = 2;
    int healthII = 4;
    int healthIII = 8;
    int healthIV = 16;

    public int currentHealth;

    //Enemies Tags
    private string[] enemiesTags = {"IBasic", "IIMisile", "IIShield", "IIIMisile", "IIIShield", "Boss"};

    //Var Shield
    public int shield = 8;


    //Var Components
    public Animator enemiesAnim;
    AudioSource audioSourceEnemies;

    //AudioClips
    public AudioClip basicShootSound;
    public AudioClip misileSound;
    public AudioClip enemiesDeath;


    //Var Enemies Basic Attack
    public Transform firePositionEnemies;
    
    public GameObject bulletEnemiesPrefab;
    public float bulletForce = 5;
    
    public GameObject misileEnemiesPrefab;
    public float misileForce = 2.5f;

    public GameObject shieldObject;

    public GameObject group;

    //Enemies Booleans
    public bool canAttack;
    bool canBeAttacked = false;
    bool shieldDisable = false;

    //External Scripts
    PlayerHealth playerScript;



    private void Awake()
    {
        switch(gameObject.tag)
        {
            case "IBasic":
                currentHealth = healthI;
                StartCoroutine(WaitingForAimBasicShoot());
                return;
            case "IIMisile":
                currentHealth = healthII;
                StartCoroutine(WaitingForAimMisile());
                return;
            case "IIShield":
                currentHealth = healthII;
                return;
            case "IIIMisile":
                currentHealth = healthIII;
                StartCoroutine(WaitingForAimBasicShoot());
                StartCoroutine(WaitingForAimMisile());
                return;
            case "IIIShield":
                currentHealth = healthIII;
                StartCoroutine(WaitingForAimBasicShoot());
                return;
            case "Boss":
                currentHealth = healthIV;
                StartCoroutine(WaitingForAimBasicShoot());
                StartCoroutine(WaitingForAimMisile());
                return;
        }
    }

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemiesAnim = GetComponent<Animator>();
        audioSourceEnemies = GetComponent<AudioSource>();

        canAttack = true;

        StartCoroutine(EnemyDeathCheck());
    }

    //Enemy health check
    private IEnumerator EnemyDeathCheck()
    {
        while(true)
        {
            yield return new WaitUntil(() => currentHealth <= 0);
            Debug.LogWarning("Enemy death");
            EnemySquadMove.instance.GetComponent<AudioSource>().PlayOneShot(enemiesDeath);
            Destroy(gameObject);
        }
    }

    //Sets up that if the enemy have another enemy under then cannot attack player until this enemy is destroyed
    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.4f), -Vector2.up);

        if(hit.collider == null)
        {
            return;
        }
        else if(enemiesTags.Contains(hit.collider.tag))
        {
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.4f), -Vector2.up, Color.red);
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }
    }

    //Assignment for Aim&Shoot
    IEnumerator WaitingForAimBasicShoot()
    {
        while (true)
        {
            firePositionEnemies.up = (GameObject.FindGameObjectWithTag("Player").transform.position - firePositionEnemies.position) * -1;
            BasicShoot();
            yield return new WaitForSeconds(Random.Range(2, 10));
        }
    }

    //Set the enemy to aim to the player
    IEnumerator WaitingForAimMisile()
    {
        while (true)
        {
            firePositionEnemies.up = (GameObject.FindGameObjectWithTag("Player").transform.position - firePositionEnemies.position) * -1;
            MisileShoot();
            yield return new WaitForSeconds(Random.Range(2, 10));
        }
    }

    //Show up enemy Active Shield animation
    public void ActivateEnemyShieldAnimation()
    {
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOnDamage");
    }

    //Show up enemy Deactive Shield animation
    public void DeactivateEnemyShieldAnimation()
    {
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOff");
        shieldDisable = true;
    }


    //On enable script, sets up current animations coroutines
    private void OnEnable()
    {
        if (shieldObject != null)
        {
            StartCoroutine(shieldWait());
        }
        else
        {
            StartCoroutine(attackWait());
        }
    }

    //Activate shield enemy mode, cannot be attacked
    IEnumerator shieldWait()
    {
        yield return new WaitForSeconds(0.5f);
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOn");
        yield return new WaitForSeconds(0.27f);
        canBeAttacked = true;
    }

    //Sets up that the enemy can be attacked after 0.5f seconds from the beginning
    IEnumerator attackWait()
    {
        yield return new WaitForSeconds(0.5f);
        canBeAttacked = true;
    }

    //Sets the enemy basic shoot instance
    void BasicShoot()
    {
        if (canAttack)
        {
            GameObject bullet = Instantiate(bulletEnemiesPrefab, firePositionEnemies.position, firePositionEnemies.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(-firePositionEnemies.up * bulletForce, ForceMode2D.Impulse);
            //EnemySquadMove.instance.GetComponent<AudioSource>().Stop();
            EnemySquadMove.instance.GetComponent<AudioSource>().PlayOneShot(basicShootSound);
        }
    }

    //Sets the enemy missile shot instance 
    void MisileShoot()
    {
        if (canAttack)
        {
            GameObject misile = Instantiate(misileEnemiesPrefab, firePositionEnemies.position, firePositionEnemies.rotation);
            Rigidbody2D rb = misile.GetComponent<Rigidbody2D>();
            rb.AddForce(-firePositionEnemies.up * misileForce, ForceMode2D.Force);
            //EnemySquadMove.instance.GetComponent<AudioSource>().Stop();
            EnemySquadMove.instance.GetComponent<AudioSource>().PlayOneShot(misileSound);
        }
    }





}
