using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IResettable
{
    [SerializeField] private float _resetInterval = 5f; // segundos, editable en el Inspector
    private Vector3 _initialPosition;
    private float _timer;

    private void Start()
    {
        _initialPosition = transform.position;
        _timer = _resetInterval;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            ResetObject();
            _timer = _resetInterval;
        }
    }

    public void ResetObject()
    {
        transform.position = _initialPosition;
        // Restablecer otros estados necesarios
    }
}