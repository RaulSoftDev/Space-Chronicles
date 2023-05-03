using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMovement : MonoBehaviour
{
    public float squadSpeed = 4;
    public Vector3 pos1 = new Vector3(-1, 3, 0);
    public Vector3 pos2 = new Vector3(1, 3, 0);

    public GameObject[] prefablist;
    public float gridX = 5f;
    public float gridY = 5f;
    public float spacingX = 2f;
    public float spacingY = 2f;

    GameObject[] enemiesBasic;
    GameObject[] enemiesIIShield;
    GameObject[] enemiesIIMisile;
    GameObject[] enemiesIIIShield;
    GameObject[] enemiesIIIMisile;
    GameObject[] enemiesBoss;

    void SpawnEnemies()
    {
        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                Vector3 pos = new Vector3(x * spacingX, y * spacingY, 0);
                Instantiate(prefablist[Random.Range(0, 5)], pos, Quaternion.identity);
            }
        }
    }

    void SpawnPosition()
    {
        enemiesBasic = GameObject.FindGameObjectsWithTag("IBasic");
        enemiesIIShield = GameObject.FindGameObjectsWithTag("IIShield");
        enemiesIIMisile = GameObject.FindGameObjectsWithTag("IIMisile");
        enemiesIIIShield = GameObject.FindGameObjectsWithTag("IIIShield");
        enemiesIIIMisile = GameObject.FindGameObjectsWithTag("IIIMisile");
        enemiesBoss = GameObject.FindGameObjectsWithTag("Boss");

        for (int i = 0; i < enemiesBasic.Length; i++)
        {
            enemiesBasic[i].transform.SetParent(transform, false);
        }

        for (int i = 0; i < enemiesIIShield.Length; i++)
        {
            enemiesIIShield[i].transform.SetParent(transform, false);
        }

        for (int i = 0; i < enemiesIIMisile.Length; i++)
        {
            enemiesIIMisile[i].transform.SetParent(transform, false);
        }

        for (int i = 0; i < enemiesIIIShield.Length; i++)
        {
            enemiesIIIShield[i].transform.SetParent(transform, false);
        }

        for (int i = 0; i < enemiesIIIMisile.Length; i++)
        {
            enemiesIIIMisile[i].transform.SetParent(transform, false);
        }

        for (int i = 0; i < enemiesBoss.Length; i++)
        {
            enemiesBoss[i].transform.SetParent(transform, false);
        }
    }

    private void Start()
    {
        SpawnEnemies();
        SpawnPosition();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp (pos1, pos2, Mathf.PingPong(Time.time * squadSpeed, 1.0f));
    }
}
