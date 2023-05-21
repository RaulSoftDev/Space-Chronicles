using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : Singleton<SwipeDetection>
{
    [SerializeField]
    private float minimunDistance = 0.2f;
    public float minimumTime = 2f;

    private InputManager inputManager;

    public Vector2 startPosition;
    public float startTime;
    private Vector2 endPosition;
    public float endTime;

    public bool touchStart;

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
        touchStart = true;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        touchStart=false;
    }

    private void DetectSwipe()
    {
        Debug.Log("Swipe Detected");
    }
}
