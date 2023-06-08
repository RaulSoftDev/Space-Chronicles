using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesBullet : MonoBehaviour
{

    public GameObject enemiesExplosionFx;
    private float timeToHide = 3;
    private float currentTime = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameObject hit = Instantiate(enemiesExplosionFx, transform.position, transform.rotation);
            Destroy(hit, 1.5f);
            Destroy(gameObject);
        }

        if (collision.tag == "Bullet")
        {
            GameObject hit = Instantiate(enemiesExplosionFx, transform.position, transform.rotation);
            Destroy(hit, 1.5f);
            Destroy(gameObject);
        }

        if (collision.tag == "Rocket")
        {
            GameObject hit = Instantiate(enemiesExplosionFx, transform.position, transform.rotation);
            Destroy(hit, 1.5f);
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        currentTime += 1 * Time.deltaTime;

        if (currentTime >= timeToHide)
        {
            Destroy(gameObject);
        }
    }
}
