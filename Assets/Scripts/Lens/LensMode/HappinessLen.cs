using Lens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Lens
{
    public class MagicLensHappiness : MagicLen
    {
        [SerializeField] private PlayerMove playerMove; // Referencia al controlador del jugador
        [SerializeField] private float dashForce = 10f; // Magnitud del dash

        public override void Activate()
        {
            base.Activate(); // Llama al comportamiento base (opcional)

            if (playerMove != null)
            {
                Vector2 dashDirection = CalculateDashDirection();
                PlayerMove.Instance.PerformDash(dashDirection); // Asegúrate de que `dashDirection` sea un Vector2.

            }
            else
            {
                Debug.LogWarning("PlayerMove no está asignado en MagicLensHappiness.");
            }
        }

        private Vector2 CalculateDashDirection()
        {
            // Calcula la dirección del dash basada en la posición del lente
            Vector2 lensPosition = transform.position;
            Vector2 playerPosition = playerMove.transform.position;
            return (lensPosition - playerPosition).normalized;
        }
    }
}
