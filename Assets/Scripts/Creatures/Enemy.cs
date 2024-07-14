using Generators;
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

        private Rigidbody _rigidbody;

        private float _speed;

        private bool _active = true;

        public void Damage(int damage = 1)
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
            DifficultySO currentDifficulty = DifficultyManager.Instance.CurrentDifficulty;
            _speed = Random.Range(currentDifficulty.EnemySpeedRange.Item1, currentDifficulty.EnemySpeedRange.Item2);

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
                MoveToTarget(Player.Instance.transform);
            }
        }

        private void MoveToTarget(Transform target)
        {
            Vector3 direction = target.position - transform.position;
            _rigidbody.velocity = direction.normalized * _speed;
            transform.LookAt(target);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.Damage();
                Damage();
            }
        }

        private void OnDestroy()
        {
            GameManager.Instance.EndGame -= GameManager_EndGame;
            GameManager.Instance.ClearGame -= GameManager_ClearGame;

            GameManager.Instance.PauseGame -= GameManager_PauseGame;
            GameManager.Instance.ResumeGame -= GameManager_ResumeGame;

            ObjectsGenerator.Instance.ReduceEnemyCounter();
        }
    }
}
