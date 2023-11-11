using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : IDisposable
{
    public static InputController Instance;
    public Action<Vector2> OnShotStarted;

    private InputActions _inputActions;

    public InputController()
    {
        _inputActions = new InputActions();
        _inputActions.Enable();
        _inputActions.GunMap.Shot.performed += HandleShotStart;
    }

    private void HandleShotStart(InputAction.CallbackContext context)
    {
        OnShotStarted?.Invoke(_inputActions.GunMap.PointerPosition.ReadValue<Vector2>());
    }

    public void Dispose()
    {
        _inputActions?.Dispose();
    }
}