using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Asteroid : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _explosionPrefab;
    [Header("Params")]
    [SerializeField] private float _minSize = 0.7f;
    [SerializeField] private float _maxSize = 1.3f;
    [Space]
    [SerializeField] private float _minSpeed = 20f;
    [SerializeField] private float _maxSpeed = 35f;
    [Space]
    [SerializeField] private float _rotationSpeed = 10f;

    private Rigidbody _rigidbody;

    private float _size;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.transform.localScale *= Random.Range(_minSize, _maxSize);
        _rigidbody.velocity = new Vector3(0, 0, -Random.Range(_minSpeed, _maxSpeed));
        _rigidbody.angularVelocity = Random.insideUnitSphere * _rotationSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Asteroid _) || other.TryGetComponent(out Boundary _) || other.TryGetComponent(out PowerUp _))
            return;


        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale *= _size;
        Destroy(gameObject);

        if (other.TryGetComponent(out Enemy _) || other.TryGetComponent(out Lazer _) || other.TryGetComponent(out PowerUp _))
        {
            Destroy(other.gameObject);
        }
    }
}
