using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lens
{
    // Enum LensMode para los diferentes modos de la lupa
    public enum LensMode
    {
        Reveal,
        Observe,
        Destroy,
        Scanner
    }

    // Clase base MagicLens
    public abstract class MagicLen : MonoBehaviour
    {
        public LayerMask revealLayer;
        public float revealDistance = 0.5f;

        protected Rigidbody2D rb2D;
        protected LensMode currentMode;

        protected virtual void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                rb2D.isKinematic = true;
            }
            else
            {
                Debug.LogError("RigidBody2D is null");
            }

            Collider2D collider = GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
            else
            {
                Debug.LogError("Collider2D is null");
            }
        }

        protected virtual void Update()
        {
            MoveLensWithMouse();
            SwitchLensMode();
            HandleObjects();
        }

        private void MoveLensWithMouse()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb2D.MovePosition(mousePosition);
        }

        protected virtual void HandleObjects()
        {
            Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, revealDistance, revealLayer);

            foreach (Collider2D obj in objectsInRange)
            {
                switch (currentMode)
                {
                    case LensMode.Reveal:
                        RevealObjects(obj);
                        break;
                    case LensMode.Observe:
                        ObserveObjects(obj);
                        break;
                    case LensMode.Destroy:
                        DestroyObjects(obj);
                        break;
                    case LensMode.Scanner:
                        ScannerObjects(obj);
                        break;
                    default:
                        Debug.LogError("Unknown LensMode");
                        break;
                }
            }
        }

        protected virtual void SwitchLensMode()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentMode = (LensMode)(((int)currentMode + 1) % System.Enum.GetValues(typeof(LensMode)).Length);
                SetLensColor();
                Debug.Log($"Current lens mode: {currentMode}");
            }
        }

        protected virtual void SetLensColor()
        {
            // Define el color según el modo actual
            Color lensColor = currentMode.GetLensColor();
            // Aquí puedes aplicar el color a un componente visual de la lente, si tienes uno
        }

        // Métodos abstractos para que las clases hijas los implementen
        protected abstract void RevealObjects(Collider2D obj);
        protected abstract void ObserveObjects(Collider2D obj);
        protected abstract void DestroyObjects(Collider2D obj);
        protected abstract void ScannerObjects(Collider2D obj);
    }

    // Métodos de extensión para LensMode
    public static class LensModeExtensions
    {
        public static Color GetLensColor(this LensMode mode)
        {
            return mode switch
            {
                LensMode.Reveal => Color.red,
                LensMode.Observe => Color.blue,
                LensMode.Destroy => Color.green,
                LensMode.Scanner => Color.yellow,
                _ => Color.white
            };
        }
    }
}

