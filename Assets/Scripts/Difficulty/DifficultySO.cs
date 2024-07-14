using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu()]
public class DifficultySO : ScriptableObject
{
    [Header("Score")]
    [SerializeField] private int _scoreToActivate;
    [Header("BackgroundGenerator")]
    [SerializeField] private float _backgroundVelocity;
    [Header("ObjectGenerator")]
    [SerializeField] private float _enemyMinSpawnDelay;
    [SerializeField] private float _enemyMaxSpawnDelay;
    [Space]
    [Range(0.1f, 0.9f)] [SerializeField] private float _enemySpawnChance;
    [SerializeField] private int _maxEnemyCount;
    [Space]
    [SerializeField] private float _bonusSpawnDelay;
    [Header("Asteroids")]
    [SerializeField] private float _asteroidMinSpeed;
    [SerializeField] private float _asteroidMaxSpeed;
    [Space]
    [SerializeField] private float _asteroidMinScale;
    [SerializeField] private float _asteroidMaxScale;
    [Header("Enemy")]
    [SerializeField] private float _enemyMinSpeed;
    [SerializeField] private float _enemyMaxSpeed;
    [Space]
    [SerializeField] private float _enenyMinAttackSpeed;
    [SerializeField] private float _enenyMaxAttackSpeed;
    [Space]
    [SerializeField] private float _enemyAttackDelay;

    public int ScoreToActivate => _scoreToActivate;

    public float BackgroundVelocity => _backgroundVelocity;

    public (float, float) EnemySpawnDelayRange => (_enemyMinSpawnDelay, _enemyMaxSpawnDelay);
    public float EnemySpawnChance => _enemySpawnChance;
    public int MaxEnemyCount => _maxEnemyCount;

    public float BonusSpawnDelay => _bonusSpawnDelay;

    public (float, float) AsteroidSpeedRange => (_asteroidMinSpeed, _asteroidMaxSpeed);
    public (float, float) AsteroidScaleRange => (_asteroidMinScale, _asteroidMaxScale);

    public (float, float) EnemySpeedRange => (_enemyMinSpeed, _enemyMaxSpeed);
    public (float, float) EnemyAttackSpeedRange => (_enenyMinAttackSpeed, _enenyMaxAttackSpeed);
    public float EnemyAttackDelay => _enemyAttackDelay;
}