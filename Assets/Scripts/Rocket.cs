using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosionFx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogError(collision.name);
        switch (collision.tag)
        {
            case "IBasic":
                //In case of no shield enemy we take 1 enemy life point
                collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= 3;
                collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                break;
            case "IIMisile":
                //In case of no shield enemy we take 1 enemy life point
                collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= 3;
                collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                break;
            case "IIShield":
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
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= 3;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                }
                break;
            case "IIIMisile":
                //In case of no shield enemy we take 1 enemy life point
                collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= 3;
                collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                break;
            case "IIIShield":
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
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= 3;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                }
                break;
            case "Boss":
                //In case of boss enemy we take 1 enemy shield point
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
                    collision.gameObject.GetComponent<EnemiesAI>().currentHealth -= 3;
                    collision.gameObject.GetComponent<EnemiesAI>().enemiesAnim.SetTrigger("DamageOn");
                }
                break;
        }

        //Instantiate explosion on collision
        GameObject hit = Instantiate(explosionFx, transform.position, transform.rotation);
        Destroy(hit, 1.6f);
        Destroy(gameObject);
    }
}
