using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IMPOneWay : InvisibleMovingPlatform
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float waitTimeAtStart = 1f;
    [SerializeField] private List<Transform> _destinationPoints;

    private Vector3 _initialPosition;
    private bool _isWaiting = false;
    private int _currentDestinationIndex = 0;

    protected override void Start()
    {
        base.Start();
        _initialPosition = transform.position;

        if (_destinationPoints.Count == 0)
        {
            Debug.LogWarning("No se han asignado puntos de destino, la plataforma no se moverá.");
        }

        // Inicia la plataforma en espera inicial
        StartCoroutine(InitialWaitAndStartCycle());
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator InitialWaitAndStartCycle()
    {
        // Espera inicial antes de iniciar el ciclo
        _isWaiting = true;
        transform.position = _initialPosition; // Asegura que la plataforma comience en la posición inicial
        yield return new WaitForSeconds(waitTimeAtStart); // Espera inicial
        _isWaiting = false;

        // Inicia el ciclo de movimiento
        StartCoroutine(PlatformCycle());
    }

    private IEnumerator PlatformCycle()
    {
        for (int i = 0; i < _destinationPoints.Count; i++)
        {
            if (_destinationPoints[i] == null)
            {
                Debug.LogError($"IMPOneWay '{gameObject.name}': _destinationPoints[{i}] es null. Revisá el Inspector.");
                yield break;
            }
        }
        if (!_isInvisible && _destinationPoints.Count > 0)
        {
            Transform targetPoint = _destinationPoints[_currentDestinationIndex];

            while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                if (_isInvisible)
                {
                    yield return null; // pausa sin matar la coroutine
                    continue;
                }
                Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;
                transform.Translate(directionToTarget * _speed * Time.deltaTime, Space.World);
                yield return null;
            }

            _isWaiting = true;
            yield return new WaitForSeconds(waitTimeAtStart);
            _isWaiting = false;

            _currentDestinationIndex++;
            if (_currentDestinationIndex >= _destinationPoints.Count)
            {
                transform.position = _destinationPoints[0].position;
                _currentDestinationIndex = _destinationPoints.Count > 1 ? 1 : 0;
            }
        }
        else
        {
            yield return null;
        }
    }
}
