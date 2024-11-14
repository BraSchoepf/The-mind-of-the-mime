using UnityEngine;

public class InvisibleMovingPlatform : MovingPlatform, IInvisiblePlatform
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private bool _isInvisible = true;

    // Llamar a la funci�n Start de la clase base (MovingPlatform) para configurar el movimiento
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

    // Hacer la plataforma visible y habilitar la colisi�n
    public void ActivatePlatform()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = true; // Activar el sprite
        }

        if (_collider != null)
        {
            _collider.isTrigger = false; // Desactivar el trigger y permitir colisiones
        }

        _isInvisible = false;
        Debug.Log("Plataforma Invisible Activada");
    }

    // Implementaci�n de ActivateSprite de la interfaz IInvisiblePlatform
    public void ActivateSprite()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = true; // Asegurarse de que el sprite est� visible
            Debug.Log("Sprite activado en la plataforma");
        }
    }

    // Llamado en Update, si la plataforma est� visible, permite que se mueva
    protected override void Update()
    {
        // Si est� visible, permite el movimiento
        if (!_isInvisible)
        {
            base.Update(); // Llama al m�todo Update de MovingPlatform para mover la plataforma
        }

        // Verifica si el jugador hizo clic derecho
        if (Input.GetMouseButtonDown(1)) // Clic derecho
        {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
                if (_isInvisible)
            {
                ActivatePlatform(); // Activa la visibilidad y la colisi�n
            }
        }
    }

    // Funci�n que desactiva la interacci�n con las m�scaras de sprite (para hacer invisible)
    public void DeactivateMaskInteraction()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.None; // Cambiar la interacci�n a None
            Debug.Log("Interacci�n con la m�scara desactivada.");
        }
    }

    // Implementaci�n de la interfaz IInvisiblePlatform
    public void EnableCollision()
    {
        if (_collider != null)
        {
            _collider.isTrigger = false; // Habilita la colisi�n completa
            Debug.Log("Colisi�n de la plataforma activada.");
        }
    }
}


