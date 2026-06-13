using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _maxLifetime = 5f; // seguro por si no sale de cámara

    private Vector2 _direction;
    private float _lifetime;

    public void Launch(Vector2 direction)
    {
        _direction = direction.normalized;
        _lifetime = 0f;
    }

    private void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);

        _lifetime += Time.deltaTime;
        if (_lifetime >= _maxLifetime)
            BulletPool.instance.ReturnBullet(this);
    }

    private void OnBecameInvisible()
    {
        BulletPool.instance.ReturnBullet(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckPointSystem.instance.TeleportToCheckpoint(); // ← en lugar de RespawnPlayer
            BulletPool.instance.ReturnBullet(this);
        }
        else if (!other.CompareTag("Bullet"))
        {
            // Choca con geometría, vuelve al pool
            BulletPool.instance.ReturnBullet(this);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            BulletPool.instance.ReturnBullet(this);
        }
    }
}