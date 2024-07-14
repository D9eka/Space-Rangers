using Creatures;
using Managers;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Rigidbody))]
    public class Asteroid : MonoBehaviour, IDamageable
    {
        [Header("Exposion")]
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private AudioClip[] _explosionSounds;
        [Header("Params")]
        [SerializeField] private float _rotationSpeed = 10f;
        [Space]
        [SerializeField] private int _damage = 1;

        private Rigidbody _rigidbody;

        private float _velocity;
        private float _size;

        public void Damage(int damage = 1)
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
            DifficultySO _currentDifficulty = DifficultyManager.Instance.CurrentDifficulty;
            _velocity = Random.Range(_currentDifficulty.AsteroidSpeedRange.Item1, _currentDifficulty.AsteroidSpeedRange.Item2);
            _size = Random.Range(_currentDifficulty.AsteroidScaleRange.Item1, _currentDifficulty.AsteroidScaleRange.Item2);

            _rigidbody.transform.localScale *= _size;
            _rigidbody.velocity = new Vector3(0, 0, -_velocity);
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
            _rigidbody.velocity = new Vector3(0, 0, -_velocity);
            _rigidbody.angularVelocity = Random.insideUnitSphere * _rotationSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable) && !IsFriendlyObject(other))
            {
                damageable.Damage(_damage);
                Damage();
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
}