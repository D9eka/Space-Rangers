using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private bool _isEnemyShot;

    public bool IsEnemyLazer => _isEnemyShot;
}
