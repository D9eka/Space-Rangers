using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public Action PrimaryAttack;
    public Action AdditionalAttack;
    public Action Pause;

    public Action Interact;
    public Action Resume;

    public static InputManager Instance { get; private set; }

    public Vector2 GetMovementVectorNormalized()
    {
        return _playerInputActions.Player.Move.ReadValue<Vector2>().normalized;
    }

    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        _playerInputActions.UI.Enable();

        GameManager.Instance.StartGame += GameManager_StartGame;
        GameManager.Instance.EndGame += GameManager_EndGame;

        GameManager.Instance.PauseGame += GameManager_PauseGame;
        GameManager.Instance.ResumeGame += GameManager_ResumeGame;
    }

    private void GameManager_StartGame()
    {
        _playerInputActions.UI.Disable();
        _playerInputActions.Player.Enable();
    }

    private void GameManager_EndGame()
    {
        _playerInputActions.UI.Enable();
        _playerInputActions.Player.Disable();
    }

    private void GameManager_PauseGame()
    {
        _playerInputActions.UI.Enable();
        _playerInputActions.Player.Disable();
    }

    private void GameManager_ResumeGame()
    {
        _playerInputActions.UI.Disable();
        _playerInputActions.Player.Enable();
    }

    private void Update()
    {
        if (_playerInputActions.Player.enabled)
        {
            OnPrimaryAttack();
            OnAdditionalAttack();
            OnPause();
        }
        if (_playerInputActions.UI.enabled)
        {
            OnInteract();
            OnResume();
        }
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

    private void OnPause()
    {
        if (_playerInputActions.Player.Pause.ReadValue<float>() > 0f)
        {
            Pause?.Invoke();
        }
    }

    private void OnInteract()
    {
        if (_playerInputActions.UI.Interact.ReadValue<float>() > 0f)
        {
            Interact?.Invoke();
        }
    }

    private void OnResume()
    {
        if (_playerInputActions.UI.Resume.ReadValue<float>() > 0f)
        {
            Resume?.Invoke();
        }
    }
}
