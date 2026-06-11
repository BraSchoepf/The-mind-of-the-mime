using System.Collections;
using UnityEngine;

/// <summary>
/// Maneja la rotación visual del sprite al hacer wall jump.
/// Adjuntarlo al GameObject hijo que contiene el SpriteRenderer/Animator,
/// NO en el root del personaje.
/// </summary>
public class PlayerVisualRoot : MonoBehaviour
{
    [Header("Wall Jump Flip")]
    [SerializeField] private float _flipDuration = 0.35f;   // duración del barrel roll
    [SerializeField] private AnimationCurve _flipCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float _flipDegrees = 360f;     // 360 = roll completo, 180 = medio flip

    private Coroutine _flipRoutine;

    /// <summary>
    /// Llamado por PlayerMove al ejecutar un wall jump.
    /// </summary>
    public void TriggerWallJumpFlip()
    {
        if (_flipRoutine != null)
            StopCoroutine(_flipRoutine);

        _flipRoutine = StartCoroutine(DoFlip());
    }

    private IEnumerator DoFlip()
    {
        float elapsed = 0f;
        Quaternion startRot = transform.localRotation;

        while (elapsed < _flipDuration)
        {
            elapsed += Time.deltaTime;
            float t = _flipCurve.Evaluate(elapsed / _flipDuration);
            float angle = Mathf.Lerp(0f, _flipDegrees, t);
            transform.localRotation = startRot * Quaternion.Euler(0f, angle, 0f);
            yield return null;
        }

        // Asegurar que termina limpio
        transform.localRotation = startRot;
        _flipRoutine = null;
    }
}