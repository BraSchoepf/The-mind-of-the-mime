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

    [Header("Modo Fijo")]
    [SerializeField] private Vector2 _shootDirection = Vector2.right;

    [Header("Modo Tracker")]
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private bool _alwaysShoot = false;

    private float _timer;
    private Transform _player => CheckPointSystem.instance.PlayerTransform;

    private void Start()
    {
        _timer = -_initialDelay;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fireRate)
        {
            TryShoot();
            _timer = 0f;
        }
    }

    private void TryShoot()
    {
        if (_shootMode == ShootMode.Fixed)
        {
            Shoot(_shootDirection.normalized);
        }
        else
        {
            if (_player == null) return;

            float distance = Vector2.Distance(transform.position, _player.position);
            if (_alwaysShoot || distance <= _detectionRange)
            {
                Vector2 direction = (_player.position - transform.position).normalized;
                Shoot(direction);
            }
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (BulletPool.instance == null) return;

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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(origin, 0.15f);
        }
    }
}