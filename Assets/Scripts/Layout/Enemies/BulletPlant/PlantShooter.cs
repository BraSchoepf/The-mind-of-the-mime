using System.Collections;
using UnityEngine;

public class PlantShooter : MonoBehaviour
{
    public enum ShootMode { Fixed, Tracker }

    [Header("Modo")]
    [SerializeField] private ShootMode _shootMode = ShootMode.Fixed;

    [Header("Disparo")]
    [SerializeField] private float _fireRate = 2f;
    [SerializeField] private float _initialDelay = 0f;
    [SerializeField] private Transform _shootOrigin;
    [SerializeField] private float _shootDelay = 0.2f;

    [Header("Modo Fijo")]
    [SerializeField] private Vector2 _shootDirection = Vector2.right;

    [Header("Modo Tracker")]
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private bool _alwaysShoot = false;

    [Header("Visual")]
    [SerializeField] private Transform _plantVisual;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _visualRotationOffset = 0f; // ajustás desde Inspector

    private float _timer;

    private void Start()
    {
        _timer = -_initialDelay;
    }

    private void Update()
    {
        if (_shootMode == ShootMode.Tracker)
            TrackPlayer();

        _timer += Time.deltaTime;
        if (_timer >= _fireRate)
        {
            TryShoot();
            _timer = 0f;
        }
    }

    private void TrackPlayer()
    {
        if (_plantVisual == null) return;
        Transform player = CheckPointSystem.instance?.PlayerTransform;
        if (player == null) return;

        Vector2 dir = (player.position - _plantVisual.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _plantVisual.rotation = Quaternion.Euler(0f, 0f, angle + _visualRotationOffset);
    }
    private void TryShoot()
    {
        if (_shootMode == ShootMode.Fixed)
        {
            Shoot(_shootDirection.normalized);
        }
        else
        {
            Transform player = CheckPointSystem.instance?.PlayerTransform;
            if (player == null) return;

            float distance = Vector2.Distance(transform.position, player.position);
            if (!_alwaysShoot && distance > _detectionRange) return;

            // Dirección mundo desde el visual hacia el jugador, ignora rotación del padre
            Vector3 origin = _shootOrigin != null ? _shootOrigin.position : _plantVisual.position;
            Vector2 dir = (player.position - origin).normalized;
            Shoot(dir);
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (BulletPool.instance == null) return;
        _animator?.Play("Shoot", 0, 0f);
        StartCoroutine(SpawnBulletDelayed(direction));
    }

    private IEnumerator SpawnBulletDelayed(Vector2 direction)
    {
        yield return new WaitForSeconds(_shootDelay);
        if (BulletPool.instance == null) yield break;

        Bullet b = BulletPool.instance.GetBullet();
        b.transform.position = _shootOrigin != null ? _shootOrigin.position : transform.position;
        b.Launch(direction);
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = _shootOrigin != null ? _shootOrigin.position : transform.position;

        if (_shootMode == ShootMode.Fixed)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, (Vector3)_shootDirection.normalized * 1.5f);
            Gizmos.DrawWireSphere(origin, 0.15f);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
        }
    }
}