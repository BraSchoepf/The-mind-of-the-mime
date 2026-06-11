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
    private bool _lookingRight = true;
    private float _horizontalMovement;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _coyoteTime = 0.15f;
    private float _coyoteTimeCounter;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector3 _boxDimension;
    private bool _isGrounded;
    private bool _jump = false;

    [Header("Wall Slide")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Vector3 _boxWallDimension;
    [SerializeField] private float _slideSpeed = 2f;  // velocidad de caída en pared
    private bool _isWall;
    private bool _isSliding;

    [Header("Wall Jump")]
    [SerializeField] private float _wallJumpForceY = 14f;
    [SerializeField] private float _wallJumpForceX = 8f;
    [SerializeField] private float _controlLockDuration = 0.25f; // configurable
    private bool _controlLocked;
    private int _wallDirection; // -1 izq, 1 der

    [Header("Animation")]
    private Animator _animator;

    // Referencia al visual root para triggerear el flip
    private PlayerVisualRoot _visualRoot;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _visualRoot = GetComponentInChildren<PlayerVisualRoot>();
    }

    void Update()
    {
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        float speed = _isGrounded ? _groundSpeed : _airSpeed;
        _horizontalMovement = _controlLocked ? _horizontalMovement : _input.x * speed;

        _animator.SetFloat("Horizontal", Mathf.Abs(_input.x));
        _animator.SetFloat("SpeedY", _rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            if (_isSliding)
                TriggerWallJump();
            else if (_coyoteTimeCounter > 0f && (!_isWall || _isGrounded))
                _jump = true;
        }

        // Flip del sprite según dirección (solo si no hay control lock)
        if (!_controlLocked)
        {
            if (_input.x < 0 && _lookingRight) FlipCharacter();
            if (_input.x > 0 && !_lookingRight) FlipCharacter();
        }
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapBox(
            _groundCheck.position, _boxDimension, 0f,
            _groundLayer | (1 << LayerMask.NameToLayer("HiddenLayer"))
                         | (1 << LayerMask.NameToLayer("PlatformLayer"))
        );

        if (_isGrounded)
            _coyoteTimeCounter = _coyoteTime;
        else
            _coyoteTimeCounter -= Time.fixedDeltaTime;

        _isWall = Physics2D.OverlapBox(_wallCheck.position, _boxWallDimension, 0f, _groundLayer);
        _animator.SetBool("_isGround", _isGrounded);

        // Determinar si está haciendo wall slide
        _isSliding = !_isGrounded && _isWall;

        if (_isSliding)
        {
            // Guardar hacia qué lado está la pared
            _wallDirection = _lookingRight ? 1 : -1;

            // Clampear caída
            _rb.velocity = new Vector2(_rb.velocity.x,
                Mathf.Clamp(_rb.velocity.y, -_slideSpeed, float.MaxValue));
        }

        // Movimiento horizontal normal
        if (!_controlLocked)
        {
            Vector2 targetVelocity = new Vector2(_horizontalMovement, _rb.velocity.y);
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, _motionSoftener);
        }

        if (_isGrounded && _jump)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        _jump = false;
    }

    private void TriggerWallJump()
    {
        // Dirección opuesta a la pared
        int jumpDir = -_wallDirection;

        _rb.velocity = new Vector2(jumpDir * _wallJumpForceX, _wallJumpForceY);

        // Flip visual inmediato + triggerear barrel roll
        if (jumpDir > 0 && !_lookingRight) FlipCharacter();
        if (jumpDir < 0 && _lookingRight) FlipCharacter();

        StartCoroutine(LockControl());
    }

    IEnumerator LockControl()
    {
        _controlLocked = true;
        yield return new WaitForSeconds(_controlLockDuration);
        _controlLocked = false;
    }

    private void FlipCharacter()
    {
        _lookingRight = !_lookingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (_groundCheck) Gizmos.DrawWireCube(_groundCheck.position, _boxDimension);
        if (_wallCheck) Gizmos.DrawWireCube(_wallCheck.position, _boxWallDimension);
    }
}