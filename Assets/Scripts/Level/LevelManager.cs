using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private int round = -1;
    private bool noEnemiesLeft = true;
    public GameObject currentSquad;
    public GameObject enemyText;
    public GameObject spawnPoint;
    public GameObject[] squads;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        StartCoroutine(RoundsManager());
    }

    private IEnumerator RoundsManager()
    {
        while (round < 3)
        {
            yield return new WaitUntil(() => noEnemiesLeft);
            StartCoroutine(SpawnSquad());
        }

        SceneManager.LoadScene(3);
    }

    private IEnumerator SpawnSquad()
    {
        round++;
        
        noEnemiesLeft = false;
        currentSquad = Instantiate(squads[round], spawnPoint.transform.position, spawnPoint.transform.rotation);
        enemyText.SetActive(true);
        yield return new WaitForSeconds(3f);
        enemyText.SetActive(false);
        Debug.LogWarning("Spawning Squad");
        StartCoroutine(EnemyBehaviour.instance.SetUpEnemies(currentSquad.transform));
        StartCoroutine(EnemyBehaviour.instance.WaitCoroutine(currentSquad.transform));
        StartCoroutine(CheckForEnemiesOnSquads());
        yield return new WaitUntil(() => noEnemiesLeft);

        Destroy(currentSquad);
    }

    private IEnumerator CheckForEnemiesOnSquads()
    {
        yield return new WaitUntil(() => currentSquad != null);
        foreach (Transform child in currentSquad.transform)
        {
            yield return new WaitUntil(() => child.childCount == 0);
        }
        noEnemiesLeft = true;
        Debug.Log("Round Clear");
        EnemyBehaviour.instance.enemyInPosition = false;
    }
}
