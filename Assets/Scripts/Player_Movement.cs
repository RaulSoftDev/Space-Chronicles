using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 4;
    [SerializeField] Joystick joystick;
    private Rigidbody2D playerRigidbody;
    private SwipeDetection swipeDetection;
    private InputManager inputManager;
    private float touchXPosition;
    private bool playerInPos = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        swipeDetection = SwipeDetection.Instance;
        inputManager = InputManager.Instance;

        StartCoroutine(SetUpPlayer());
    }

    private void Update()
    {
        touchXPosition = swipeDetection.startPosition.x - inputManager.PrimaryPosition().x;

        if(playerInPos)
        {
            if (!inputManager.joystickMode)
            {
                if (swipeDetection.touchStart && !EventSystem.current.IsPointerOverGameObject())
                {
                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, inputManager.PrimaryPosition().x, Time.deltaTime * playerSpeed), Mathf.Lerp(transform.position.y, inputManager.PrimaryPosition().y, Time.deltaTime * playerSpeed), 0);
                }
            }
            else
            {
                Vector2 direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;

                transform.Translate(direction * playerSpeed * Time.deltaTime);
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.20f, 2.20f), Mathf.Clamp(transform.position.y, -3.2f, 4.5f), transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if(inputManager.joystickMode)
        {
            Vector2 direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;

            transform.Translate(direction * playerSpeed * Time.deltaTime);
        }
    }

    private IEnumerator SetUpPlayer()
    {
        float counter = 0f;
        while (counter < 2)
        {
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, -6, transform.position.z), new Vector3(transform.position.x, -2.5f, transform.position.z), counter / 2);
            counter += Time.deltaTime;
            yield return null;
        }
        playerInPos = true;
    }
}
