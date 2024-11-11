using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D _rb;

    [Header("displacement")]

    [SerializeField] private float _groundSpeed = 5f;
    [SerializeField] private float _airSpeed = 3f;
    [Range(0, 0.3f)][SerializeField] private float _motionSoftener;
    private Vector2 _input;
    private Vector3 _velocity = Vector3.zero;
    private bool _lokingRigth;
    private float _horizontalMovement;


    [Header("jump")]

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
    private bool _slidIng;
    [SerializeField] private float _jumpForceWallY;
    [SerializeField] private float _jumpForceWallX;
    [SerializeField] private float _jumpTimeWall;
    private bool _jumpingWall;




    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float _speed = _isGrounded ? _groundSpeed : _airSpeed;
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        _horizontalMovement = _input.x * _speed;


        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
        }
        if (!_isGrounded && _isWall && _input.x != 0)
        {
            _slidIng = true;
        }
        else
        {
            _slidIng = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (_input.y >= 0)
            {
                _jump = true;
            }




        }
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapBox(_groundCheck.position, _boxDimension, 0f, _groundLayer | (1 << LayerMask.NameToLayer("HiddenLayer")));
        _isWall = Physics2D.OverlapBox(_wallCheck.position, _boxWallDimension, 0f, _groundLayer | (1 << LayerMask.NameToLayer("HiddenLayer")));

        // Reiniciar jumptimewall si el jugador está en el suelo
        if (_isGrounded)
        {
            _jumpingWall = false;
        }

        Move(_horizontalMovement * Time.fixedDeltaTime, _jump);

        _jump = false;

        if (_slidIng)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -_slidinVelocity, float.MaxValue));
        }
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
        if (_isGrounded && hop && !_slidIng)
        {
            Jump();
        }
        if (_isWall && hop && _slidIng)
        {
            JumpingFromTheWall();
        }

    }
    private void JumpingFromTheWall()
    {
        _isWall = false;
        _rb.velocity = new Vector2(_jumpForceWallX * -_input.x, _jumpForceWallY);

        // Reinicia el temporizador cada vez que el jugador hace un salto de pared
        StopCoroutine(SwichJumpWall());  // Detenemos cualquier ejecución previa para reiniciar
        StartCoroutine(SwichJumpWall());
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


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_groundCheck.position, _boxDimension);
        Gizmos.DrawWireCube(_wallCheck.position, _boxWallDimension);
    }
}
