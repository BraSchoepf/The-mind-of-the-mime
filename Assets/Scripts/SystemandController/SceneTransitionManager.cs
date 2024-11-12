using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public void ChangeScene(int sceneIndex, int startCheckPoint)
    {
        PlayerPrefs.SetInt("ChecksIndex", startCheckPoint); // Guarda el punto de inicio en PlayerPrefs
        SceneManager.LoadScene(sceneIndex); // Carga la escena deseada
    }
}

