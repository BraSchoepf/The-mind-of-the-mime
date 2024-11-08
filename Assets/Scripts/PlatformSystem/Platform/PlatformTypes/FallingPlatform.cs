using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _waitingTime = 2f;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float timeBeforeDisappear = 2f;
    [SerializeField] private float timeBeforeReset = 2f;
    [SerializeField] private float vibrationIntensity = 0.05f;
    [SerializeField] private float vibrationDuration = 0.5f;

    private Vector3 _initialPosition;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private bool _fall;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();  // Asigna el componente Collider2D de la plataforma
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

    private IEnumerator HandleFalling(Collider2D playerCollider)
    {
        yield return new WaitForSeconds(_waitingTime);
        _fall = true;

        yield return StartCoroutine(VibratePlatform());

        // Ignora la colisión con el jugador temporalmente
        Physics2D.IgnoreCollision(_collider, playerCollider, true);

        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.AddForce(new Vector2(0.1f, 0));

        yield return new WaitForSeconds(timeBeforeDisappear);

        ResetPlatform();

        // Rehabilita la colisión con el jugador
        Physics2D.IgnoreCollision(_collider, playerCollider, false);
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

        transform.position = _initialPosition;

        if (_rb != null)
        {

            if (_rb.bodyType == RigidbodyType2D.Dynamic)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
                _rb.velocity = Vector2.zero;
                _rb.angularVelocity = 0f;
            }
        }
    }
}










