using UnityEngine;

public class InvisiblePlatformPair : MonoBehaviour
{
    [Header("Plataformas vinculadas")]
    [SerializeField] private InvisibleMovingPlatform _platformA;
    [SerializeField] private InvisibleMovingPlatform _platformB;

    private void Update()
    {
        if (_platformA == null || _platformB == null) return;

        // Si A ya se activó pero B sigue invisible, activamos B
        if (!_platformA.IsInvisible && _platformB.IsInvisible)
        {
            _platformB.ActivatePlatform();
        }
        // Si B ya se activó pero A sigue invisible, activamos A
        else if (!_platformB.IsInvisible && _platformA.IsInvisible)
        {
            _platformA.ActivatePlatform();
        }
    }
}