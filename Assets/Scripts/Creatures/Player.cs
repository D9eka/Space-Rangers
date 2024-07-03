using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _explosionPrefab;
    [Header("Shields")]
    [SerializeField] private GameObject[] _shields;
    [Header("Params")]
    [SerializeField] private float _speed = 50f;
    [SerializeField] private float _tilt = 0.3f;

    private Rigidbody _rigidbody;
    private BoxCollider _collider;

    private int _hp;

    public int HP
    {
        get => _hp;
        private set
        {
            _hp = Math.Clamp(value, -1, _shields.Length);
            if (_hp < 0)
            {
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);

                UI.Instance.ChangeHPText($"Игра завершена! Вы продержались {TimeSpan.FromSeconds(Time.time).ToString("mm':'ss")}");
                return;
            }
            UI.Instance.ChangeHPText($"Энергетических щитов осталось: {_hp}");
            for (int i = 0; i < _shields.Length; i++)
            {
                _shields[i].SetActive(_hp > i);
            }
        }
    }

    public static Player Instance;

    private void Awake()
    {
        Instance = this;

        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
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
        _rigidbody.position = new Vector3(restrictedX, 0, restrictedZ);
        _rigidbody.rotation = Quaternion.Euler(_tilt * _rigidbody.velocity.z, 0, -_rigidbody.velocity.x * _tilt);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Boundary _) || (other.TryGetComponent(out Lazer lazer) && !lazer.IsEnemyLazer))
            return;

        if (other.TryGetComponent(out PowerUp _))
        {
            HP++;
            Destroy(other.gameObject);
            return;
        }

        Destroy(other.gameObject);
        HP--;
    }
}
