// InvisiblePlatform implementa la interfaz IInvisiblePlatform
using UnityEngine;

public class InvisiblePlatform : MonoBehaviour, IInvisiblePlatform
{
    private SpriteRenderer spriteRenderer;
    private Collider2D _coll;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _coll = GetComponent<Collider2D>();

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false; // Invisible al principio
        }

        if (_coll != null)
        {
            _coll.isTrigger = true; // Inicialmente en modo trigger
        }
    }

    public void ActivateSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            Debug.Log("Sprite de la plataforma activado.");
        }
    }

    public void EnableCollision()
    {
        if (_coll != null)
        {
            _coll.isTrigger = false; // Habilita la colisión completa
            Debug.Log("Colisión de la plataforma activada.");
        }
    }

    public void DeactivateMaskInteraction()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            Debug.Log("Interacción con la máscara desactivada.");
        }
    }
}




