// GENERATED AUTOMATICALLY FROM 'Assets/_GameAssets/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""GunMap"",
            ""id"": ""654696e4-90d1-42f8-a38c-4e7be0abc088"",
            ""actions"": [
                {
                    ""name"": ""Shot"",
                    ""type"": ""Button"",
                    ""id"": ""2c078c80-66bb-4a3e-879e-0d52b9b9e39a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""2b6129ca-eac0-48ce-830a-1bf178140953"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a4efe4c9-da6e-45dc-b301-23cf439c1f3b"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b3b972d7-d8f1-49ee-b52d-d900cfb57c60"",
                    ""path"": ""<Touchscreen>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14d14cb9-210d-4819-b6e3-50bf7fc5d232"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""980f45e7-27fe-4e41-97dc-cdb0441ce258"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GunMap
        m_GunMap = asset.FindActionMap("GunMap", throwIfNotFound: true);
        m_GunMap_Shot = m_GunMap.FindAction("Shot", throwIfNotFound: true);
        m_GunMap_PointerPosition = m_GunMap.FindAction("PointerPosition", throwIfNotFound: true);
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

    // GunMap
    private readonly InputActionMap m_GunMap;
    private IGunMapActions m_GunMapActionsCallbackInterface;
    private readonly InputAction m_GunMap_Shot;
    private readonly InputAction m_GunMap_PointerPosition;
    public struct GunMapActions
    {
        private @InputActions m_Wrapper;
        public GunMapActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shot => m_Wrapper.m_GunMap_Shot;
        public InputAction @PointerPosition => m_Wrapper.m_GunMap_PointerPosition;
        public InputActionMap Get() { return m_Wrapper.m_GunMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GunMapActions set) { return set.Get(); }
        public void SetCallbacks(IGunMapActions instance)
        {
            if (m_Wrapper.m_GunMapActionsCallbackInterface != null)
            {
                @Shot.started -= m_Wrapper.m_GunMapActionsCallbackInterface.OnShot;
                @Shot.performed -= m_Wrapper.m_GunMapActionsCallbackInterface.OnShot;
                @Shot.canceled -= m_Wrapper.m_GunMapActionsCallbackInterface.OnShot;
                @PointerPosition.started -= m_Wrapper.m_GunMapActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_GunMapActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_GunMapActionsCallbackInterface.OnPointerPosition;
            }
            m_Wrapper.m_GunMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shot.started += instance.OnShot;
                @Shot.performed += instance.OnShot;
                @Shot.canceled += instance.OnShot;
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
            }
        }
    }
    public GunMapActions @GunMap => new GunMapActions(this);
    public interface IGunMapActions
    {
        void OnShot(InputAction.CallbackContext context);
        void OnPointerPosition(InputAction.CallbackContext context);
    }
}
