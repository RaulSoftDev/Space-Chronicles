using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Bullet : MonoBehaviour
{

    public GameObject explosionFx;
    private float timeToHide = 1.75f;
    private float currentTime = 0;
    public int damageLevel = 10;
    public int shieldPoints = 1;
    public int rocketPoints = 1;

    private void Start()
    {
        LoadPlayerPoints();
    }

    private void LoadPlayerPoints()
    {
        shieldPoints = PlayerPrefs.GetInt("ShieldPoints");
        rocketPoints = PlayerPrefs.GetInt("RocketPoints");
    }

    private void OnEnable()
    {
        currentTime = 0;
    }

    //Check if the bullet have collision on the enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Explosion on hit
        GameObject hit;

        switch (collision.tag)
        {
            case "IBasic":
                if (collision.gameObject.GetComponent<EnemiesAI>().enableCollision)
                {
                    //Instantiate explosion on collision
                    hit = Instantiate(explosionFx, transform.position, transform.rotation);
                    Destroy(hit, 1.6f);
                    //In case of no shield enemy we take 1 enemy life point
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= damageLevel;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                    fireScript.instance.playerPoints += rocketPoints;
                    PlayerHealth.instance.playerShieldPoints += shieldPoints;
                    //Destroy(gameObject);
                    gameObject.SetActive(false);
                }
                break;
            case "IIBasic":
                if (collision.gameObject.GetComponent<EnemiesAI>().enableCollision)
                {
                    //Instantiate explosion on collision
                    hit = Instantiate(explosionFx, transform.position, transform.rotation);
                    Destroy(hit, 1.6f);
                    //In case of no shield enemy we take 1 enemy life point
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= damageLevel;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                    fireScript.instance.playerPoints += rocketPoints;
                    PlayerHealth.instance.playerShieldPoints += shieldPoints;
                    //Destroy(gameObject);
                    gameObject.SetActive(false);
                }
                break;
            case "IIMisile":
                if (collision.gameObject.GetComponent<EnemiesAI>().enableCollision)
                {
                    //Instantiate explosion on collision
                    hit = Instantiate(explosionFx, transform.position, transform.rotation);
                    Destroy(hit, 1.6f);
                    //In case of no shield enemy we take 1 enemy life point
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= damageLevel;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                    fireScript.instance.playerPoints += rocketPoints;
                    PlayerHealth.instance.playerShieldPoints += shieldPoints;
                    //Destroy(gameObject);
                    gameObject.SetActive(false);
                }
                break;
            case "IIShield":
                if (collision.gameObject.GetComponent<EnemiesAI>().enableCollision)
                {
                    //Instantiate explosion on collision
                    hit = Instantiate(explosionFx, transform.position, transform.rotation);
                    Destroy(hit, 1.6f);
                    fireScript.instance.playerPoints += rocketPoints;
                    PlayerHealth.instance.playerShieldPoints += shieldPoints;
                    CheckEnemiesShield(collision);
                    gameObject.SetActive(false);
                }
                break;
            case "IIIMisile":
                //Instantiate explosion on collision
                hit = Instantiate(explosionFx, transform.position, transform.rotation);
                Destroy(hit, 1.6f);
                //In case of no shield enemy we take 1 enemy life point
                collision.gameObject.GetComponent<EnemiesAI>().currentHealth--;
                collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                fireScript.instance.playerPoints += rocketPoints;
                PlayerHealth.instance.playerShieldPoints += shieldPoints;
                Destroy(gameObject);
                break;
            case "IIIShield":
                //Instantiate explosion on collision
                hit = Instantiate(explosionFx, transform.position, transform.rotation);
                Destroy(hit, 1.6f);
                //In case of shield enemy we take 1 enemy shield point
                collision.gameObject.GetComponent<EnemiesAI>().shield--;
                fireScript.instance.playerPoints += rocketPoints;
                PlayerHealth.instance.playerShieldPoints += shieldPoints;
                //Then we check for shield points and activate the animation needed
                if (collision.gameObject.GetComponent<EnemiesAI>().shield > 0)
                {
                    collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().SetTrigger("ShieldOnDamage");
                }
                else if (collision.gameObject.GetComponent<EnemiesAI>().shield == 0)
                {
                    collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().SetTrigger("ShieldOff");
                }
                else
                {
                    //Once it has no more shield points start to take life points
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= damageLevel;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                }
                Destroy(gameObject);
                break;
            case "Boss":
                //Instantiate explosion on collision
                hit = Instantiate(explosionFx, transform.position, transform.rotation);
                Destroy(hit, 1.6f);
                //In case of boss enemy we take 1 enemy shield point
                collision.gameObject.GetComponent<EnemiesAI>().shield--;
                fireScript.instance.playerPoints += rocketPoints;
                PlayerHealth.instance.playerShieldPoints += shieldPoints;
                //Then we check for shield points and activate the animation needed
                if (collision.gameObject.GetComponent<EnemiesAI>().shield > 0)
                {
                    collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().SetTrigger("ShieldOnDamage");
                }
                else if (collision.gameObject.GetComponent<EnemiesAI>().shield == 0)
                {
                    collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().SetTrigger("ShieldOff");
                }
                else
                {
                    //Once it has no more shield points start to take life points
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= damageLevel;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                }
                Destroy(gameObject);
                break;
            case "BulletIBasic":
                hit = Instantiate(explosionFx, transform.position, transform.rotation);
                Destroy(hit, 1.6f);
                gameObject.SetActive(false);
                break;
            case "Missile":
                hit = Instantiate(explosionFx, transform.position, transform.rotation);
                Destroy(hit, 1.6f);
                gameObject.SetActive(false);
                break;
        }

        //Destroy(gameObject);
    }

    private void Update()
    {
        currentTime += 1 * Time.deltaTime;

        if(currentTime >= timeToHide)
        {
            gameObject.SetActive(false);
        }

        /*if(transform.position.x > 5)
        {
            Debug.LogWarning("Disabled");
            GetComponent<BoxCollider2D>().enabled = false;
        }*/
    }

    private void CheckEnemiesShield(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemiesAI>().shield > 0)
        {
            if (collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ActivateShield") && collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().GetBool("ShieldOn") == true)
            {
                collision.gameObject.GetComponent<EnemiesAI>().shield -= damageLevel;
                collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().SetTrigger("ShieldOnDamage");
            }
            else
            {
                Debug.LogError("ShieldOn");
                collision.gameObject.GetComponent<EnemiesAI>().shield -= damageLevel;
                collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().SetBool("ShieldOn", true);
            }
        }
        else if (collision.gameObject.GetComponent<EnemiesAI>().shield <= 0)
        {
            if (collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("TurnOffShield") && collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0)
            {
                //Once it has no more shield points start to take life points
                collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= damageLevel;
                collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
            }
            else
            {
                collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= damageLevel;
                collision.gameObject.GetComponent<EnemiesAI>().shieldObject.GetComponent<Animator>().SetTrigger("ShieldOff");
            }
        }
    }

    private IEnumerator DestroyOnSeconds()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Turn off");
        gameObject.SetActive(false);
    }





}
