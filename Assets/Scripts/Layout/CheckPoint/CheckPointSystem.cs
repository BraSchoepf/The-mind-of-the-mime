using System.Collections.Generic;
using System.Linq; // Agregar esta línea para utilizar LINQ
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CheckPointSystem : MonoBehaviour
{
    public static CheckPointSystem instance;

    [SerializeField] private GameObject[] _checksPoint;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CinemachineVirtualCamera _cinemachineCamera; // Referencia a la Cinemachine Virtual Camera
    private GameObject _currentPlayer;
    private int indexChecksPoint;

    // Lista de objetos que deben resetearse al morir el jugador
    private List<IResettable> _resettableObjects = new List<IResettable>();

    private void Awake()
    {
        instance = this;

        PlayerPrefs.DeleteKey("ChecksIndex");
        indexChecksPoint = 0;

        _checksPoint = GameObject.FindGameObjectsWithTag("ChecksPoint");

        if (_checksPoint.Length == 0)
        {
            Debug.LogError("No hay puntos de control en la escena.");
            return;
        }

        RespawnPlayer();
    }

    public void LastCheckPoint(GameObject checkPoint)
    {
        for (int i = 0; i < _checksPoint.Length; i++)
        {
            if (_checksPoint[i] == checkPoint && i > indexChecksPoint)
            {
                indexChecksPoint = i;
                PlayerPrefs.SetInt("ChecksIndex", indexChecksPoint);

                // Guardar objetos reiniciables
                _resettableObjects.Clear();
                IResettable[] objectsToReset = FindObjectsOfType<MonoBehaviour>().OfType<IResettable>().ToArray();
                _resettableObjects.AddRange(objectsToReset);
            }
        }
    }

    private void Update()
    {
        if (_currentPlayer == null)
        {
            RespawnPlayer();
            ResetAllObjects(); // Reiniciar objetos al reaparecer el jugador
        }
    }

    private void RespawnPlayer()
    {
        if (_currentPlayer == null)
        {
            Vector3 spawnPosition = _checksPoint[indexChecksPoint].transform.position;
            _currentPlayer = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);

            if (_cinemachineCamera != null)
            {
                _cinemachineCamera.Follow = _currentPlayer.transform;
            }
        }
    }

    public void ResetAllObjects()
    {
        foreach (IResettable obj in _resettableObjects)
        {
            obj.ResetObject();
        }
    }

    public void ResetCheckPoints()
    {
        PlayerPrefs.DeleteKey("ChecksIndex");
        indexChecksPoint = 0;
    }
}
