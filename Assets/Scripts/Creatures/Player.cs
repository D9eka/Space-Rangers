using Components;
using Managers;
using UnityEngine;

namespace Creatures
{
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
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private float _speed = 50f;
        [SerializeField] private float _tilt = 0.3f;

        private Rigidbody _rigidbody;
        private BoxCollider _collider;

        private const int MIN_HP = -1;

        public HealthComponent Health { get; private set; }

        public static Player Instance;

        public void Damage(int damage = 1)
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
        }

        private void Start()
        {
            Health.ChangeHP += Health_ChangeHP;
            Health.Die += Health_Die;

            GameManager.Instance.StartGame += GameManager_StartGame;
            GameManager.Instance.ClearGame += GameManager_ClearGame;

            gameObject.SetActive(false);
        }

        private void GameManager_StartGame()
        {
            gameObject.SetActive(true);
        }

        private void GameManager_ClearGame()
        {
            transform.position = _initialPosition;
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
            gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector2 movementVector = InputManager.Instance.GetMovementVectorNormalized();
            _rigidbody.velocity = _speed * new Vector3(movementVector.x, 0f, movementVector.y);
            float restrictedX = Mathf.Clamp(_rigidbody.position.x, LevelManager.Instance.LeftBorder + _collider.size.x / 2, 
                                                                   LevelManager.Instance.RightBorder - _collider.size.x / 2);
            float restrictedZ = Mathf.Clamp(_rigidbody.position.z, LevelManager.Instance.BottomBorder + _collider.size.z / 2, 
                                                                   LevelManager.Instance.TopBorder - _collider.size.z / 2);
            _rigidbody.position = new Vector3(restrictedX, _rigidbody.position.y, restrictedZ);
            _rigidbody.rotation = Quaternion.Euler(_tilt * _rigidbody.velocity.z, _rigidbody.rotation.y, -_rigidbody.velocity.x * _tilt);
        }
    }
}
