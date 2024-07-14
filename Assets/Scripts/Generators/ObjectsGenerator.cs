using Managers;
using UnityEngine;

namespace Generators
{
    public class ObjectsGenerator : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject[] _asteroidPrefabs;
        [SerializeField] private GameObject _enemyShipPrefab;
        [SerializeField] private GameObject _powerUpPrefab;

        private (float, float) _enemySpawnDelayRange;
        private float _bonusSpawnDelay;
        private int _maxEnemyCount;

        private float _enemySpawnDelayCounter;
        private float _enemySpawnChance;
        private float _enemyCounter;
        private float _bonusSpawnDelayCounter;

        private bool _active = false;

        public static ObjectsGenerator Instance { get; private set; }

        public void ReduceEnemyCounter()
        {
            _enemyCounter = Mathf.Max(0, _enemyCounter--);
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, LevelManager.Instance.TopBorder + transform.localScale.z);
            transform.localScale = new Vector3(LevelManager.Instance.Width, transform.localScale.y, transform.localScale.z);

            DifficultySO currentDifficulty = DifficultyManager.Instance.CurrentDifficulty;
            _enemySpawnDelayRange = currentDifficulty.EnemySpawnDelayRange;
            _enemySpawnChance = currentDifficulty.EnemySpawnChance;
            _bonusSpawnDelay = currentDifficulty.BonusSpawnDelay;
            _maxEnemyCount = currentDifficulty.MaxEnemyCount;

            _enemySpawnDelayCounter = Random.Range(_enemySpawnDelayRange.Item1, _enemySpawnDelayRange.Item1);
            _bonusSpawnDelayCounter = _bonusSpawnDelay;

            GameManager.Instance.StartGame += GameManager_StartGame;
            GameManager.Instance.EndGame += GameManager_EndGame;
            GameManager.Instance.ClearGame += GameManager_ClearGame;

            GameManager.Instance.PauseGame += GameManager_PauseGame;
            GameManager.Instance.ResumeGame += GameManager_ResumeGame;

            DifficultyManager.Instance.ChangeDifficulty += DifficultyManager_ChangeDifficulty;
        }

        private void GameManager_StartGame()
        {
            _active = true;
        }

        private void GameManager_EndGame()
        {
            _active = false;
        }

        private void GameManager_ClearGame()
        {
            _enemySpawnDelayCounter = Random.Range(_enemySpawnDelayRange.Item1, _enemySpawnDelayRange.Item2);
            _enemyCounter = 0;
            _bonusSpawnDelayCounter = _bonusSpawnDelay;
        }

        private void GameManager_PauseGame()
        {
            _active = false;
        }

        private void GameManager_ResumeGame()
        {
            _active = true;
        }

        private void DifficultyManager_ChangeDifficulty(DifficultySO difficultySO)
        {
            _enemySpawnDelayRange = difficultySO.EnemySpawnDelayRange;
            _enemySpawnChance = difficultySO.EnemySpawnChance;
            _bonusSpawnDelay = difficultySO.BonusSpawnDelay;
            _maxEnemyCount = difficultySO.MaxEnemyCount;
        }

        private void FixedUpdate()
        {
            if (!_active)
            {
                return;
            }

            _enemySpawnDelayCounter -= Time.fixedDeltaTime;
            if (_enemySpawnDelayCounter < 0)
            {
                SpawnEnemy();
            }

            _bonusSpawnDelayCounter -= Time.fixedDeltaTime;
            if (_bonusSpawnDelayCounter < 0)
            {
                SpawnBonus();
            }
        }

        private void SpawnEnemy()
        {
            float positionZ = transform.position.z;
            float positionX = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
            if (Random.Range(0f, 1f) > _enemySpawnChance || _enemyCounter >= _maxEnemyCount)
            {
                Instantiate(_asteroidPrefabs[Random.Range(0, _asteroidPrefabs.Length)], new Vector3(positionX, 0, positionZ), Quaternion.identity);
            }
            else
            {
                Instantiate(_enemyShipPrefab, new Vector3(positionX, 0, positionZ), Quaternion.identity);
                _enemyCounter++;
            }
            _enemySpawnDelayCounter = Random.Range(_enemySpawnDelayRange.Item1, _enemySpawnDelayRange.Item2);
        }

        private void SpawnBonus()
        {
            LevelManager levelController = LevelManager.Instance;
            float powerUpOffset = _powerUpPrefab.GetComponent<SphereCollider>().radius * 2;
            Instantiate(_powerUpPrefab,
                        new Vector3(Random.Range(levelController.LeftBorder + powerUpOffset, levelController.RightBorder - powerUpOffset),
                        0,
                        Random.Range(levelController.BottomBorder + powerUpOffset, levelController.TopBorder - powerUpOffset)),
                        Quaternion.identity);
            _bonusSpawnDelayCounter = _bonusSpawnDelay;
        }
    }
}
