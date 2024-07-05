using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PowerUp : MonoBehaviour
{
    [SerializeField] private int _hp = 1;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _ttlSeconds;

    private Rigidbody _rigidbody;

    private float _ttlCounter;
    private bool _active = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.angularVelocity = Vector3.forward * _rotationSpeed;
        _ttlCounter = _ttlSeconds;

        GameManager.Instance.EndGame += GameManager_EndGame;
        GameManager.Instance.ClearGame += GameManager_ClearGame;

        GameManager.Instance.PauseGame += GameManager_PauseGame;
        GameManager.Instance.ResumeGame += GameManager_ResumeGame;
    }

    private void GameManager_EndGame()
    {
        _active = false;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void GameManager_ClearGame()
    {
        Destroy(gameObject);
    }

    private void GameManager_PauseGame()
    {
        _active = false;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void GameManager_ResumeGame()
    {
        _active = true;
        _rigidbody.angularVelocity = Vector3.forward * _rotationSpeed;
    }

    private void FixedUpdate()
    {
        if (_active) 
        {
            _ttlCounter -= Time.fixedDeltaTime;
            if ( _ttlCounter < 0 )
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.GetPowerUP(_hp);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.EndGame -= GameManager_EndGame;
        GameManager.Instance.ClearGame -= GameManager_ClearGame;

        GameManager.Instance.PauseGame -= GameManager_PauseGame;
        GameManager.Instance.ResumeGame -= GameManager_ResumeGame;
    }
}
