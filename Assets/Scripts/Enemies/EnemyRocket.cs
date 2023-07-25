using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRocket : MonoBehaviour
{
    public GameObject enemiesExplosionFx;
    public AudioClip enemyRocketImpactFX;
    public AudioClip playerImpact;
    public AudioClip playerShieldImpact;
    private float timeToHide = 3;
    private float speed = 1.5f;
    private float currentTime = 0;
    private bool shotMissed = true;
    private Vector3 playerPos;

    private void Awake()
    {
        playerPos = new Vector3(fireScript.instance.gameObject.transform.position.x, -5.5f, fireScript.instance.gameObject.transform.position.z);
    }

    private void Start()
    {
        if (gameObject.tag == "Missile")
        {
            timeToHide = 12;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (TutorialManager.instance != null)
            {
                TutorialManager.instance.currentShot--;
                shotMissed = false;
            }
            if (fireScript.instance.gameObject.GetComponent<ShieldController>().shieldActive)
            {
                if (gameObject.tag == "Missile")
                {
                    GameObject hit = Instantiate(enemiesExplosionFx, transform.position, enemiesExplosionFx.transform.rotation);
                    hit.gameObject.GetComponent<AudioSource>().volume = 0.8f;
                    hit.gameObject.GetComponent<AudioSource>().PlayOneShot(enemyRocketImpactFX);
                    Destroy(hit, 1.5f);
                }
                else
                {
                    GameObject hit = Instantiate(enemiesExplosionFx, transform.position, enemiesExplosionFx.transform.rotation);
                    hit.gameObject.GetComponent<AudioSource>().volume = 0.4f;
                    hit.gameObject.GetComponent<AudioSource>().PlayOneShot(playerShieldImpact);
                    Destroy(hit, 1.5f);
                }
            }
            else
            {
                GameObject hit = Instantiate(enemiesExplosionFx, transform.position, enemiesExplosionFx.transform.rotation);
                hit.gameObject.GetComponent<AudioSource>().volume = 0.8f;
                hit.gameObject.GetComponent<AudioSource>().PlayOneShot(enemyRocketImpactFX);
                Destroy(hit, 1.5f);
            }
            Destroy(gameObject);
        }

        if (collision.tag == "Bullet")
        {
            GameObject hit = Instantiate(enemiesExplosionFx, transform.position, enemiesExplosionFx.transform.rotation);
            hit.gameObject.GetComponent<AudioSource>().volume = 0.5f;
            hit.gameObject.GetComponent<AudioSource>().PlayOneShot(playerImpact);
            Destroy(hit, 1.5f);
            Destroy(gameObject);
        }

        if (collision.tag == "Rocket")
        {
            GameObject hit = Instantiate(enemiesExplosionFx, transform.position, enemiesExplosionFx.transform.rotation);
            hit.gameObject.GetComponent<AudioSource>().volume = 0.8f;
            hit.gameObject.GetComponent<AudioSource>().PlayOneShot(playerImpact);
            Destroy(hit, 1.5f);
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        //Delete rocket if no hit
        currentTime += 1 * Time.deltaTime;

        if (currentTime >= timeToHide)
        {
            Destroy(gameObject);
        }

        //Move rocket towards player
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, playerPos, step);
    }
}
