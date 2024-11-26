using System.Collections;
using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D _rb;

    [Header("Displacement")]
    [SerializeField] private float _groundSpeed = 5f;
    [SerializeField] private float _airSpeed = 3f;
    [Range(0, 0.3f)][SerializeField] private float _motionSoftener;
    private Vector2 _input;
    private Vector3 _velocity = Vector3.zero;
    private bool _lokingRigth;
    private float _horizontalMovement;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector3 _boxDimension;
    [SerializeField] private bool _isGrounded;
    private bool _jump = false;

    [Header("WallJump")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Vector3 _boxWallDimension;
    [SerializeField] private float _slidinVelocity;
    private bool _isWall;
    private bool _isClingingToWall; // Nuevo: para aferrarse a la pared
    private bool _wallJumpBlocked;  // Nuevo: para evitar saltos consecutivos en la misma pared
    [SerializeField] private float _jumpForceWallY;
    [SerializeField] private float _jumpForceWallX;
    [SerializeField] private float _jumpTimeWall;
    private bool _jumpingWall;

    [Header("Animation")]
    private Animator _animator;

    [Header("Raycast")]
    [SerializeField] private float _raycastDistance = 1f; // Distancia ajustable desde el Inspector
    [SerializeField] private Transform _raycastOrigin; // Nueva variable pública para elegir la posición de origen del Raycast
    private bool _isLookingAtWall;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float _speed = _isGrounded ? _groundSpeed : _airSpeed;
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        _horizontalMovement = _input.x * _speed;

        _animator.SetFloat("Horizontal", Mathf.Abs(_horizontalMovement));
        _animator.SetFloat("SpeedY", _rb.velocity.y);

        _isLookingAtWall = IsLookingAtWall();

        // Detectar el salto
        if (Input.GetButtonDown("Jump"))
        {
            if (_isClingingToWall && !_wallJumpBlocked)
            {
                // Verificamos si el raycast detecta la pared en la dirección en la que está mirando
                if (IsLookingAtWall())
                {
                    // Si el raycast detecta la pared, bloqueamos el salto
                    return;
                }
                else
                {
                    // Si no detecta la pared, podemos saltar hacia la pared opuesta
                    JumpingFromTheWall();
                }
            }
            else if (_isGrounded)
            {
                _jump = true;
            }
        }

        // Permitir cambiar de dirección mientras está aferrado a la pared
        if (_isClingingToWall && _input.x != 0)
        {
            if ((_input.x < 0 && !_lokingRigth) || (_input.x > 0 && _lokingRigth))
            {
                Turn();
            }
        }
    }

    private void FixedUpdate()
    {
        // Detección de suelo
        _isGrounded = Physics2D.OverlapBox(_groundCheck.position ,_boxDimension, 0f, _groundLayer | (1 << LayerMask.NameToLayer("HiddenLayer") | (1 << LayerMask.NameToLayer("PlatformLayer"))));

        // Detección de pared
        _isWall = Physics2D.OverlapBox(_wallCheck.position, _boxWallDimension, 0f, _groundLayer);

        _animator.SetBool("_isGround", _isGrounded);

        // Lógica para aferrarse a la pared
        if (!_isGrounded && _isWall)
        {
            _isClingingToWall = true;

            // Bloquear el salto si sigue en la misma pared sin cambiar de dirección
            if ((_lokingRigth && _input.x > 0) || (!_lokingRigth && _input.x < 0))
            {
                _wallJumpBlocked = true;
            }
            else
            {
                _wallJumpBlocked = false; // Permite el salto si cambia de dirección
            }
        }
        else
        {
            _isClingingToWall = false;
            _wallJumpBlocked = false; // Resetea el bloqueo al salir de la pared
        }

        // Mover al jugador
        Move(_horizontalMovement * Time.fixedDeltaTime, _jump);

        // Deslizarse si está aferrado a la pared
        if (_isClingingToWall && !_jumpingWall)
        {
            _rb.velocity = new Vector2(0, Mathf.Clamp(_rb.velocity.y, -_slidinVelocity, float.MaxValue));
        }

        _jump = false;
    }

    public void Move(float move, bool hop)
    {
        if (!_jumpingWall)
        {
            Vector3 speedReach = new Vector2(move, _rb.velocity.y);
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, speedReach, ref _velocity, _motionSoftener);
        }

        if (move < 0 && !_lokingRigth)
        {
            Turn();
        }
        else if (move > 0 && _lokingRigth)
        {
            Turn();
        }

        if (_isGrounded && hop && !_isClingingToWall)
        {
            Jump();
        }
    }

    private void JumpingFromTheWall()
    {
        // Salta solo si no está bloqueado
        if (_isClingingToWall && !_wallJumpBlocked)
        {
            _isWall = false;
            _isClingingToWall = false;

            // Determina la dirección del salto basada en la posición del jugador
            Vector2 jumpDirection = _lokingRigth ? Vector2.left : Vector2.right;
            _rb.velocity = new Vector2(jumpDirection.x * _jumpForceWallX, _jumpForceWallY);

            // Reinicia el temporizador del salto de pared
            StopCoroutine(SwichJumpWall());
            StartCoroutine(SwichJumpWall());
        }
    }

    IEnumerator SwichJumpWall()
    {
        _jumpingWall = true;
        yield return new WaitForSeconds(_jumpTimeWall);
        _jumpingWall = false;
    }

    private void Jump()
    {
        _isGrounded = false;
        _rb.AddForce(new Vector2(0f, _jumpForce));
    }

    private void Turn()
    {
        _lokingRigth = !_lokingRigth;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Raycast que detecta si el jugador está mirando hacia la pared
    bool IsLookingAtWall()
    {
        // Raycast en la dirección opuesta a la que el jugador está mirando, usando el _raycastOrigin
        RaycastHit2D hit = Physics2D.Raycast(_raycastOrigin.position, _lokingRigth ? Vector2.left : Vector2.right, _raycastDistance, _groundLayer);

        // Si el raycast toca una pared, significa que está mirando hacia la pared
        return hit.collider != null;
    }

    // Dibujar el Raycast en el Editor para ver la dirección
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheck.position, _boxDimension);
        Gizmos.DrawWireCube(_wallCheck.position, _boxWallDimension);

        Gizmos.color = Color.red;

        // Dirección del raycast según si el jugador está mirando a la izquierda o a la derecha
        Vector2 raycastDirection = _lokingRigth ? Vector2.left : Vector2.right;

        // Dibujar la línea del raycast desde el origen hacia la dirección del raycast
        Gizmos.DrawLine(_raycastOrigin.position, (Vector2)_raycastOrigin.position + raycastDirection * _raycastDistance);
    }

}

