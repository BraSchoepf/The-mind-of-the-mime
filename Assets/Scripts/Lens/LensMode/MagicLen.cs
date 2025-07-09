using UnityEngine;

namespace Lens
{
    public class MagicLen : MonoBehaviour
    {
        public enum LensMode { Reveal, Destroy, Happiness }  // Enum para definir los modos de la lente
        public LensMode currentMode = LensMode.Reveal;  // Modo inicial en Reveal
        public Color revealColor = Color.red;
        public Color destroyColor = Color.blue;
        public Color happinessColor = Color.magenta;
        public float activationRange = 0.5f;

        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;
        private SpriteMask _revealMask;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _revealMask = GetComponentInChildren<SpriteMask>();

            UpdateLensColor();  // Asigna el color inicial basado en el modo actual
        }

        private void UpdateLensColor()
        {
            if (_spriteRenderer != null)
            {
                if (currentMode == LensMode.Reveal)
                    _spriteRenderer.color = revealColor;
                else if (currentMode == LensMode.Destroy)
                    _spriteRenderer.color = destroyColor;
                else if (currentMode == LensMode.Happiness)
                    _spriteRenderer.color = happinessColor; 

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
            if (currentMode == LensMode.Reveal && collision.CompareTag("HiddenPlatform"))
            {
                IInvisiblePlatform platform = collision.GetComponent<IInvisiblePlatform>();
                if (platform != null)
                {
                    platform.ActivateSprite();
                }
            }
        }

        private void Update()
        {
            MoveLensWithMouse();

            if (currentMode == LensMode.Happiness && Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vector2 dashDirection = (Vector2)transform.position - (Vector2)Camera.main.transform.position;
                dashDirection.Normalize();
                PlayerMove.Instance.PerformDash(dashDirection);

            }

            if (Input.GetMouseButtonDown(0))  
            {
                Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, activationRange);

                foreach (Collider2D obj in objectsInRange)
                {
                    if (currentMode == LensMode.Reveal && obj.CompareTag("HiddenPlatform"))
                    {
                        IInvisiblePlatform platform = obj.GetComponent<IInvisiblePlatform>();
                        if (platform != null)
                        {
                            platform.EnableCollision();
                            platform.ActivateSprite();
                            platform.DeactivateMaskInteraction();
                        }
                    }
                    else if (currentMode == LensMode.Destroy && obj.CompareTag("Obstacle"))
                    {
                        Destroy(obj.gameObject);  // Destruye el objeto Obstacle
                    }
                }
            }

            // Cambiar el modo de la lente con una tecla, por ejemplo "Q"
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleLensMode();
            }
        }

        public virtual void Activate()
        {
            Debug.Log("Activando el modo de lente base");
            // Comportamiento genérico de activación
        }

        private void ToggleLensMode()
        {
            // Alterna de manera cíclica entre los tres modos
            switch (currentMode)
            {
                case LensMode.Reveal:
                    currentMode = LensMode.Destroy;
                    break;
                case LensMode.Destroy:
                    currentMode = LensMode.Happiness;
                    break;
                case LensMode.Happiness:
                    currentMode = LensMode.Reveal;
                    break;
            }

            UpdateLensColor();  // Actualiza el color según el modo actual
        }

    }
}







