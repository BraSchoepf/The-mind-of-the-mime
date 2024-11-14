using UnityEngine;

public class InvisibleMovingPlatform : MovingPlatform, IInvisiblePlatform
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    protected bool _isInvisible = true;

    // Llamar a la función Start de la clase base (MovingPlatform) para configurar el movimiento
    protected override void Start()
    {
        base.Start(); // Llama al Start de MovingPlatform

        // Inicializar componentes
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        // Hacer la plataforma invisible al principio
        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = false; // Invisible al principio
        }

        if (_collider != null)
        {
            _collider.isTrigger = true; // Se usa como Trigger para evitar colisiones
        }
    }

    // Hacer la plataforma visible y habilitar la colisión
    public void ActivatePlatform()
    {
        if (_isInvisible) // Solo activar si la plataforma está invisible
        {
            ActivateSprite(); // Llama al método de la interfaz para visibilidad
            EnableCollision(); // Llama al método de la interfaz para colisión
            _isInvisible = false;
            Debug.Log("Plataforma Invisible Activada");
        }
    }

    // Implementación de ActivateSprite de la interfaz IInvisiblePlatform
    public void ActivateSprite()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = true; // Asegurarse de que el sprite esté visible
            Debug.Log("Sprite activado en la plataforma");
        }
    }

    // Llamado en Update, si la plataforma está visible, permite que se mueva
    protected override void Update()
    {
        // Si está visible, permite el movimiento
        if (!_isInvisible)
        {
            base.Update(); // Llama al método Update de MovingPlatform para mover la plataforma
        }

        // Verifica si el jugador hizo clic derecho
        if (Input.GetMouseButtonDown(1)) // Clic derecho
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0.5f, LayerMask.GetMask("HiddenLayer"));


            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                ActivatePlatform(); // Activa la visibilidad y la colisión
            
            }
        }
    }

    // Función que desactiva la interacción con las máscaras de sprite (para hacer invisible)
    public void DeactivateMaskInteraction()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.None; // Cambiar la interacción a None
            Debug.Log("Interacción con la máscara desactivada.");
        }
    }

    // Implementación de la interfaz IInvisiblePlatform
    public void EnableCollision()
    {
        if (_collider != null)
        {
            _collider.isTrigger = false; // Habilita la colisión completa
            Debug.Log("Colisión de la plataforma activada.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.2f); // Visualización del rango de activación
    }
}


