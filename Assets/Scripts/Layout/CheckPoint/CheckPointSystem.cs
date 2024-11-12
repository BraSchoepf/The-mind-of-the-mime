using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CheckPointSystem : MonoBehaviour
{
    public static CheckPointSystem instance;

    [SerializeField] private GameObject[] _checksPoint;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera;

    // Índice de punto de control inicial modificable desde el inspector
    [SerializeField] private int startingCheckPointIndex = 0;

    private GameObject _currentPlayer;
    private int indexChecksPoint;

    private void Awake()
    {
        instance = this;

        // Restablece los valores de PlayerPrefs al iniciar la escena
        PlayerPrefs.DeleteAll();  // Reinicia todos los datos guardados
        PlayerPrefs.Save();       // Guarda los cambios

        // Establecer el índice de punto de control inicial desde el inspector o el valor guardado
        indexChecksPoint = Mathf.Clamp(startingCheckPointIndex, 0, _checksPoint.Length - 1);

        // Busca todos los puntos de control en la escena
        _checksPoint = GameObject.FindGameObjectsWithTag("ChecksPoint");

        // Asegurarse de que los puntos de control existen
        if (_checksPoint.Length == 0)
        {
            Debug.LogError("No hay puntos de control en la escena.");
            return;
        }

        // Instanciar al jugador en el punto de control inicial definido
        RespawnPlayer();
    }

    public void LastCheckPoint(GameObject checkPoint)
    {
        // Guardar el último punto de control alcanzado
        for (int i = 0; i < _checksPoint.Length; i++)
        {
            if (_checksPoint[i] == checkPoint && i > indexChecksPoint)
            {
                indexChecksPoint = i;
                PlayerPrefs.SetInt("ChecksIndex", indexChecksPoint);
                PlayerPrefs.Save();
            }
        }
    }

    private void Update()
    {
        if (_currentPlayer == null)
        {
            RespawnPlayer(); // El jugador ha muerto, respawn al último punto de control
        }
    }

    private void RespawnPlayer()
    {
        if (_currentPlayer == null)
        {
            Vector3 spawnPosition = _checksPoint[indexChecksPoint].transform.position;
            _currentPlayer = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);

            // Asigna al jugador como el "Follow Target" de la Cinemachine Camera
            if (_cinemachineCamera != null)
            {
                _cinemachineCamera.Follow = _currentPlayer.transform;
            }
        }
    }

    public void ResetCheckPoints()
    {
        PlayerPrefs.DeleteKey("ChecksIndex");
        indexChecksPoint = 0;
        PlayerPrefs.Save();
    }
}

