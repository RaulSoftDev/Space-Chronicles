using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesBullet : MonoBehaviour
{

    public GameObject enemiesExplosionFx;

    private void Start()
    {
        StartCoroutine(DeleteBulletOnTime());
    }

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

    }

    private IEnumerator DeleteBulletOnTime()
    {
        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }

}
