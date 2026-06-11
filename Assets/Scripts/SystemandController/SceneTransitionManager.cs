using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    // Configurá estos índices según tu Build Settings
    [Header("Índices - Versión Nueva")]
    public int newVersion_Level1 = 1;
    public int newVersion_Level2 = 2;

    [Header("Índices - Versión Vieja")]
    public int oldVersion_Level1 = 3;
    public int oldVersion_Level2 = 4;

    [Header("Otras escenas")]
    public int endSceneIndex = 5;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- Flujo versión NUEVA ---
    public void StartNewVersion()
    {
        PlayerPrefs.SetInt("ChecksIndex", 0); // ya lo tenías
        PlayerPrefs.Save();                   // agregar esto para asegurar escritura
        LoadScene(newVersion_Level1);
    }

    // --- Flujo versión VIEJA ---
    public void StartOldVersion()
    {
        PlayerPrefs.SetInt("ChecksIndex", 0);
        PlayerPrefs.Save();
        LoadScene(oldVersion_Level1);
    }

    // Usado internamente al completar niveles
    public void ChangeLevel(int sceneIndex, int checkpointIndex)
    {
        PlayerPrefs.SetInt("ChecksIndex", checkpointIndex);
        LoadScene(sceneIndex);
    }

    public void EndGame()
    {
        PlayerPrefs.SetInt("ChecksIndex", 0);
        LoadScene(endSceneIndex);
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}