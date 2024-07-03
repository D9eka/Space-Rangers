using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _rigidbody.angularVelocity = Vector3.forward * _rotationSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Destroy(gameObject);
    }
}
