// InvisibleFallingPlatform implementa la interfaz IInvisiblePlatform
using UnityEngine;
using PlatformAndLensSystem;


public class InvisibleFallingPlatform : FallingPlatform, IInvisiblePlatform
{
    private SpriteRenderer _spriteRenderer;
    private Collider2D _invisibleCollider;

    protected override void Start()
    {
        base.Start(); // Llamamos al Start de la clase base
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _invisibleCollider = GetComponent<Collider2D>();

        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = false; // Invisible al principio
        }

        if (_invisibleCollider != null)
        {
            _invisibleCollider.isTrigger = true; // Se usa como trigger para evitar colisiones
        }
    }

    public void ActivateSprite()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = true;
            Debug.Log("Sprite de la plataforma activado.");
        }
    }

    public void EnableCollision()
    {
        if (_invisibleCollider != null)
        {
            _invisibleCollider.isTrigger = false; // Habilita la colisi�n completa
            Debug.Log("Colisi�n de la plataforma activada.");
        }
    }

    public void DeactivateMaskInteraction()
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            Debug.Log("Interacci�n con la m�scara desactivada.");
        }
    }
}
