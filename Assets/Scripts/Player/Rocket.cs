using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionFx;
    public int rocketDamage = 30;

    private void Start()
    {
        StartCoroutine(DestroyOnSeconds());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= rocketDamage;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                    Destroy(gameObject);
                }
                break;
            case "IIBasic":
                if (collision.gameObject.GetComponent<EnemiesAI>().enableCollision)
                {
                    //Instantiate explosion on collision
                    hit = Instantiate(explosionFx, transform.position, transform.rotation);
                    Destroy(hit, 1.6f);
                    //In case of no shield enemy we take 1 enemy life point
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= rocketDamage;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                    Destroy(gameObject);
                }
                break;
            case "IIMisile":
                if (collision.gameObject.GetComponent<EnemiesAI>().enableCollision)
                {
                    //Instantiate explosion on collision
                    hit = Instantiate(explosionFx, transform.position, transform.rotation);
                    Destroy(hit, 1.6f);
                    //In case of no shield enemy we take 1 enemy life point
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= rocketDamage;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                    Destroy(gameObject);
                }
                break;
            case "IIShield":
                if (collision.gameObject.GetComponent<EnemiesAI>().enableCollision)
                {
                    //Instantiate explosion on collision
                    hit = Instantiate(explosionFx, transform.position, transform.rotation);
                    Destroy(hit, 1.6f);
                    //In case of shield enemy we take 1 enemy shield point
                    collision.gameObject.GetComponent<EnemiesAI>().shield -= rocketDamage;
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
                        collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= rocketDamage;
                        collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                    }
                    Destroy(gameObject);
                }
                break;
            case "IIIMisile":
                //Instantiate explosion on collision
                hit = Instantiate(explosionFx, transform.position, transform.rotation);
                Destroy(hit, 1.6f);
                //In case of no shield enemy we take 1 enemy life point
                collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= rocketDamage;
                collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                Destroy(gameObject);
                break;
            case "IIIShield":
                //Instantiate explosion on collision
                hit = Instantiate(explosionFx, transform.position, transform.rotation);
                Destroy(hit, 1.6f);
                //In case of shield enemy we take 1 enemy shield point
                collision.gameObject.GetComponent<EnemiesAI>().shield -= 3;
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
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= rocketDamage;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                }
                Destroy(gameObject);
                break;
            case "Boss":
                //Instantiate explosion on collision
                hit = Instantiate(explosionFx, transform.position, transform.rotation);
                Destroy(hit, 1.6f);
                //In case of boss enemy we take 1 enemy shield point
                collision.gameObject.GetComponent<EnemiesAI>().shield -= rocketDamage;
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
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= rocketDamage;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                }
                Destroy(gameObject);
                break;
        }

    }

    private IEnumerator DestroyOnSeconds()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
