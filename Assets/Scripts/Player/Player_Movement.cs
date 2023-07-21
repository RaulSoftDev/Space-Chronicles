using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_Movement : Singleton<Player_Movement>
{
    [Header("SPEED")]
    [SerializeField] private float playerSpeed = 4;
    [Header("PADS")]
    [SerializeField] private Joystick leftPad;
    [SerializeField] private Joystick rightPad;
    [Header("ARROWS SETUP")]
    [SerializeField] private GameObject[] rightArrows;
    [SerializeField] private GameObject[] leftArrows;

    //COMPONENTS AND SCRIPTS
    private Rigidbody2D playerRigidbody;
    private SwipeDetection swipeDetection;
    private InputManager inputManager;

    //BOOLEANS
    internal bool playerInPos = false;
    internal bool enableMovement = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        swipeDetection = SwipeDetection.Instance;
        inputManager = InputManager.Instance;

        enableMovement = false;

        StartCoroutine(SetUpPlayer());
        StartCoroutine(disableArrowsRuntime());
    }

    private void Update()
    {
        //touchXPosition = swipeDetection.startPosition.x - inputManager.PrimaryPosition().x;

        PadsStates();
    }

    private void PadsStates()
    {
        if (enableMovement && playerInPos && PlayerHealth.instance.playerHealth > 0)
        {
            /*if (!inputManager.joystickMode)
            {
                if (swipeDetection.touchStart && !EventSystem.current.IsPointerOverGameObject())
                {
                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, inputManager.PrimaryPosition().x, Time.deltaTime * playerSpeed), transform.position.y, 0);
                }
            }
            else
            {
                
            }*/

            //MOVEMENT
            rightPad.enabled = true;
            leftPad.enabled = true;

            if (rightPad.Horizontal > 0)
            {
                Vector2 directionX = Vector2.right * rightPad.Horizontal;
                Debug.LogWarning(directionX);
                gameObject.transform.Translate(new Vector3(directionX.x, 0, 0) * playerSpeed * Time.deltaTime);

            }

            if (leftPad.Horizontal < 0)
            {
                Vector2 directionX = Vector2.right * leftPad.Horizontal;
                Debug.LogWarning(directionX);
                gameObject.transform.Translate(new Vector3(directionX.x, 0, 0) * playerSpeed * Time.deltaTime);
            }

            //RIGHT PAD
            if (rightPad.Horizontal > 0f)
            {
                rightArrows[0].GetComponent<Image>().color = Color.white;
            }

            if (rightPad.Horizontal > 0.25f)
            {
                rightArrows[1].GetComponent<Image>().color = Color.white;
            }

            if (rightPad.Horizontal > 0.5f)
            {
                rightArrows[2].GetComponent<Image>().color = Color.white;
            }

            if (rightPad.Horizontal > 0.75f)
            {
                rightArrows[3].GetComponent<Image>().color = Color.white;
            }

            if (rightPad.Horizontal < 0.1f)
            {
                rightArrows[0].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            if (rightPad.Horizontal < 0.25f)
            {
                rightArrows[1].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            if (rightPad.Horizontal < 0.5f)
            {
                rightArrows[2].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            if (rightPad.Horizontal < 0.75f)
            {
                rightArrows[3].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            //LEFT PAD
            if (leftPad.Horizontal < 0f)
            {
                leftArrows[0].GetComponent<Image>().color = Color.white;
            }

            if (leftPad.Horizontal < -0.25f)
            {
                leftArrows[1].GetComponent<Image>().color = Color.white;
            }

            if (leftPad.Horizontal < -0.5f)
            {
                leftArrows[2].GetComponent<Image>().color = Color.white;
            }

            if (leftPad.Horizontal < -0.75f)
            {
                leftArrows[3].GetComponent<Image>().color = Color.white;
            }

            if (leftPad.Horizontal > 0.1f)
            {
                leftArrows[0].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            if (leftPad.Horizontal > -0.25f)
            {
                leftArrows[1].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            if (leftPad.Horizontal > -0.5f)
            {
                leftArrows[2].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            if (leftPad.Horizontal > -0.75f)
            {
                leftArrows[3].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.20f, 2.20f), transform.position.y, transform.position.z);
        }
        else
        {
            rightPad.enabled = false;
            leftPad.enabled = false;
        }
    }

    private IEnumerator disableArrowsRuntime()
    {
        while (Application.isPlaying)
        {
            yield return new WaitUntil(() => !swipeDetection.touchStart);
            for (int i = 0; i < leftArrows.Length; i++)
            {
                leftArrows[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                rightArrows[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
            yield return null;
        }
    }

    private IEnumerator SetUpPlayer()
    {
        float counter = 0f;
        while (counter < 7)
        {
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, -6, transform.position.z), new Vector3(transform.position.x, -2.5f, transform.position.z), counter / 7);
            counter += Time.deltaTime;
            yield return null;
        }
        playerInPos = true;
        yield break;
    }
}
