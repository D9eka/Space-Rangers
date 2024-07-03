using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private float _prefabHeight;
    private Vector3 _scale;

    private List<Transform> _spawnedPrefabs = new List<Transform>();

    private const float POSITION_Y = -20f;

    public static BackgroundGenerator Instance { get; private set; }

    private void Start()
    {
        Instance = this;

        LevelController levelController = LevelController.Instance;

        float scale = levelController.Width / _prefab.GetComponent<Renderer>().bounds.size.x;
        _scale = new Vector3(scale, 1f, scale);
        _prefabHeight = _prefab.GetComponent<Renderer>().bounds.size.z * scale;
        SpawnPrefab();
    }

    private void Update()
    {
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        LevelController levelController = LevelController.Instance;

        if (_spawnedPrefabs.Count == 0)
        {
            GameObject background = Instantiate(_prefab, new Vector3(0f, POSITION_Y, levelController.BottomBorder + _scale.x), Quaternion.identity, transform);
            background.transform.localScale = _scale;
            _spawnedPrefabs.Add(background.transform);
        }

        while (_spawnedPrefabs.Last().position.z < levelController.TopBorder)
        {
            GameObject background = Instantiate(_prefab, new Vector3(0f, POSITION_Y, _spawnedPrefabs.Last().position.z + _prefabHeight), Quaternion.identity, transform);
            background.transform.localScale = _scale;
            _spawnedPrefabs.Add(background.transform);
        }
    }
}