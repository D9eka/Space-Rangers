using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Exposion")]
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip[] _explosionSounds;
    [Header("Shields")]
    [SerializeField] private GameObject[] _shields;
    [SerializeField] private int _initialActiveShields = 0;
    [Header("Params")]
    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _tilt = 0.3f;

    private Rigidbody _rigidbody;
    private BoxCollider _collider;

    private const int MIN_HP = -1;

    public HealthComponent Health { get; private set; }

    public static Player Instance;

    public void Damage(int damage)
    {
        Health.HP -= damage;
    }

    public void GetPowerUP(int hp)
    {
        Health.HP += hp;
    }

    private void Awake()
    {
        Instance = this;

        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();

        Health = new HealthComponent(_initialActiveShields, MIN_HP, _shields.Length);
        Health.ChangeHP += Health_ChangeHP;
        Health.Die += Health_Die;
    }

    private void Health_ChangeHP(int hp)
    {
        for (int i = 0; i < _shields.Length; i++)
        {
            _shields[i].SetActive(hp > i);
        }
    }

    private void Health_Die()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySound(_explosionSounds, transform.position);
        Destroy(gameObject);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 movementVector = GameInput.Instance.GetMovementVectorNormalized();
        _rigidbody.velocity = _speed * new Vector3(movementVector.x, 0f, movementVector.y);
        float restrictedX = Mathf.Clamp(_rigidbody.position.x, LevelController.Instance.LeftBorder + _collider.size.x / 2, 
                                                               LevelController.Instance.RightBorder - _collider.size.x / 2);
        float restrictedZ = Mathf.Clamp(_rigidbody.position.z, LevelController.Instance.BottomBorder + _collider.size.z / 2, 
                                                               LevelController.Instance.TopBorder - _collider.size.z / 2);
        _rigidbody.position = new Vector3(restrictedX, _rigidbody.position.y, restrictedZ);
        _rigidbody.rotation = Quaternion.Euler(_tilt * _rigidbody.velocity.z, _rigidbody.rotation.y, -_rigidbody.velocity.x * _tilt);
    }
}
