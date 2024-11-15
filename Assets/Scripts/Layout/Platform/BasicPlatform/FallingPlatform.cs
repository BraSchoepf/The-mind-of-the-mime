using System.Collections;
using UnityEngine;

namespace PlatformAndLensSystem
{

    public class FallingPlatform : MonoBehaviour
    {
        [SerializeField] protected float _waitingTime = 2f;
        [SerializeField] protected float _rotationSpeed;

        [SerializeField] protected float timeBeforeDisappear = 2f;
        [SerializeField] protected float timeBeforeReset = 2f;
        [SerializeField] protected float vibrationIntensity = 0.05f;
        [SerializeField] protected float vibrationDuration = 0.5f;

        protected Vector3 _initialPosition;
        protected Rigidbody2D _rb;
        protected Collider2D _collider;
        protected bool _fall;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();  
            _initialPosition = transform.position;   // Guarda la posición inicial
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Collider2D playerCollider = other.collider;  // Obtiene el collider del jugador
                StartCoroutine(HandleFalling(playerCollider));
            }
        }

        protected virtual IEnumerator HandleFalling(Collider2D playerCollider)
        {
            yield return new WaitForSeconds(_waitingTime);

            yield return StartCoroutine(VibratePlatform());

            _fall = true;

            if (playerCollider != null && _collider != null)  // Verificar que ambos colliders no sean nulos
            {
                // Ignora la colisión con el jugador temporalmente
                Physics2D.IgnoreCollision(_collider, playerCollider, true);
            }

            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.AddForce(new Vector2(0.1f, 0));

            yield return new WaitForSeconds(timeBeforeDisappear);

            ResetPlatform();

            if (playerCollider != null && _collider != null)  // Verificar que ambos colliders no sean nulos
            {
                // Rehabilita la colisión con el jugador
                Physics2D.IgnoreCollision(_collider, playerCollider, false);
            }
        }

        protected virtual IEnumerator VibratePlatform()
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

        protected virtual void ResetPlatform()
        {
            gameObject.SetActive(true);

            // Restablecer la posición inicial
            transform.position = _initialPosition;

            // Restablecer la rotación a cero
            transform.rotation = Quaternion.identity;

            if (_rb != null)
            {
                // Si el Rigidbody2D es dinámico, restablece las restricciones y velocidad
                if (_rb.bodyType == RigidbodyType2D.Dynamic)
                {
                    _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    _rb.velocity = Vector2.zero;
                    _rb.angularVelocity = 0f;
                }
            }
        }
    }
}










