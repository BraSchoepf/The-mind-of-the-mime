using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    public void Play()
    {
        SceneTransitionManager.instance.StartGame(); // Inicia el juego desde el primer nivel
    }

    
    public void Exit()
    {
        Debug.Log("EXIT");
        Application.Quit(); // Sale de la aplicación
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Progreso reiniciado.");
    }
}




