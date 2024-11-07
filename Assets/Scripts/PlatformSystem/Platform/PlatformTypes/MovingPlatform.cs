using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformSystem
{


    public class MovingPlatform : Platform
    {
        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;
        [SerializeField] private float speed = 2f;

        private Vector2 targetPosition;

        protected override void Awake()
        {
            base.Awake();
            
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 0;  // Asegúrate de que la gravedad no afecte el movimiento
            }
            targetPosition = pointB.position;
        }

        private void Update()
        {
            MovePlatform();
        }

        private void MovePlatform()
        {
            if (rb != null)
            {
                rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime));

                if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
                {
                    targetPosition = targetPosition == (Vector2)pointA.position ? (Vector2)pointB.position : (Vector2)pointA.position;
                }
            }
        }
    }
}
