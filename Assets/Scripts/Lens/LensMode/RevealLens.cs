using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lens
{
    public class RevealLens : MagicLen
    {
        protected override void RevealObjects(Collider2D obj)
        {
            if (Input.GetMouseButtonDown(1))
            {

                if (obj.CompareTag("HiddenPlatform"))
                {
                    obj.gameObject.SetActive(true);
                    Debug.Log("Enabled Hidden Platform");
                }
            }
        }

        // Métodos vacíos para cumplir con los requerimientos abstractos
        protected override void ObserveObjects(Collider2D obj) { }
        protected override void DestroyObjects(Collider2D obj) { }
        protected override void ScannerObjects(Collider2D obj) { }

        protected override void SetLensColor()
        {
            Color lensColor = currentMode.GetLensColor();
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = lensColor;
            }
            else
            {
                Debug.LogWarning("No se encontró un SpriteRenderer en el objeto.");
            }
        }
    }
}

