using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] Animator dialogueText;
    [SerializeField] Animator movementArrows;
    [SerializeField] Animator basicAttack;
    [SerializeField] GameObject enemyTestSquad;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] float enemySpeed = 1000;
    public GameObject currentSquad;
    private float maxShots = 5;
    public float currentShot = 0;

    //Checking
    private bool movementDone = false;
    private bool basicAttackDone = false;
    private bool shieldDone = false;
    private bool rocketDone = false;

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

                if (child.transform.childCount < 3)
                {
                    Debug.LogWarning("BA Done");
                    basicAttackDone = true;
                }
            }
        }
    }

    IEnumerator TutorialIntro()
    {
        yield return new WaitUntil(() => Player_Movement.Instance.playerInPos);
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        dialogueText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(LoadEnemies());
        StartCoroutine(TutorialMovement());
    }

    IEnumerator TutorialMovement()
    {
        yield return new WaitUntil(() => currentSquad != null && currentSquad.transform.position.y <= 3.5f);
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
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        basicAttack.SetTrigger("TriggerOn");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
        basicAttack.SetTrigger("TriggerOff");
        fireScript.instance.enableAttack = true;
        dialogueText.gameObject.SetActive(false);
        EnemiesAttackState(true);
        currentSquad.GetComponent<RowManager>().rightMoveLoop = true;
        StartCoroutine(TutorialShield());
    }

    IEnumerator TutorialShield()
    {
        yield return new WaitUntil(() => basicAttackDone);
        EnemiesAttackState(false);
        currentSquad.GetComponent<RowManager>().rightMoveLoop = false;
        dialogueText.gameObject.SetActive(true);
        dialogueText.SetTrigger("OpenDialogue");
        yield return new WaitUntil(() => dialogueText.gameObject.GetComponent<DialogueSystem>().isDialogueClosed);
    }

    IEnumerator LoadEnemies()
    {
        SpawnTestSquad();
        currentSquad.GetComponent<SquadMovementManager>().lerpTime = enemySpeed;
        yield return new WaitUntil(() => currentSquad != null && currentSquad.transform.position.y <= 3.5f);
        currentSquad.GetComponent<SquadMovementManager>().startMove = false;
    }

    private void SpawnTestSquad()
    {
        currentSquad = Instantiate(enemyTestSquad, spawnPoint.transform.position, spawnPoint.transform.rotation);
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
