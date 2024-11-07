using System.Collections;
using UnityEngine;

namespace PlatformSystem
{
    public class FallingPlatform : Platform
    {
        [SerializeField] private float timeBeforeDisappear = 2f;  // Tiempo antes de desaparecer
        [SerializeField] private float timeBeforeReset = 2f;      // Tiempo antes de regresar a la posición original
        [SerializeField] private float vibrationIntensity = 0.05f; // Intensidad de la vibración
        [SerializeField] private float vibrationDuration = 0.5f;  // Duración de la vibración

        private Vector3 initialPosition;

        protected override void Awake()
        {
            base.Awake();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
            initialPosition = transform.position; 
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(HandleFalling());
            }
        }

        private IEnumerator HandleFalling()
        {
          
            Fall();

            yield return new WaitForSeconds(0.1f);

            yield return StartCoroutine(VibratePlatform());

            yield return new WaitForSeconds(timeBeforeDisappear);

            gameObject.SetActive(true);

            yield return new WaitForSeconds(timeBeforeReset);

            ResetPlatform();
        }

        private IEnumerator VibratePlatform()
        {
            Vector3 originalPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < vibrationDuration)
            {
                transform.position = originalPosition + (Vector3)Random.insideUnitCircle * vibrationIntensity;
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition;
        }

        private void ResetPlatform()
        {
            gameObject.SetActive(true);

            transform.position = initialPosition;

            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;

                if (rb.bodyType == RigidbodyType2D.Dynamic)
                {
                    rb.velocity = Vector2.zero;  
                    rb.angularVelocity = 0f;     
                }
            }
        }

        protected override void Fall()
        {
            base.Fall();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; 
                rb.gravityScale = 1.5f;
            }
        }
    }
}






