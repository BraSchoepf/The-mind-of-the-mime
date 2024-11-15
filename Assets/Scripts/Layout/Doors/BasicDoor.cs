using UnityEngine;

public class BasicDoor : MonoBehaviour
{
    [SerializeField] private int nextSceneIndex; // Índice de la siguiente escena
    [SerializeField] private int checkpointIndex; // Índice del punto de control de la siguiente escena

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransitionManager.instance.ChangeLevel(nextSceneIndex, checkpointIndex); // Cambia de nivel al atravesar la puerta
        }
    }
}

