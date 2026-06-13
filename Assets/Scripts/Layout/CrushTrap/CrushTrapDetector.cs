using UnityEngine;

public class CrushTrapDetector : MonoBehaviour
{
    [SerializeField] private CrushTrap _trap;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _trap.PlayerDetected();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            _trap.PlayerLeft();
    }
}