using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField]
    private float minimunDistance = 0.2f;

    private InputManager inputManager;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
        DetectSwipe();
    }

    private void CurrentFingerPosition(Vector2 position, float time)
    {
        //Debug.Log(position + " : " + time);
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        //DetectSwipe();
    }

    private void DetectSwipe()
    {
        Debug.Log("Swipe Detected");
        //Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
    }

    private void Update()
    {
        Debug.Log(inputManager.PrimaryPosition());
    }
}
