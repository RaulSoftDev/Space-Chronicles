using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private int round = 0;
    [SerializeField] float enemySpeedBegins = 60;
    [SerializeField] float enemySpeed = 150;
    private int squadNumber = -1;
    private bool noEnemiesLeft = false;
    public GameObject currentSquad;
    public GameObject enemyText;
    public GameObject spawnPoint;
    public GameObject[] squads;
    public GameObject[] squadsLV2;
    public GameObject[] squadsLV3;
    public List<GameObject> runtimeSquads = new List<GameObject>();
    public Animator warningSign;
    public Animator[] roundSigns;
    public bool roundSignDone = false;
    public Vector3 startPosition;
    public GameObject dialogue;
    private bool levelAlreadyFinished = false;
    private bool unlockNext = false;
    private float yPosition = 0;

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

        SetEnemySpeed();
    }

    private void SetEnemySpeed()
    {
        enemySpeed = PlayerPrefs.GetInt("EnemySpeed", 150);
    }

    private void Start()
    {
        //Player cannot attack until Round 1
        fireScript.instance.enableAttack = false;

        StartCoroutine(RoundsManager());
        StartCoroutine(PlayerHealthCheck());

        //SaveLevelData();
    }

    private void Update()
    {
        AnimatorStateInfo roundState = roundSigns[round].GetCurrentAnimatorStateInfo(0);
        float roundSignTime = roundState.normalizedTime;

        if(roundState.IsName("EnterRound") && roundSignTime > 1.0f)
        {
            roundSignDone = true;
        }

        foreach (GameObject squad in runtimeSquads.ToList())
        {
            foreach (Transform child in squad.transform)
            {
                if (child.childCount < 1)
                {
                    Destroy(child.gameObject);
                }
            }

            if (squad.transform.childCount < 1)
            {
                runtimeSquads.Remove(squad);
            }
        }
    }

    private void ShowDialogue()
    {
        dialogue.gameObject.SetActive(true);
        dialogue.GetComponent<Animator>().SetTrigger("OpenDialogue");
    }

    private IEnumerator RoundsManager()
    {
        yield return new WaitUntil(() => Player_Movement.Instance.playerInPos);

        Player_Movement.Instance.enableMovement = true;

        //AI MESSAGE
        DialogueIndex.Instance.SetDialogue(DialogueIndex.Dialogue.System_1_Intro);
        ShowDialogue();
        yield return new WaitUntil(() => dialogue.GetComponent<DialogueSystem>().isDialogueClosed);
        dialogue.GetComponent<DialogueSystem>().isDialogueClosed = false;
        dialogue.gameObject.SetActive(false);

        Debug.LogWarning("Round 1");

        //ROUND 1
        StartCoroutine(GenerateRound());

        yield return new WaitUntil(() => round == 1);

        //Player cannot attack until next round
        fireScript.instance.enableAttack = false;

        //ROUND 2
        Debug.LogWarning("Round 2");

        noEnemiesLeft = false;
        currentSquad = null;
        squadNumber = -1;

        StartCoroutine(GenerateRound());

        yield return new WaitUntil(() => round == 2);

        //Player cannot attack until next round
        fireScript.instance.enableAttack = false;

        //ROUND 3

        Debug.LogWarning("Round 3");

        noEnemiesLeft = false;
        currentSquad = null;
        squadNumber = -1;

        StartCoroutine(GenerateRound());

        yield return new WaitUntil(() => round == 3);

        //Player cannot attack, end of mission
        fireScript.instance.enableAttack = false;

        //VICTORY SCENE
        DialogueIndex.Instance.SetDialogue(DialogueIndex.Dialogue.System_1_Outtro);
        ShowDialogue();
        dialogue.GetComponent<Animator>().SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogue.GetComponent<DialogueSystem>().isDialogueClosed);
        dialogue.GetComponent<DialogueSystem>().isDialogueClosed = false;
        dialogue.gameObject.SetActive(false);

        SaveLevelData();
    }

    private void SaveLevelData()
    {
        //UNLOCK NEXT DIFFICULTY
        levelAlreadyFinished = true;
        CheckDifficultyState();
        bool skipLogo = true;
        PlayerPrefs.SetInt("SkipLogo", skipLogo ? 1 : 0);
        MenuScript.Instance.StartMenu(1);
    }

    private void CheckDifficultyState()
    {
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 0:
                if(PlayerPrefs.HasKey("PussycatDone") && PlayerPrefs.GetInt("PussycatDone") == 1)
                {
                    unlockNext = false;
                    PlayerPrefs.SetInt("UnlockLevel", unlockNext ? 1 : 0);
                    Debug.Log("PUSSYCAT ALREADY DONE");
                }
                else
                {
                    unlockNext = true;
                    PlayerPrefs.SetInt("UnlockLevel", unlockNext ? 1 : 0);
                    PlayerPrefs.SetInt("PussycatDone", levelAlreadyFinished ? 1 : 0);
                }
                break;
            case 1:
                if (PlayerPrefs.HasKey("AverageDone") && PlayerPrefs.GetInt("AverageDone") == 1)
                {
                    unlockNext = false;
                    PlayerPrefs.SetInt("UnlockLevel", unlockNext ? 1 : 0);
                    Debug.Log("AVERAGE ALREADY DONE");
                }
                else
                {
                    unlockNext = true;
                    PlayerPrefs.SetInt("UnlockLevel", unlockNext ? 1 : 0);
                    PlayerPrefs.SetInt("AverageDone", levelAlreadyFinished ? 1 : 0);
                    Debug.Log("FIRST TIME");
                }
                break;
            case 2:
                if (PlayerPrefs.HasKey("FoolishDone") && PlayerPrefs.GetInt("FoolishDone") == 1)
                {
                    unlockNext = false;
                    PlayerPrefs.SetInt("UnlockLevel", unlockNext ? 1 : 0);
                }
                else
                {
                    unlockNext = true;
                    PlayerPrefs.SetInt("UnlockLevel", unlockNext ? 1 : 0);
                    PlayerPrefs.SetInt("FoolishDone", levelAlreadyFinished ? 1 : 0);
                }
                break;
            case 3:
                unlockNext = false;
                PlayerPrefs.SetInt("UnlockLevel", unlockNext ? 1 : 0);
                break;
        }
    }

    private IEnumerator GenerateRound()
    {
        //StartCoroutine(CheckEnemiesState());
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
        startPosition = currentSquad.transform.position;
        RuntimeEnemySpeed();
        currentSquad.GetComponent<SquadMovementManager>().startMove = true;
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        foreach (Transform child in currentSquad.transform)
        {
            foreach(Transform child2 in child.transform)
            {
                child2.GetComponent<EnemiesAI>().enableAttack = true;
            }
        }

        //ENABLE ATTACKS
        fireScript.instance.enableAttack = true;

        yield return new WaitUntil(() => currentSquad.transform.position.y <= yPosition || runtimeSquads.Count == 0);
        SpawnSquad();
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        foreach (Transform child in currentSquad.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                child2.GetComponent<EnemiesAI>().enableAttack = true;
            }
        }

        yield return new WaitUntil(() => currentSquad.transform.position.y <= yPosition || runtimeSquads.Count == 0);
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

    private void RuntimeEnemySpeed()
    {
        currentSquad.GetComponent<SquadMovementManager>().startPos = startPosition;
        currentSquad.GetComponent<SquadMovementManager>().lerpTime = enemySpeed;
    }

    private void SpawnSquad()
    {
        squadNumber++;

        switch (round)
        {
            case 0:
                GetSquad(squads, squadNumber);
                break;
            case 1:
                GetSquad(squadsLV2, squadNumber);
                break;
            case 2:
                yPosition = -1;
                GetSquad(squadsLV3, squadNumber);
                break;
            default:
                break;
        }
    }

    private void GetSquad(GameObject[] squad, int squadNumber)
    {
        currentSquad = Instantiate(squad[squadNumber], spawnPoint.transform.position, spawnPoint.transform.rotation);
        runtimeSquads.Add(currentSquad);
        Debug.LogWarning("Spawning Squad");
        if (squadNumber == 0)
        {
            currentSquad.GetComponent<SquadMovementManager>().startMove = true;
            currentSquad.GetComponent<SquadMovementManager>().lerpTime = enemySpeedBegins;
        }
        else
        {
            currentSquad.GetComponent<SquadMovementManager>().startMove = true;
            currentSquad.GetComponent<SquadMovementManager>().lerpTime = enemySpeed;
        }
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

    IEnumerator PlayerHealthCheck()
    {
        yield return new WaitUntil(() => PlayerHealth.instance.playerHealth <= 0);
        PlayerHealth.instance.gameObject.GetComponent<Animator>().SetTrigger("IsPlayerDead");
        yield return new WaitForSeconds(1);
        StopAllCoroutines();
    }
}
