using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _poolSize = 20;

    private Queue<Bullet> _pool = new Queue<Bullet>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_bulletPrefab, transform);
            Bullet b = obj.GetComponent<Bullet>();
            obj.SetActive(false);
            _pool.Enqueue(b);
        }
    }

    public Bullet GetBullet()
    {
        if (_pool.Count > 0)
        {
            Bullet b = _pool.Dequeue();
            b.gameObject.SetActive(true);
            return b;
        }

        // Si el pool está vacío, crea una extra
        GameObject obj = Instantiate(_bulletPrefab, transform);
        return obj.GetComponent<Bullet>();
    }

    public void ReturnBullet(Bullet b)
    {
        b.gameObject.SetActive(false);
        b.transform.SetParent(transform);
        _pool.Enqueue(b);
    }
}