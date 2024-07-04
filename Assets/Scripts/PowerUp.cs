using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PowerUp : MonoBehaviour
{
    [SerializeField] private int _hp = 1;
    [SerializeField] private float _rotationSpeed = 5f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.angularVelocity = Vector3.forward * _rotationSpeed;

        Player.Instance.Health.Die += Player_Die;
    }

    private void Player_Die()
    {
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.GetPowerUP(_hp);
            Player.Instance.Health.Die -= Player_Die;
            Destroy(gameObject);
        }
    }
}
