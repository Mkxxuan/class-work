using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim; 

    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;

    [Header("Dash info")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;

    private int facingDir = 1;
    private bool facingRight = true;

    private float xInput;

    [Header("Collision info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;


    // 初始更新
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

   

    // 实时更新
    void Update()
    {
        movement();
        CheckInput();
        AnimatorControllers();
        FlipController();
        CollisionChecks();

        dashTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashTime = dashDuration;
        }

        


    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(rb);
        }
    }

    private void movement()
    {
        if (dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed * 2,0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
            
    }

    private void Jump(Rigidbody2D rb)
    {
        if(isGrounded)
        {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTime > 0);

    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
        


    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        
    }

}
