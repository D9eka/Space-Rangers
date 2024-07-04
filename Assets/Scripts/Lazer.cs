using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private bool _isEnemyShot;

    private Rigidbody _rigidbody;

    public bool IsEnemyLazer => _isEnemyShot;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Player.Instance.Health.Die += Player_Die;
    }

    private void Player_Die()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            if ((!_isEnemyShot && other.TryGetComponent(out Player _)) || (_isEnemyShot && other.TryGetComponent(out Enemy _)))
            {
                return;
            }
            damageable.Damage(_damage);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Player.Instance)
        {
            Player.Instance.Health.Die -= Player_Die;
        }
    }
}
