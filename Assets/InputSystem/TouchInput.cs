// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/TouchInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TouchInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TouchInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TouchInput"",
    ""maps"": [
        {
            ""name"": ""Hold"",
            ""id"": ""bebf8401-5d82-4f3d-a893-2198f1752a37"",
            ""actions"": [
                {
                    ""name"": ""HoldOnTouch"",
                    ""type"": ""Value"",
                    ""id"": ""d693146f-70a2-4985-86c8-0746cc17bf5e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Value"",
                    ""id"": ""4b0d7b18-889a-4ce9-8f3f-63768085fbd7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis Touch"",
                    ""id"": ""8fd07ba1-f333-43f0-9fda-a0be578ff051"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldOnTouch"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""2e42db0f-5969-4de1-beab-54830e52fc21"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldOnTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""44abaf81-959d-448f-bf99-d6b1e5ee0e02"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HoldOnTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9940928a-d43e-455c-bf11-0db2c380fdd7"",
                    ""path"": ""<Touchscreen>/primaryTouch/indirectTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Hold
        m_Hold = asset.FindActionMap("Hold", throwIfNotFound: true);
        m_Hold_HoldOnTouch = m_Hold.FindAction("HoldOnTouch", throwIfNotFound: true);
        m_Hold_Shoot = m_Hold.FindAction("Shoot", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Hold
    private readonly InputActionMap m_Hold;
    private IHoldActions m_HoldActionsCallbackInterface;
    private readonly InputAction m_Hold_HoldOnTouch;
    private readonly InputAction m_Hold_Shoot;
    public struct HoldActions
    {
        private @TouchInput m_Wrapper;
        public HoldActions(@TouchInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @HoldOnTouch => m_Wrapper.m_Hold_HoldOnTouch;
        public InputAction @Shoot => m_Wrapper.m_Hold_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Hold; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HoldActions set) { return set.Get(); }
        public void SetCallbacks(IHoldActions instance)
        {
            if (m_Wrapper.m_HoldActionsCallbackInterface != null)
            {
                @HoldOnTouch.started -= m_Wrapper.m_HoldActionsCallbackInterface.OnHoldOnTouch;
                @HoldOnTouch.performed -= m_Wrapper.m_HoldActionsCallbackInterface.OnHoldOnTouch;
                @HoldOnTouch.canceled -= m_Wrapper.m_HoldActionsCallbackInterface.OnHoldOnTouch;
                @Shoot.started -= m_Wrapper.m_HoldActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_HoldActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_HoldActionsCallbackInterface.OnShoot;
            }
            m_Wrapper.m_HoldActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HoldOnTouch.started += instance.OnHoldOnTouch;
                @HoldOnTouch.performed += instance.OnHoldOnTouch;
                @HoldOnTouch.canceled += instance.OnHoldOnTouch;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
            }
        }
    }
    public HoldActions @Hold => new HoldActions(this);
    public interface IHoldActions
    {
        void OnHoldOnTouch(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
