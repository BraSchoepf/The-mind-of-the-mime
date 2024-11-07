using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float groundCheckDistance;
    public LayerMask groundLayer;

    private bool _isGrounded = false;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    public void Move()
    {
        float moveDirection = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveDirection * speed, _rb.velocity.y);

        _rb.velocity = movement;
    }

    public void Jump()
    {
        // Comprueba si el jugador está tocando el suelo
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // Maneja la entrada del salto
        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);

        // Si está en el suelo y se presiona una tecla de salto
        if (_isGrounded && jumpKeyPressed)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
        else if (!_isGrounded && jumpKeyPressed)
        {
            Debug.Log("Intento de salto sin estar en el suelo.");
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}
