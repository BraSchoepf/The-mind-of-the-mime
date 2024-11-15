using System.Collections;
using UnityEngine;
using Cinemachine;

public class CheckPointSystem : MonoBehaviour
{
    public static CheckPointSystem instance;

    [SerializeField] private GameObject[] _checksPoint; // Puntos de control asignados desde el Inspector
    [SerializeField] private GameObject _playerPrefab; // Prefab del jugador
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera; // Cámara que seguirá al jugador

    [SerializeField] private int startingCheckPointIndex = 0; // Índice inicial desde el Inspector

    private GameObject _currentPlayer; // Referencia al jugador actual
    private int indexChecksPoint; // Índice del último punto de control alcanzado

    private void Awake()
    {
        instance = this;

        // Cargar el índice del último punto de control o usar el predeterminado
        indexChecksPoint = PlayerPrefs.GetInt("ChecksIndex", startingCheckPointIndex);

        // Validar que los puntos de control están asignados correctamente
        if (_checksPoint == null || _checksPoint.Length == 0)
        {
            Debug.LogError("No hay puntos de control asignados en el Inspector.");
            return;
        }

        // Garantizar que el índice inicial no sea inválido
        indexChecksPoint = Mathf.Clamp(indexChecksPoint, 0, _checksPoint.Length - 1);

        // Buscar al jugador en la escena
        _currentPlayer = GameObject.FindGameObjectWithTag("Player");

        if (_currentPlayer != null)
        {
            // Si ya existe un jugador, asignar la cámara
            if (_cinemachineCamera != null)
            {
                _cinemachineCamera.Follow = _currentPlayer.transform;
            }
        }
        else
        {
            // Instanciar al jugador en el último punto de control
            RespawnPlayer();
        }
    }

    public void LastCheckPoint(GameObject checkPoint)
    {
        // Guardar el último punto de control alcanzado
        for (int i = 0; i < _checksPoint.Length; i++)
        {
            if (_checksPoint[i] == checkPoint && i > indexChecksPoint)
            {
                indexChecksPoint = i; // Actualizar el índice
                PlayerPrefs.SetInt("ChecksIndex", indexChecksPoint); // Guardar el progreso
                PlayerPrefs.Save();
            }
        }
    }

    private void Update()
    {
        // Respawnear si el jugador no está activo en la escena
        if (_currentPlayer == null)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        // Verificar que el índice del punto de control sea válido
        if (indexChecksPoint >= 0 && indexChecksPoint < _checksPoint.Length)
        {
            Vector3 spawnPosition = _checksPoint[indexChecksPoint].transform.position;
            _currentPlayer = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);

            // Asignar la cámara al nuevo jugador
            if (_cinemachineCamera != null)
            {
                _cinemachineCamera.Follow = _currentPlayer.transform;
            }
        }
        else
        {
            Debug.LogError("Índice de punto de control no válido.");
        }
    }
}




