using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // M�todo para cambiar de escena al iniciar el juego desde el men�
    public void StartGame()
    {

        int startingSceneIndex = 1;
        LoadScene(startingSceneIndex);
    }


    public void ChangeLevel(int sceneIndex, int checkpointIndex)
    {
        PlayerPrefs.SetInt("ChecksIndex", checkpointIndex); // Guardamos el punto de control
        LoadScene(sceneIndex); // Cargamos la siguiente escena
    }

    //Finalizar el juego
    public void EndGame()
    {
        Debug.Log("Fin del juego.");
        LoadScene(0);
    }
    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}




