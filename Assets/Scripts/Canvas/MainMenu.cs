using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayNewVersion()
    {
        SceneTransitionManager.instance.StartNewVersion(); // Va al nivel 1 nuevo (escena 1)
    }

    public void PlayOldVersion()
    {
        SceneTransitionManager.instance.StartOldVersion(); // Va al nivel 1 viejo (escena 3)
    }

    public void Exit()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Progreso reiniciado.");
    }
}