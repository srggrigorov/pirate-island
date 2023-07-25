using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    public Action<Vector2> OnShotStarted;

    private InputActions _inputActions;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
        _inputActions = new InputActions();
    }

    private void HandleShotStart(InputAction.CallbackContext context)
    {
        OnShotStarted?.Invoke(_inputActions.GunMap.PointerPosition.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.GunMap.Shot.performed += HandleShotStart;
    }

    private void OnDisable()
    {
        _inputActions.GunMap.Shot.performed -= HandleShotStart;
        _inputActions.Disable();
    }
}