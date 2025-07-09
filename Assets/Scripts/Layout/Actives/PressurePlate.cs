using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
   public MovingObstacle obstacle;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            if (obstacle != null)
            {
                obstacle.ActivateMovement();
                Debug.Log("active plate: stone is moving");
            }
            
        }
    }
}
