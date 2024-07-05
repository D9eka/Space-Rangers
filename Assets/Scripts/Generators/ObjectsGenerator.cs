using UnityEngine;

public class ObjectsGenerator : MonoBehaviour
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

    private bool _active = false;

    private void Awake()
    {
        _enemySpawnDelayCounter = Random.Range(_enemyMinSpawnDelay, _enemyMaxSpawnDelay);
        _bonusSpawnDelayCounter = _bonusSpawnDelay;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, LevelManager.Instance.TopBorder + transform.localScale.z);
        transform.localScale = new Vector3(LevelManager.Instance.Width, transform.localScale.y, transform.localScale.z);

        GameManager.Instance.StartGame += GameManager_StartGame;
        GameManager.Instance.EndGame += GameManager_EndGame;

        GameManager.Instance.PauseGame += GameManager_PauseGame;
        GameManager.Instance.ResumeGame += GameManager_ResumeGame;
    }

    private void GameManager_StartGame()
    {
        _active = true;
    }

    private void GameManager_EndGame()
    {
        _active = false;
    }

    private void GameManager_PauseGame()
    {
        _active = false;
    }

    private void GameManager_ResumeGame()
    {
        _active = true;
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
        int choice = Random.Range(0, _asteroidPrefabs.Length + 1);
        if (choice < _asteroidPrefabs.Length)
        {
            Instantiate(_asteroidPrefabs[choice], new Vector3(positionX, 0, positionZ), Quaternion.identity);
        }
        else
        {
            Instantiate(_enemyShipPrefab, new Vector3(positionX, 0, positionZ), Quaternion.identity);
        }
        _enemySpawnDelayCounter = Random.Range(_enemyMinSpawnDelay, _enemyMaxSpawnDelay);
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
