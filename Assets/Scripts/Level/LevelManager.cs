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
    public GameObject[] squadsLV2;
    public GameObject[] squadsLV3;
    public List<GameObject> runtimeSquads = new List<GameObject>();
    public GameObject inGameMenu;
    public Animator warningSign;
    public Animator[] roundSigns;
    public bool roundSignDone = false;
    public Vector3 startPosition;
    public GameObject dialogue;

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
        StartCoroutine(PlayerHealthCheck());
    }

    private void Update()
    {
        AnimatorStateInfo roundState = roundSigns[round].GetCurrentAnimatorStateInfo(0);
        float roundSignTime = roundState.normalizedTime;

        if(roundState.IsName("EnterRound") && roundSignTime > 1.0f)
        {
            roundSignDone = true;
        }

        foreach (GameObject squad in runtimeSquads)
        {
            foreach(Transform child in squad.transform)
            {
                if(child.childCount < 1)
                {
                    Destroy(child.gameObject);
                }
            }

            if(squad.transform.childCount < 1)
            {
                runtimeSquads.Remove(squad);
            }
        }
    }

    private IEnumerator RoundsManager()
    {
        yield return new WaitUntil(() => Player_Movement.Instance.playerInPos);

        //AI MESSAGE
        dialogue.GetComponent<Animator>().SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogue.GetComponent<DialogueSystem>().isDialogueClosed);

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
        dialogue.GetComponent<Animator>().SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogue.GetComponent<DialogueSystem>().isDialogueClosed);
        MenuScript.Instance.StartMenu(0);
    }

    private IEnumerator GenerateRound()
    {
        //StartCoroutine(CheckEnemiesState());

        fireScript.instance.enableAttack = false;
        roundSignDone = false;

        yield return new WaitForSeconds(2);

        //WARNING SIGN
        warningSign.SetTrigger("SignOn");
        yield return new WaitForSeconds(4);
        warningSign.SetTrigger("SignOff");

        //ROUND 1
        SpawnSquad();

        yield return new WaitUntil(() => currentSquad.transform.position.y <= 5.5f);
        currentSquad.GetComponent<SquadMovementManager>().startMove = false;

        //ROUND SIGN
        roundSigns[round].SetTrigger("RoundOn");
        yield return new WaitUntil(() => roundSignDone);

        //BEGIN FIGHT
        currentSquad.GetComponent<SquadMovementManager>().startMove = true;
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        startPosition = currentSquad.transform.position;
        foreach (Transform child in currentSquad.transform)
        {
            foreach(Transform child2 in child.transform)
            {
                child2.GetComponent<EnemiesAI>().enableAttack = true;
            }
        }

        //ENABLE ATTACKS
        fireScript.instance.enableAttack = true;

        yield return new WaitUntil(() => currentSquad.transform.position.y <= 0 || runtimeSquads.Count == 0);
        SpawnSquad();
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        foreach (Transform child in currentSquad.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                child2.GetComponent<EnemiesAI>().enableAttack = true;
            }
        }

        yield return new WaitUntil(() => currentSquad.transform.position.y <= 0 || runtimeSquads.Count == 0);
        SpawnSquad();
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        foreach (Transform child in currentSquad.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                child2.GetComponent<EnemiesAI>().enableAttack = true;
            }
        }

        yield return new WaitUntil(() => runtimeSquads.Count == 0);
        round++;
    }

    private void SpawnSquad()
    {
        squadNumber++;

        switch (round)
        {
            case 0:
                currentSquad = Instantiate(squads[squadNumber], spawnPoint.transform.position, spawnPoint.transform.rotation);
                runtimeSquads.Add(currentSquad);
                Debug.LogWarning("Spawning Squad");
                currentSquad.GetComponent<SquadMovementManager>().startMove = true;
                break;
            case 1:
                currentSquad = Instantiate(squadsLV2[squadNumber], spawnPoint.transform.position, spawnPoint.transform.rotation);
                runtimeSquads.Add(currentSquad);
                Debug.LogWarning("Spawning Squad");
                currentSquad.GetComponent<SquadMovementManager>().startMove = true;
                break;
            case 2:
                currentSquad = Instantiate(squadsLV3[squadNumber], spawnPoint.transform.position, spawnPoint.transform.rotation);
                runtimeSquads.Add(currentSquad);
                Debug.LogWarning("Spawning Squad");
                currentSquad.GetComponent<SquadMovementManager>().startMove = true;
                break;
            default:
                break;
        }
        /*currentSquad = Instantiate(squads[squadNumber], spawnPoint.transform.position, spawnPoint.transform.rotation);
        runtimeSquads.Add(currentSquad);
        //enemyText.SetActive(true);
        //yield return new WaitForSeconds(3f);
        //enemyText.SetActive(false);
        Debug.LogWarning("Spawning Squad");
        currentSquad.GetComponent<SquadMovementManager>().startMove = true;*/
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
    public void RestartScene()
    {
        BlockUIButtons();
        MenuScript.Instance.RestartPreviousScene();
    }

    private void BlockUIButtons()
    {
        Button[] UIButtons = FindObjectsOfType<Button>();

        foreach (Button button in UIButtons)
        {
            button.interactable = false;
        }
    }

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
        /*foreach (Transform child in inGameMenu.transform)
        {
            if(child.GetComponent<Button>() != null)
            {
                child.GetComponent<Button>().interactable = false;
            }
        }*/
        BlockUIButtons();
        Time.timeScale = 1;
        MenuScript.Instance.MainMenu(0);
    }

    IEnumerator PlayerHealthCheck()
    {
        yield return new WaitUntil(() => PlayerHealth.instance.playerHealth <= 0);
        PlayerHealth.instance.gameObject.GetComponent<Animator>().SetTrigger("IsPlayerDead");
        yield return new WaitForSeconds(1);
        StopAllCoroutines();
    }
}
