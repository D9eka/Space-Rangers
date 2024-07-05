﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour, IDamageable
{
    [Header("Exposion")]
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip[] _explosionSounds;
    [Header("Params")]
    [SerializeField] private float _minSize = 0.7f;
    [SerializeField] private float _maxSize = 1.3f;
    [Space]
    [SerializeField] private float _minSpeed = 20f;
    [SerializeField] private float _maxSpeed = 35f;
    [Space]
    [SerializeField] private float _rotationSpeed = 10f;
    [Space]
    [SerializeField] private int _damage = 1;

    private Rigidbody _rigidbody;

    private float _size;

    public void Damage(int damage)
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale *= _size;
        AudioManager.Instance.PlaySound(_explosionSounds, transform.position);
        Destroy(gameObject);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.transform.localScale *= Random.Range(_minSize, _maxSize);
        _rigidbody.velocity = new Vector3(0, 0, -Random.Range(_minSpeed, _maxSpeed));
        _rigidbody.angularVelocity = Random.insideUnitSphere * _rotationSpeed;

        GameManager.Instance.EndGame += GameManager_EndGame;
        GameManager.Instance.ClearGame += GameManager_ClearGame;

        GameManager.Instance.PauseGame += GameManager_PauseGame;
        GameManager.Instance.ResumeGame += GameManager_ResumeGame;
    }

    private void GameManager_EndGame()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void GameManager_ClearGame()
    {
        Destroy(gameObject);
    }

    private void GameManager_PauseGame()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void GameManager_ResumeGame()
    {
        _rigidbody.velocity = new Vector3(0, 0, -Random.Range(_minSpeed, _maxSpeed));
        _rigidbody.angularVelocity = Random.insideUnitSphere * _rotationSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable) && !IsFriendlyObject(other))
        {
            damageable.Damage(_damage);
            Damage(_damage);
        }
    }

    private bool IsFriendlyObject(Collider other)
    {
        return other.TryGetComponent(out Enemy _) || 
               other.TryGetComponent(out Asteroid _);
    }

    private void OnDestroy()
    {
        GameManager.Instance.EndGame -= GameManager_EndGame;
        GameManager.Instance.ClearGame -= GameManager_ClearGame;

        GameManager.Instance.PauseGame -= GameManager_PauseGame;
        GameManager.Instance.ResumeGame -= GameManager_ResumeGame;
    }
}