using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private int round = 0;
    private int squadNumber = -1;
    private bool noEnemiesLeft = false;
    public GameObject currentSquad;
    public GameObject enemyText;
    public GameObject spawnPoint;
    public GameObject[] squads;
    public GameObject inGameMenu;

    public GameObject[] enemiesList;

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
        Debug.LogWarning("Round 1");

        //ROUND 1
        StartCoroutine(GenerateRound());

        yield return new WaitUntil(() => round == 1);

        //ROUND 2

        Debug.LogWarning("Round 2");

        noEnemiesLeft = false;
        currentSquad = null;
        squadNumber = -1;

        StartCoroutine(GenerateRound());

        yield return new WaitUntil(() => round == 2);

        //ROUND 3

        Debug.LogWarning("Round 3");

        noEnemiesLeft = false;
        currentSquad = null;
        squadNumber = -1;

        StartCoroutine(GenerateRound());

        yield return new WaitUntil(() => round == 3);

        //VICTORY SCENE

        MenuScript.Instance.StartMenu(3);
    }

    private IEnumerator GenerateRound()
    {
        StartCoroutine(CheckEnemiesState());

        //WARNING TEXT
        yield return new WaitForSeconds(2f);
        enemyText.SetActive(true);
        yield return new WaitForSeconds(2f);
        enemyText.SetActive(false);

        //ROUND 1
        SpawnSquad();

        yield return new WaitUntil(() => currentSquad.transform.position.y <= 3);
        SpawnSquad();

        yield return new WaitUntil(() => currentSquad.transform.position.y <= 3);
        SpawnSquad();

        yield return new WaitUntil(() => noEnemiesLeft);
        round++;
    }

    private void SpawnSquad()
    {
        squadNumber++;

        currentSquad = Instantiate(squads[squadNumber], spawnPoint.transform.position, spawnPoint.transform.rotation);
        //enemyText.SetActive(true);
        //yield return new WaitForSeconds(3f);
        //enemyText.SetActive(false);
        Debug.LogWarning("Spawning Squad");
        currentSquad.GetComponent<SquadMovementManager>().startMove = true;
        //StartCoroutine(EnemyBehaviour.instance.SetUpEnemies(currentSquad.transform));
        //StartCoroutine(EnemyBehaviour.instance.WaitCoroutine(currentSquad.transform));
        //StartCoroutine(CheckForEnemiesOnSquads());
        //yield return new WaitUntil(() => noEnemiesLeft);

        //Destroy(currentSquad);
    }

    private IEnumerator CheckEnemiesState()
    {
        yield return new WaitUntil(() => currentSquad != null);
        while (!noEnemiesLeft)
        {
            enemiesList = GameObject.FindGameObjectsWithTag("IBasic");

            if (enemiesList.Length == 0)
            {
                noEnemiesLeft = true;
            }
            yield return null;
        }
        yield break;
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


    //MENU SETTINGS
    public void OpenInGameMenu()
    {
        Time.timeScale = 0;
        inGameMenu.SetActive(true);
    }

    public void CloseInGameMenu()
    {
        Time.timeScale = 1;
        inGameMenu.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        foreach (Transform child in inGameMenu.transform)
        {
            child.GetComponent<Button>().interactable = false;
        }
        Time.timeScale = 1;
        MenuScript.Instance.MainMenu(0);
    }
}
