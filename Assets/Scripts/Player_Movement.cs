using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public float playerSpeed = 4;
    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * playerSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            playerRigidbody.AddForce(Vector2.right * playerSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerRigidbody.AddForce(-Vector2.right * playerSpeed);
        }

    }
}
