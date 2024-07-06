using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Generators
{
    public class BackgroundGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [Space]
        [SerializeField] private float _positionY = -20f;
        [SerializeField] private float _initialVelocity = 30f; 

        private float _prefabHeight;
        private Vector3 _scale;

        private LinkedList<Transform> _spawnedPrefabs = new LinkedList<Transform>();
        private bool _movePrefabs;

        public static BackgroundGenerator Instance { get; private set; }

        private void Start()
        {
            Instance = this;

            GetInitialValues();

            GameManager.Instance.StartGame += GameManager_StartGame;
            GameManager.Instance.EndGame += GameManager_EndGame;
            GameManager.Instance.ClearGame += GameManager_ClearGame;

            GameManager.Instance.PauseGame += GameManager_PauseGame;
            GameManager.Instance.ResumeGame += GameManager_ResumeGame;
        }

        private void GetInitialValues()
        {
            LevelManager levelController = LevelManager.Instance;

            float scale = levelController.Width / _prefab.GetComponent<Renderer>().bounds.size.x;
            _scale = new Vector3(scale, 1f, scale);
            _prefabHeight = _prefab.GetComponent<Renderer>().bounds.size.z * scale;
        }

        private void GameManager_StartGame()
        {
            _movePrefabs = true;
            _spawnedPrefabs.Clear();
        }

        private void GameManager_EndGame()
        {
            _movePrefabs = false;
            ChangePrefabsSpeed(0f);
        }

        private void GameManager_ClearGame()
        {
            foreach (Transform prefab in _spawnedPrefabs)
            {
                Destroy(prefab.gameObject);
            }
            _spawnedPrefabs.Clear();
        }

        private void GameManager_PauseGame()
        {
            _movePrefabs = false;
            ChangePrefabsSpeed(0f);
        }

        private void GameManager_ResumeGame()
        {
            _movePrefabs = true;
            ChangePrefabsSpeed(_initialVelocity);
        }

        private void ChangePrefabsSpeed(float speed)
        {
            foreach (Transform prefab in _spawnedPrefabs)
            {
                prefab.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -speed);
            }
        }

        private void Update()
        {
            SpawnPrefab();
        }

        private void SpawnPrefab()
        {
            LevelManager levelController = LevelManager.Instance;

            DestroyInviciblePrefabs(levelController.BottomBorder);

            if (_spawnedPrefabs.Count == 0)
            {
                InstantiatePrefab(new Vector3(0f, _positionY, levelController.BottomBorder + _scale.x));
            }

            while (_spawnedPrefabs.Last.Value.position.z < levelController.TopBorder)
            {
                InstantiatePrefab(new Vector3(0f, _positionY, _spawnedPrefabs.Last.Value.position.z + _prefabHeight));
            }
        }

        private void DestroyInviciblePrefabs(float bottomBorder)
        {
            while (_spawnedPrefabs.First != null &&
                   _spawnedPrefabs.First.Value.position.z + (_prefabHeight / 2) < bottomBorder)
            {
                Transform prefab = _spawnedPrefabs.First.Value;
                _spawnedPrefabs.RemoveFirst();
                Destroy(prefab.gameObject);
            }
        }

        private void InstantiatePrefab(Vector3 position)
        {
            GameObject background = Instantiate(_prefab, position, Quaternion.identity, transform);
            background.transform.localScale = _scale;
            if (_movePrefabs)
            {
                background.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -_initialVelocity);
            }
            _spawnedPrefabs.AddLast(background.transform);
        }
    }
}