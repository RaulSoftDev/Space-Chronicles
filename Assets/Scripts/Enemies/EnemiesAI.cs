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
    private AudioSource audioSourceEnemies;

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

    public GameObject deathAnimation;

    //Enemies Booleans
    public bool canAttack;
    bool canBeAttacked = false;
    bool shieldDisable = false;

    //External Scripts
    PlayerHealth playerScript;

    private float rotateSpeed = 2f;
    private float radius = 0.75f;
    private float angle;
    private Vector2 center;
    private float counterZero = 0;

    private void Start()
    {
        LoadShipsAttacks();

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        enemiesAnim = GetComponent<Animator>();
        audioSourceEnemies = GetComponent<AudioSource>();

        canAttack = true;
        center = transform.localPosition;

        StartCoroutine(EnemyDeathCheck());
    }

    private void LoadShipsAttacks()
    {
        switch (gameObject.tag)
        {
            case "IBasic":
                currentHealth = healthI;
                StartCoroutine(WaitingForAimBasicShoot());
                //StartCoroutine(basicMovement());
                //StartCoroutine(basicEnemiesRotation(transform));
                return;
            case "IIMisile":
                currentHealth = healthII;
                StartCoroutine(WaitingForAimMisile());
                //StartCoroutine(basicMovement());
                return;
            case "IIShield":
                currentHealth = healthII;
                //StartCoroutine(WaitingForAimBasicShoot());
                //StartCoroutine(basicMovement());
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

    //Enemy health check
    private IEnumerator EnemyDeathCheck()
    {
        yield return new WaitUntil(() => currentHealth < 1);
        Debug.LogWarning("Enemy death");
        transform.parent = null;
        GameObject explotionClone = Instantiate(deathAnimation, transform.position, transform.rotation);
        //EnemySquadMove.instance.GetComponent<AudioSource>().PlayOneShot(enemiesDeath);
        Destroy(gameObject);
    }

    private IEnumerator basicEnemiesRotation(Transform t)
    {
        while(Application.isPlaying)
        {
            yield return new WaitUntil(() => gameObject.tag == "IBasic");
            angle += rotateSpeed * Time.deltaTime;
            var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            t.localPosition = new Vector2(transform.parent.GetChild(3).transform.localPosition.x, transform.parent.GetChild(3).transform.localPosition.y) + offset;
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

    private IEnumerator basicMovement()
    {
        Vector3 position1 = new Vector3(transform.localPosition.x, transform.localPosition.y - 1, transform.localPosition.z);
        Vector3 position2 = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);

        yield return new WaitUntil(() => LevelManager.instance.currentSquad.transform.position.y <= 2.2f);
        while (Application.isPlaying)
        {
            float counter = 0f;
            while (counter < 2)
            {
                transform.localPosition = Vector3.Lerp(position2, position1, counter / 2);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
            counter = 0f;
            while (counter < 2)
            {
                transform.localPosition = Vector3.Lerp(position1, position2, counter / 2);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    //To be implemented later
    private IEnumerator circleMovement()
    {
        Vector3 localOriginalPosition = transform.localPosition;
        while (Application.isPlaying)
        {
            float counter = 0f;
            while(counter < 2)
            {
                transform.localPosition = Vector3.Slerp(new Vector3(localOriginalPosition.x, localOriginalPosition.y, localOriginalPosition.z), new Vector3(localOriginalPosition.x, -0.15f, localOriginalPosition.z), counter / 2);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
            counter = 0;
            while (counter < 2)
            {
                transform.localPosition = Vector3.Slerp(new Vector3(localOriginalPosition.x, -0.15f, localOriginalPosition.z), new Vector3(localOriginalPosition.x, localOriginalPosition.y, localOriginalPosition.z), counter / 2);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    //Assignment for Aim&Shoot
    IEnumerator WaitingForAimBasicShoot()
    {
        while (true)
        {
            yield return new WaitUntil(() => EnemyBehaviour.instance.enemyInPosition);
            //firePositionEnemies.up = (GameObject.FindGameObjectWithTag("Player").transform.position - firePositionEnemies.position) * -1;
            yield return new WaitForSeconds(Random.Range(1, 8));
            BasicShoot();
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

    //Show up enemy Active Shield damage animation
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
        /*shieldObject.GetComponent<Animator>().SetTrigger("ShieldOn");
        yield return new WaitForSeconds(0.27f);*/
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
            GameObject bullet = Instantiate(bulletEnemiesPrefab, new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(-transform.up * bulletForce, ForceMode2D.Impulse);
            audioSourceEnemies.Stop();
            audioSourceEnemies.PlayOneShot(basicShootSound);
        }
    }

    //Sets the enemy missile shot instance 
    void MisileShoot()
    {
        if (canAttack)
        {
            GameObject misile = Instantiate(misileEnemiesPrefab, firePositionEnemies.position, firePositionEnemies.rotation);
            Rigidbody2D rb = misile.GetComponent<Rigidbody2D>();
            rb.AddForce(-firePositionEnemies.up * misileForce, ForceMode2D.Impulse);
            audioSourceEnemies.Stop();
            audioSourceEnemies.PlayOneShot(misileSound);
        }
    }





}
