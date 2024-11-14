using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IMPOneWay : InvisibleMovingPlatform
{
    [Header("Configuración de Movimiento")]
    [SerializeField] private float _speed = 2f; // Velocidad de movimiento de la plataforma
    [SerializeField] private float waitTimeAtStart = 1f; // Tiempo de espera en la posición inicial antes de empezar a moverse
    [SerializeField] private List<Transform> _destinationPoints; // Lista de puntos de destino a los que la plataforma debe ir

    private Vector3 _initialPosition; // Posición inicial de la plataforma
    private bool _isWaiting = false; // Indica si la plataforma está esperando
    private int _currentDestinationIndex = 0; // Índice del punto de destino actual
    private bool _hasCompletedCycle = false; // Bandera para verificar si se completó el ciclo

    protected override void Start()
    {
        base.Start();
        _initialPosition = transform.position; // Guarda la posición inicial al empezar

        if (_destinationPoints.Count == 0)
        {
            Debug.LogWarning("No se han asignado puntos de destino, la plataforma no se moverá.");
        }

        // Iniciar la espera inicial antes de comenzar el movimiento
        StartCoroutine(WaitAtStart());
    }

    protected override void Update()
    {
        if (!_isInvisible && !_isWaiting && _destinationPoints.Count > 0) // Mueve solo si está visible y no está en espera
        {
            MovePlatform();
        }

        base.Update();
    }

    private void MovePlatform()
    {
        if (_destinationPoints.Count > 0)
        {
            // Obtener el siguiente punto de destino en la lista
            Transform targetPoint = _destinationPoints[_currentDestinationIndex];

            // Calcular la dirección hacia el objetivo sin afectar la rotación de la plataforma
            Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;

            // Mover la plataforma en la dirección calculada
            transform.Translate(directionToTarget * _speed * Time.deltaTime, Space.World);

            // Comprobar si la plataforma ha alcanzado el destino
            if (Vector3.Distance(transform.position, targetPoint.position) <= 0.1f)
            {
                // Iniciar espera en el punto antes de pasar al siguiente
                StartCoroutine(WaitAtPoint());
            }
        }
    }


    private IEnumerator WaitAtPoint()
    {
        _isWaiting = true; // Activa el estado de espera
        yield return new WaitForSeconds(waitTimeAtStart); // Espera el tiempo especificado en el punto
        _isWaiting = false; // Desactiva el estado de espera y pasa al siguiente punto

        // Avanzar al siguiente punto de destino
        _currentDestinationIndex++;

        // Si hemos llegado al último punto, reiniciamos el ciclo de inmediato
        if (_currentDestinationIndex >= _destinationPoints.Count)
        {
            // Teletransportar la plataforma al primer punto
            transform.position = _destinationPoints[0].position;

            // Después de teletransportarse, inicia el movimiento hacia el primer punto
            _currentDestinationIndex = 1; // Comienza el ciclo de nuevo desde el segundo punto
        }
    }

    private IEnumerator WaitAtStart()
    {
        _isWaiting = true; // Activa el estado de espera
        transform.position = _initialPosition; // Respawn en el punto inicial
        yield return new WaitForSeconds(waitTimeAtStart); // Espera el tiempo especificado en la posición inicial
        _isWaiting = false; // Desactiva el estado de espera y comienza a moverse
    }
}
