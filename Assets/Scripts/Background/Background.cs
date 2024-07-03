using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Background : MonoBehaviour
{
    [SerializeField] private float _initialVelocity;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.velocity = new Vector3 (0, 0, _initialVelocity);
    }
}