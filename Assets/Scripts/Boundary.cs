using Managers;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class Boundary : MonoBehaviour
{
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        LevelManager levelController = LevelManager.Instance;
        _collider.size = new Vector3(levelController.Width, _collider.size.y, levelController.Height);
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
