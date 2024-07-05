using System;
using System.Collections;
using UnityEngine;

public class LazerGun : MonoBehaviour
{
    public enum LazerGunType
    {
        Primary,
        Additional,
        Enemy
    }

    [Header("Prefab")]
    [SerializeField] private GameObject _lazerPrefab;
    [Space]
    [SerializeField] private AudioClip[] _sounds;
    [Header("Params")]
    [SerializeField] private LazerGunType _type;
    [SerializeField] private float _delay;
    [SerializeField] private Vector3 _lazerVelocity;

    private bool _delayed;

    private void Start()
    {
        GameManager.Instance.StartGame += GameManager_StartGame;
        GameManager.Instance.EndGame += GameManager_EndGame;

        GameManager.Instance.PauseGame += GameManager_PauseGame;
        GameManager.Instance.ResumeGame += GameManager_ResumeGame;

        switch (_type) 
        { 
            case LazerGunType.Primary:
                InputManager.Instance.PrimaryAttack += GameInput_PrimaryAttack;
                break;
            case LazerGunType.Additional:
                InputManager.Instance.AdditionalAttack += GameInput_AdditionalAttack;
                break;
            case LazerGunType.Enemy:
                GetComponentInParent<Enemy>().Attack += Enemy_Attack;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void GameManager_StartGame()
    {
        _delayed = false;
    }

    private void GameManager_EndGame()
    {
        StopAllCoroutines();
        _delayed = true;
    }

    private void GameManager_PauseGame()
    {
        StopAllCoroutines();
        _delayed = true;
    }

    private void GameManager_ResumeGame()
    {
        _delayed = false;
    }

    private void GameInput_PrimaryAttack()
    {
        Shot();
    }

    private void GameInput_AdditionalAttack()
    {
        Shot();
    }

    private void Enemy_Attack()
    {
        Shot(true);
    }

    private void Shot(bool toPlayer = false)
    {
        if (_delayed)
        {
            return;
        }

        Rigidbody shot = Instantiate(_lazerPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        if (!toPlayer)
        {
            shot.velocity = _lazerVelocity;
        }
        else
        {
            Vector3 playerDirection = Player.Instance.transform.position - shot.position;
            float angle = Vector3.SignedAngle(Vector3.back, playerDirection, Vector3.up);
            shot.transform.Rotate(0, angle, 0);
            shot.velocity = playerDirection / playerDirection.magnitude * _lazerVelocity.z;
        }
        AudioManager.Instance.PlaySound(_sounds, transform.position);

        StartCoroutine(DelayRoutine());
    }

    private IEnumerator DelayRoutine()
    {
        _delayed = true;
        yield return new WaitForSeconds(_delay);
        _delayed = false;
    }

    private void OnDestroy()
    {
        GameManager.Instance.StartGame -= GameManager_StartGame;
        GameManager.Instance.EndGame -= GameManager_EndGame;

        GameManager.Instance.PauseGame -= GameManager_PauseGame;
        GameManager.Instance.ResumeGame -= GameManager_ResumeGame;

        switch (_type)
        {
            case LazerGunType.Primary:
                InputManager.Instance.PrimaryAttack -= GameInput_PrimaryAttack;
                break;
            case LazerGunType.Additional:
                InputManager.Instance.AdditionalAttack -= GameInput_AdditionalAttack;
                break;
            case LazerGunType.Enemy:
                break;
            default:
                throw new NotImplementedException();
        }
    }
}