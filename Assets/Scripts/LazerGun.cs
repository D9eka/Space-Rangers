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
        Player.Instance.Health.Die += Player_Die;

        switch (_type) 
        { 
            case LazerGunType.Primary:
                GameInput.Instance.PrimaryAttack += GameInput_PrimaryAttack;
                break;
            case LazerGunType.Additional:
                GameInput.Instance.AdditionalAttack += GameInput_AdditionalAttack;
                break;
            case LazerGunType.Enemy:
                GetComponentInParent<Enemy>().Attack += Enemy_Attack;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void Player_Die()
    {
        StopAllCoroutines();
        _delayed = true;
    }

    private void GameInput_PrimaryAttack()
    {
        Shot();
    }

    private void GameInput_AdditionalAttack()
    {
        Shot();
    }

    private void Enemy_Attack(float angle)
    {
        Shot(angle);
    }

    private void Shot(float angle = 0)
    {
        if (_delayed)
        {
            return;
        }

        GameObject shot = Instantiate(_lazerPrefab, transform.position, Quaternion.Euler(0, angle, 0));
        shot.GetComponent<Rigidbody>().velocity = _lazerVelocity;
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
        Player.Instance.Health.Die -= Player_Die;

        switch (_type)
        {
            case LazerGunType.Primary:
                GameInput.Instance.PrimaryAttack -= GameInput_PrimaryAttack;
                break;
            case LazerGunType.Additional:
                GameInput.Instance.AdditionalAttack -= GameInput_AdditionalAttack;
                break;
            case LazerGunType.Enemy:
                break;
            default:
                throw new NotImplementedException();
        }
    }
}