using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [Space]
    [SerializeField] private float _positionY = -20f;

    private float _prefabHeight;
    private Vector3 _scale;

    private LinkedList<Transform> _spawnedPrefabs = new LinkedList<Transform>();

    public static BackgroundGenerator Instance { get; private set; }

    private void Start()
    {
        Instance = this;

        GetInitialValues();

        Player.Instance.Health.Die += Player_Die;
    }

    private void GetInitialValues()
    {
        LevelController levelController = LevelController.Instance;

        float scale = levelController.Width / _prefab.GetComponent<Renderer>().bounds.size.x;
        _scale = new Vector3(scale, 1f, scale);
        _prefabHeight = _prefab.GetComponent<Renderer>().bounds.size.z * scale;
    }

    private void Player_Die()
    {
        foreach (Transform prefab in _spawnedPrefabs)
        {
            prefab.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void Update()
    {
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        LevelController levelController = LevelController.Instance;

        while (_spawnedPrefabs.First != null && 
               _spawnedPrefabs.First.Value.position.z + (_prefabHeight / 2) < levelController.BottomBorder)
        {
            Transform prefab = _spawnedPrefabs.First.Value;
            _spawnedPrefabs.RemoveFirst();
            Destroy(prefab.gameObject);
        }

        if (_spawnedPrefabs.Count == 0)
        {
            GameObject background = Instantiate(_prefab, 
                new Vector3(0f, _positionY, levelController.BottomBorder + _scale.x), 
                Quaternion.identity, transform);
            background.transform.localScale = _scale;
            _spawnedPrefabs.AddLast(background.transform);
        }

        while (_spawnedPrefabs.Last.Value.position.z < levelController.TopBorder)
        {
            GameObject background = Instantiate(_prefab, 
                new Vector3(0f, _positionY, _spawnedPrefabs.Last.Value.position.z + _prefabHeight), 
                Quaternion.identity, transform);
            background.transform.localScale = _scale;
            _spawnedPrefabs.AddLast(background.transform);
        }
    }
}