using UnityEngine;

public class BasicDoor : MonoBehaviour
{
    [SerializeField] private int nextSceneIndex; // �ndice de la escena a cargar
    [SerializeField] private int startCheckPoint; // �ndice del punto de control inicial en la siguiente escena

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Encuentra el SceneTransitionManager y llama a ChangeScene con ambos valores
            FindObjectOfType<SceneTransitionManager>().ChangeScene(nextSceneIndex, startCheckPoint);
        }
    }
}
