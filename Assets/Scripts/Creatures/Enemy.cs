using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Exposion")]
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip[] _explosionSounds;
    [Header("Params")]
    [SerializeField] private float _minSpeed = 2f;
    [SerializeField] private float _maxSpeed = 5f;

    private Rigidbody _rigidbody;

    private float _speed;
    private Vector3 _lastPosition;
    private bool _active;

    public Action<float> Attack;

    public void Damage(int damage)
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySound(_explosionSounds, transform.position);
        Destroy(gameObject);
    }

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

        Player.Instance.Health.Die += Player_Die;
    }

    private void Player_Die()
    {
        _active = false;
        _rigidbody.velocity = Vector3.zero;
    }

    private void Update()
    {
        if (!_active)
        {
            return;
        }

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

    private void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.Health.Die -= Player_Die;
        }
    }
}
