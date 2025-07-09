using System.Collections;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Transform _startPoint;   // Punto de inicio de la piedra
    [SerializeField] private Transform _targetPoint;  // Punto de destino de la piedra
    [SerializeField] private float _speed = 5f;       // Velocidad de la piedra

    private Rigidbody2D _rb;
    private bool isMoving = false;  // Indica si la piedra está en movimiento

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_startPoint)
        {
            transform.position = _startPoint.position;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            // Movimiento hacia el destino
            transform.position = Vector3.MoveTowards(transform.position, _targetPoint.position, _speed * Time.deltaTime);

            // Detener la piedra si alcanza el destino
            if (Vector3.Distance(transform.position, _targetPoint.position) < 0.2f)
            {
                StopObstacle();
                Debug.Log("Piedra ha llegado al destino final.");
            }
        }
    }

    // Activar el movimiento inicial de la piedra
    public void ActivateMovement()
    {
        isMoving = true;
    }

    // Detener la piedra completamente
    private void StopObstacle()
    {
        isMoving = false;
        _rb.bodyType = RigidbodyType2D.Static; // Cambia el Rigidbody a estático para evitar empujarla
    }

    // Detectar si la piedra entra en el pozo
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hole"))
        {
            Debug.Log("Piedra ha caído al pozo.");
            isMoving = false;

            // Desacelerar suavemente
            StartCoroutine(SmoothStop());
        }
    }

    private IEnumerator SmoothStop()
    {
        while (_rb.velocity.magnitude > 0.1f)
        {
            _rb.velocity *= 0.9f; // Reduce gradualmente la velocidad
            yield return new WaitForFixedUpdate();
        }

        _rb.velocity = Vector2.zero; // Asegúrate de detener completamente
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Static;
    }

}

