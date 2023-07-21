using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [SerializeField] Animator dialogueText;
    [SerializeField] Animator movementArrows;
    [SerializeField] Animator basicAttack;
    [SerializeField] GameObject enemyTestSquad;
    [SerializeField] GameObject enemyTestSquad02;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] Animator shieldButton;
    [SerializeField] Animator rocketButton;
    [SerializeField] float enemySpeed = 90;
    [SerializeField] int shieldPointsTutorial = 5;
    [SerializeField] int rocketPointsTutorial = 5;
    public GameObject currentSquad;
    private bool isRocketDisabled = false;
    private float maxShots = 5;
    public float currentShot = 0;

    //Checking
    private bool movementDone = false;
    private bool basicAttackDone = false;
    private bool shieldRocketDone = false;

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

        SetTutorialStats();
    }

    private void Start()
    {
        StartCoroutine(TutorialIntro());
    }

    private void Update()
    {
        if(currentSquad != null)
        {
            foreach (Transform child in currentSquad.transform)
            {
                Debug.Log(child.transform.childCount);

                if (child.transform.childCount < 1)
                {
                    if (!basicAttackDone)
                    {
                        Debug.LogWarning("BA Done");
                        basicAttackDone = true;
                        if (currentSquad != null)
                        {
                            Destroy(currentSquad);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("SR Done");
                        shieldRocketDone = true;
                        if (currentSquad != null)
                        {
                            Destroy(currentSquad);
                        }
                    }
                }
            }
        }

        if (isRocketDisabled)
        {
            Debug.Log("Disable");
            fireScript.instance.disableRocket = true;
            fireScript.instance.HideRocketButton();
        }
        else
        {
            fireScript.instance.disableRocket = false;
        }
    }

    private void SetTutorialStats()
    {
        PlayerPrefs.SetInt("ShieldPoints", shieldPointsTutorial);
        PlayerPrefs.SetInt("RocketPoints", rocketPointsTutorial);
        PlayerPrefs.SetInt("BHealth", 30);
    }

    IEnumerator TutorialIntro()
    {
        yield return new WaitUntil(() => Player_Movement.Instance.playerInPos);
        DialogueIndex.Instance.SetDialogue(DialogueIndex.Dialogue.Tutorial_Intro);
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        dialogueText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(LoadEnemies(enemyTestSquad));
        StartCoroutine(TutorialMovement());
    }

    IEnumerator TutorialMovement()
    {
        yield return new WaitUntil(() => currentSquad != null && currentSquad.transform.position.y <= 4.5f);
        DialogueIndex.Instance.SetDialogue(DialogueIndex.Dialogue.Tutorial_Movement);
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        movementArrows.SetTrigger("ArrowsOn");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        dialogueText.gameObject.SetActive(false);
        movementArrows.SetTrigger("ArrowsOff");
        movementArrows.enabled = false;
        Player_Movement.Instance.enableMovement = true;
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        EnemiesAttackState(true);
        StartCoroutine(TutorialBasicAttack());
    }

    IEnumerator TutorialBasicAttack()
    {
        yield return new WaitUntil(() => currentShot >= maxShots);
        EnemiesAttackState(false);
        currentSquad.GetComponent<RowManager>().rightMoveLoop = false;
        DialogueIndex.Instance.SetDialogue(DialogueIndex.Dialogue.Tutorial_Attack);
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        basicAttack.SetTrigger("TriggerOn");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        basicAttack.SetTrigger("TriggerOff");
        fireScript.instance.enableAttack = true;
        dialogueText.gameObject.SetActive(false);
        EnemiesAttackState(true);
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        StartCoroutine(TutorialShieldRocket());
    }

    IEnumerator TutorialShieldRocket()
    {
        yield return new WaitUntil(() => basicAttackDone);
        //Shield Slider 1/3
        //EnemiesAttackState(false);
        //currentSquad.GetComponent<RowManager>().rightMoveLoop = false;
        //Shield button animation On
        shieldButton.SetTrigger("TriggerOn");
        //Show dialogue
        DialogueIndex.Instance.SetDialogue(DialogueIndex.Dialogue.Tutorial_Defence);
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        dialogueText.gameObject.SetActive(false);
        //Shield button animation Off
        shieldButton.SetTrigger("TriggerOff");
        //Rocket button animation On
        isRocketDisabled = true;
        rocketButton.SetTrigger("TriggerOn");
        DialogueIndex.Instance.SetDialogue(DialogueIndex.Dialogue.Tutorial_Rocket);
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        dialogueText.gameObject.SetActive(false);
        //Rocket button animation Off
        rocketButton.SetTrigger("TriggerOff");
        isRocketDisabled = false;
        fireScript.instance.enableAttack = true;
        StartCoroutine(LoadEnemies(enemyTestSquad02));
        EnemiesAttackState(true);
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        StartCoroutine(TutorialEnd());
    }

    IEnumerator TutorialEnd()
    {
        yield return new WaitUntil(() => shieldRocketDone);

        DialogueIndex.Instance.SetDialogue(DialogueIndex.Dialogue.Tutorial_End);
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        //Exit to main menu
        bool skipLogo = true;
        PlayerPrefs.SetInt("SkipLogo", (skipLogo ? 1 : 0));
        MenuScript.Instance.MainMenu(1);
    }

    IEnumerator LoadEnemies(GameObject squad)
    {
        SpawnTestSquad(squad);
        currentSquad.GetComponent<SquadMovementManager>().lerpTime = enemySpeed;
        yield return new WaitUntil(() => currentSquad != null && currentSquad.transform.position.y <= 4.5f);
        currentSquad.GetComponent<SquadMovementManager>().startMove = false;
    }

    private void SpawnTestSquad(GameObject squad)
    {
        currentSquad = Instantiate(squad, spawnPoint.transform.position, spawnPoint.transform.rotation);
        Debug.LogWarning("Spawning Squad");
        currentSquad.GetComponent<SquadMovementManager>().startMove = true;
    }

    private void EnemiesAttackState(bool attack)
    {
        foreach (Transform child in currentSquad.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                child2.GetComponent<EnemiesAI>().enableAttack = attack;
            }
        }
    }


}
