using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteBlink : MonoBehaviour
{
    [Header("Colores del parpadeo")]
    [SerializeField] private Color _lightColor = Color.white;
    [SerializeField] private Color _darkColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    [Header("Velocidad")]
    [SerializeField] private float _blinkSpeed = 2f; // ciclos por segundo, aprox

    private SpriteRenderer _spriteRenderer;
    private float _t;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _t += Time.deltaTime * _blinkSpeed;
        // PingPong hace que _lerp vaya de 0 a 1 y de vuelta a 0 suavemente
        float lerp = Mathf.PingPong(_t, 1f);
        _spriteRenderer.color = Color.Lerp(_darkColor, _lightColor, lerp);
    }
}