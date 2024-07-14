using Creatures;
using Managers;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lazers
{
    public class LazerGun : MonoBehaviour
    {
        public enum LazerGunType
        {
            Primary,
            Additional,
            Enemy
        }

        [Header("Prefabs")]
        [SerializeField] private GameObject _lazerPrefab;
        [SerializeField] private AudioClip[] _sounds;
        [Header("Params")]
        [SerializeField] private LazerGunType _type;
        [SerializeField] private float _delay;
        [SerializeField] private float _lazerVelocity;

        private bool _delayed;

        private void Start()
        {
            GameManager.Instance.StartGame += GameManager_StartGame;
            GameManager.Instance.EndGame += GameManager_EndGame;

            GameManager.Instance.PauseGame += GameManager_PauseGame;
            GameManager.Instance.ResumeGame += GameManager_ResumeGame;

            switch (_type) 
            { 
                case LazerGunType.Primary:
                    InputManager.Instance.PrimaryAttack += GameInput_PrimaryAttack;
                    break;
                case LazerGunType.Additional:
                    InputManager.Instance.AdditionalAttack += GameInput_AdditionalAttack;
                    break;
                case LazerGunType.Enemy:
                    DifficultySO difficulty = DifficultyManager.Instance.CurrentDifficulty;
                    _delay = difficulty.EnemyAttackDelay;
                    _lazerVelocity = Random.Range(difficulty.EnemyAttackSpeedRange.Item1, difficulty.EnemyAttackSpeedRange.Item2);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void GameManager_StartGame()
        {
            _delayed = false;
        }

        private void GameManager_EndGame()
        {
            StopAllCoroutines();
            _delayed = true;
        }

        private void GameManager_PauseGame()
        {
            StopAllCoroutines();
            _delayed = true;
        }

        private void GameManager_ResumeGame()
        {
            _delayed = false;
        }

        private void GameInput_PrimaryAttack()
        {
            Shot();
        }

        private void GameInput_AdditionalAttack()
        {
            Shot();
        }

        private void Update()
        {
            if (!_delayed && _type == LazerGunType.Enemy)
            {
                Shot();
            }
        }

        private void Shot()
        {
            if (_delayed)
            {
                return;
            }

            Rigidbody shot = Instantiate(_lazerPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            shot.transform.rotation = transform.rotation;
            shot.velocity = shot.transform.forward * _lazerVelocity;
            AudioManager.Instance.PlaySound(_sounds, transform.position);

            StartCoroutine(DelayRoutine());
        }

        private IEnumerator DelayRoutine()
        {
            _delayed = true;
            yield return new WaitForSeconds(_delay);
            _delayed = false;
        }

        private void OnDestroy()
        {
            GameManager.Instance.StartGame -= GameManager_StartGame;
            GameManager.Instance.EndGame -= GameManager_EndGame;

            GameManager.Instance.PauseGame -= GameManager_PauseGame;
            GameManager.Instance.ResumeGame -= GameManager_ResumeGame;

            switch (_type)
            {
                case LazerGunType.Primary:
                    InputManager.Instance.PrimaryAttack -= GameInput_PrimaryAttack;
                    break;
                case LazerGunType.Additional:
                    InputManager.Instance.AdditionalAttack -= GameInput_AdditionalAttack;
                    break;
                case LazerGunType.Enemy:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}