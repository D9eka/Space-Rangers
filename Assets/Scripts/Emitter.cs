using UnityEngine;
using System;

public class Emitter : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] _asteroidPrefabs;
    [SerializeField] private GameObject _enemyShipPrefab;
    [SerializeField] private GameObject _powerUpPrefab;
    [Header("Enemies")]
    [SerializeField] private float _enemyMinSpawnDelay = 0.5f;
    [SerializeField] private float _enemyMaxSpawnDelay = 1f;
    [Header("Bonuses")]
    [SerializeField] private float _bonusSpawnDelay = 10f;

    private float _enemySpawnDelayCounter;
    private float _bonusSpawnDelayCounter;

    private void Awake()
    {
        _enemySpawnDelayCounter = UnityEngine.Random.Range(_enemyMinSpawnDelay, _enemyMaxSpawnDelay);
        _bonusSpawnDelayCounter = _bonusSpawnDelay;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, LevelController.Instance.TopBorder + transform.localScale.z);
        transform.localScale = new Vector3(LevelController.Instance.Width, transform.localScale.y, transform.localScale.z);
    }

    private void Update()
    {
        _enemySpawnDelayCounter -= Time.deltaTime;
        if (_enemySpawnDelayCounter < 0)
        {
            SpawnEnemy();
        }

        _bonusSpawnDelayCounter -= Time.deltaTime;
        if (_bonusSpawnDelayCounter < 0)
        {
            SpawnBonus();
        }

        UI.Instance.ChangeBonusText($"До появления бонуса осталось {TimeSpan.FromSeconds(_bonusSpawnDelayCounter):ss} секунд");
    }

    private void SpawnEnemy()
    {
        float positionZ = transform.position.z;
        float positionX = UnityEngine.Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
        int choice = UnityEngine.Random.Range(0, _asteroidPrefabs.Length + 1);
        if (choice < _asteroidPrefabs.Length)
        {
            Instantiate(_asteroidPrefabs[choice], new Vector3(positionX, 0, positionZ), Quaternion.identity);
        }
        else
        {
            Instantiate(_enemyShipPrefab, new Vector3(positionX, 0, positionZ), Quaternion.identity);
        }
        _enemySpawnDelayCounter = UnityEngine.Random.Range(_enemyMinSpawnDelay, _enemyMaxSpawnDelay);
    }

    private void SpawnBonus()
    {
        LevelController levelController = LevelController.Instance;
        float powerUpOffset = _powerUpPrefab.GetComponent<SphereCollider>().radius * 2;
        Instantiate(_powerUpPrefab,
                    new Vector3(UnityEngine.Random.Range(levelController.LeftBorder + powerUpOffset, levelController.RightBorder - powerUpOffset),
                    0,
                    UnityEngine.Random.Range(levelController.BottomBorder + powerUpOffset, levelController.TopBorder - powerUpOffset)),
                    Quaternion.identity);
        _bonusSpawnDelayCounter = _bonusSpawnDelay;
    }
}
