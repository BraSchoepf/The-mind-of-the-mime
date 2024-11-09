using UnityEngine;

namespace Lens
{
    public class MagicLen : MonoBehaviour
    {
        public Color lensColor = Color.red;
        public float activationRange = 0.5f;

        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;
        private SpriteMask _revealMask;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _revealMask = GetComponentInChildren<SpriteMask>();

            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = lensColor;
            }
        }

        private void MoveLensWithMouse()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _rb.MovePosition(mousePosition);

            if (_revealMask != null)
            {
                _revealMask.transform.position = mousePosition;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("HiddenPlatform"))
            {
                // Buscamos el componente IInvisiblePlatform
                IInvisiblePlatform platform = collision.GetComponent<IInvisiblePlatform>();
                if (platform != null)
                {
                    Debug.Log("Plataforma detectada por la lupa");
                    platform.ActivateSprite();
                }
                else
                {
                    Debug.LogWarning("No se encontró el componente IInvisiblePlatform en el objeto detectado.");
                }
            }
        }

        private void Update()
        {
            MoveLensWithMouse();

            if (Input.GetMouseButtonDown(1))  // Detecta clic derecho
            {
                Debug.Log("Clic derecho detectado, intentando activar colisión.");

                Collider2D[] platformsInRange = Physics2D.OverlapCircleAll(transform.position, activationRange);

                foreach (Collider2D platformCollider in platformsInRange)
                {
                    if (platformCollider.CompareTag("HiddenPlatform"))
                    {
                        IInvisiblePlatform platform = platformCollider.GetComponent<IInvisiblePlatform>();
                        if (platform != null)
                        {
                            Debug.Log("Activando colisión de la plataforma y sprite visibles");
                            platform.EnableCollision();  // Llamada a EnableCollision para activar colisión
                            platform.ActivateSprite();   // Asegura que el sprite esté visible
                            platform.DeactivateMaskInteraction();
                        }
                        else
                        {
                            Debug.LogWarning("No se encontró el componente IInvisiblePlatform en el objeto detectado.");
                        }
                    }
                }
            }
        }
    }
}









