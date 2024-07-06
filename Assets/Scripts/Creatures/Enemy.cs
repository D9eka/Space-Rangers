using Managers;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Creatures
{
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
        private bool _active = true;

        public Action Attack;

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

            GameManager.Instance.EndGame += GameManager_EndGame;
            GameManager.Instance.ClearGame += GameManager_ClearGame;

            GameManager.Instance.PauseGame += GameManager_PauseGame;
            GameManager.Instance.ResumeGame += GameManager_ResumeGame;
        }

        private void GameManager_EndGame()
        {
            _active = false;
        }

        private void GameManager_ClearGame()
        {
            Destroy(gameObject);
        }

        private void GameManager_PauseGame()
        {
            _active = false;
        }

        private void GameManager_ResumeGame()
        {
            _active = true;
        }

        private void Update()
        {
            if (!_active)
            {
                _rigidbody.velocity = Vector3.zero;
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
                OnAttack();
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

        private void OnAttack()
        {
            Attack?.Invoke();
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
