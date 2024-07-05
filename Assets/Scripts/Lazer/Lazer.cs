using System;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public enum LazerType
    {
        Player,
        Enemy
    }

    [SerializeField] private int _damage = 1;
    [SerializeField] private LazerType _type;

    private Rigidbody _rigidbody;

    private Vector3 _velocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameManager.Instance.EndGame += GameManager_EndGame;
        GameManager.Instance.ClearGame += GameManager_ClearGame;

        GameManager.Instance.PauseGame += GameManager_PauseGame;
        GameManager.Instance.ResumeGame += GameManager_ResumeGame;
    }

    private void GameManager_EndGame()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void GameManager_ClearGame()
    {
        Destroy(gameObject);
    }

    private void GameManager_PauseGame()
    {
        _velocity = _rigidbody.velocity;
        _rigidbody.velocity = Vector3.zero;
    }

    private void GameManager_ResumeGame()
    {
        _rigidbody.velocity = _velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable) && CanDamage(other))
        {
            damageable.Damage(_damage);
            Destroy(gameObject);
        }
    }

    private bool CanDamage(Collider other)
    {
        return _type switch
        {
            LazerType.Player => !other.TryGetComponent(out Player _),
            LazerType.Enemy => !other.TryGetComponent(out Enemy _),
            _ => throw new NotImplementedException(),
        };
    }

    private void OnDestroy()
    {
        GameManager.Instance.EndGame -= GameManager_EndGame;
        GameManager.Instance.ClearGame -= GameManager_ClearGame;

        GameManager.Instance.PauseGame -= GameManager_PauseGame;
        GameManager.Instance.ResumeGame -= GameManager_ResumeGame;
    }
}
