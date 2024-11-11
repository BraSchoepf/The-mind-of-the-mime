using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool _isActivated = false; // Variable para controlar si ya fue activado

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_isActivated)
        {
            CheckPointSystem.instance.LastCheckPoint(gameObject);
            _isActivated = true; // Marcar el punto como activado
        }
    }

    private void OnDrawGizmos()
    {
        // Dibuja una esfera para visualizar el punto de control en la escena
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
