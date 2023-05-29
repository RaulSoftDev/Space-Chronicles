using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class InputManager : Singleton<InputManager>
{
    #region Events
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    #endregion

    private TouchInput touchInput;
    private Camera mainCamera;

    public bool joystickMode = false;
    public Vector2 currentPosition;

    public int FirstFingerTouchID()
    {
        foreach (var touch in Touch.activeTouches)
        {
            // Only respond to first finger
            if (touch.finger.index == 0 && touch.isInProgress)
            {
                return touch.finger.index;
            }
        }

        return -1;
    }

    private void Awake()
    {
        touchInput = new TouchInput();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        touchInput.Enable();
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        touchInput.Disable();
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        touchInput.Touch.PrimaryContact.performed += ctx => StartTouchPrimary(ctx);
        touchInput.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(mainCamera, touchInput.Touch.Primaryposition.ReadValue<Vector2>()), (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(mainCamera, touchInput.Touch.Primaryposition.ReadValue<Vector2>()), (float)context.time);
    }

    public Vector2 PrimaryPosition()
    {
        return Utils.ScreenToWorld(mainCamera, touchInput.Touch.Primaryposition.ReadValue<Vector2>());
    }
}
