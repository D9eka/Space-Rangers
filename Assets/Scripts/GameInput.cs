using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public Action PrimaryAttack;
    public Action AdditionalAttack;

    public static GameInput Instance { get; private set; }

    public Vector2 GetMovementVectorNormalized()
    {
        return _playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }

    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    private void Update()
    {
        OnPrimaryAttack();
        OnAdditionalAttack();
    }

    private void OnPrimaryAttack()
    {
        if (_playerInputActions.Player.PrimaryAttack.ReadValue<float>() > 0f)
        {
            PrimaryAttack?.Invoke();
        }
    }

    private void OnAdditionalAttack()
    {
        if (_playerInputActions.Player.AdditionalAttack.ReadValue<float>() > 0f)
        {
            AdditionalAttack?.Invoke();
        }
    }
}
