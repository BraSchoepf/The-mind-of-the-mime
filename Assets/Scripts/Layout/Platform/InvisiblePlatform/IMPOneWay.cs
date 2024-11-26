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
        while (true)
        {
            if (!_isInvisible && _destinationPoints.Count > 0)
            {
                // Obtener el siguiente punto de destino
                Transform targetPoint = _destinationPoints[_currentDestinationIndex];

                // Mover la plataforma hacia el punto
                while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
                {
                    if (_isInvisible) yield break; // Detener movimiento si la plataforma se vuelve invisible
                    Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;
                    transform.Translate(directionToTarget * _speed * Time.deltaTime, Space.World);
                    yield return null; // Espera hasta el siguiente frame
                }

                // Espera en el punto alcanzado
                _isWaiting = true;
                yield return new WaitForSeconds(waitTimeAtStart);
                _isWaiting = false;

                // Avanzar al siguiente punto
                _currentDestinationIndex++;

                // Si llegamos al último punto, teletransportar al primero
                if (_currentDestinationIndex >= _destinationPoints.Count)
                {
                    transform.position = _destinationPoints[0].position; // Teletransporte al punto inicial
                    _currentDestinationIndex = 1; // Reinicia desde el segundo punto
                }
            }
            else
            {
                yield return null; // Espera el siguiente frame si está invisible o sin puntos
            }
        }
    }
}
