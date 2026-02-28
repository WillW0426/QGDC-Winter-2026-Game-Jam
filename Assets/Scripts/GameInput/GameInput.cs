using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    PlayerInputActions playerInputActions;

    public event EventHandler OnPlayerJump;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        playerInputActions.Player.Jump.performed += Jump_performed;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        OnPlayerJump?.Invoke(this, EventArgs.Empty);
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    /*
     * Returns the normalized movement input from the player. This is useful for ensuring consistent movement speed regardless of the input magnitude.
    */
    public Vector2 GetMovementNormalized()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }
}
