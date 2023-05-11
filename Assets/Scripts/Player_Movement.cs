using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public float playerSpeed = 4;
    private Rigidbody2D playerRigidbody;
    private SwipeDetection swipeDetection;
    private InputManager inputManager;
    private float touchXPosition;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        swipeDetection = SwipeDetection.Instance;
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        touchXPosition = swipeDetection.startPosition.x - inputManager.PrimaryPosition().x;

        if (swipeDetection.touchStart)
        {
            transform.position += new Vector3(inputManager.PrimaryPosition().x, 0, 0) * playerSpeed * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //var movement = Input.GetAxis("Horizontal");

        /*if (swipeDetection.touchStart && touchXPosition < swipeDetection.startPosition.x)
        {
            playerRigidbody.AddForce(Vector2.right * playerSpeed);
        }

        if (swipeDetection.touchStart && touchXPosition > swipeDetection.startPosition.x)
        {
            playerRigidbody.AddForce(-Vector2.right * playerSpeed);
        }*/

    }
}
