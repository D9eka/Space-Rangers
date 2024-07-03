using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject _explosionPrefab;
    [Header("Params")]
    [SerializeField] private float _minSpeed = 2f;
    [SerializeField] private float _maxSpeed = 5f;

    private Rigidbody _rigidbody;

    private float _speed;
    private Vector3 _lastPosition;

    public Action<float> Attack;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _speed = Random.Range(_minSpeed, _maxSpeed);
        _rigidbody.velocity = new Vector3(0, 0, -_speed);
        _rigidbody.transform.rotation = Quaternion.Euler(0, 180, 0);
        _lastPosition = _rigidbody.transform.position;
    }

    private void Update()
    {
        if (Player.Instance == null)
        {
            _rigidbody.velocity = new Vector3(0, 0, -_speed);
            _rigidbody.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            Vector3 playerDirection = Player.Instance.transform.position - _rigidbody.transform.position;
            MoveTo(playerDirection);
            OnAttack(playerDirection);
        }
        _lastPosition = _rigidbody.transform.position;
    }

    private void MoveTo(Vector3 direction)
    {
        Vector3 currentDirection = _rigidbody.transform.position - _lastPosition;
        float angle = Vector3.SignedAngle(direction, currentDirection, Vector3.up);
        _rigidbody.transform.Rotate(0, -angle, 0);
        _rigidbody.velocity = direction / direction.magnitude * _speed;
    }

    private void OnAttack(Vector3 playerDirection)
    {
        float angle = Vector3.SignedAngle(Vector3.back, playerDirection, Vector3.up);
        Attack?.Invoke(angle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Boundary _) || (other.TryGetComponent(out Lazer lazer) && lazer.IsEnemyLazer) || 
            other.TryGetComponent(out PowerUp _) || other.TryGetComponent(out Asteroid _))
            return;

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);

        if (lazer != null && lazer.IsEnemyLazer)
            Destroy(other.gameObject);
    }
}
