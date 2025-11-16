using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexPlayerController : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] float maxHorizontalSpeed = 7.5f;

    [Header("Jumping")]
    [SerializeField] float apexJumpHeight = 4f; // units
    [SerializeField] float timeToReachApex = 1.0f; // sec
    [SerializeField] float timeToFallToGround = 1.0f; // different falling gravity (sec)
    [SerializeField] LayerMask jumpableSurface;

    [Header("Debug Info")]
    [SerializeField] bool isGrounded;
    [SerializeField] float inputX;
    [SerializeField] bool jumpKeyPressed;
    [SerializeField] bool jumpKeyReleased;

    Rigidbody2D rb2D;
    Collider2D coll2D;
    SpriteRenderer spriteRenderer;
    float jumpVelocity;
    float fastFallGravityScale;

    // Awake is run the moment the script is run.
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update() is called every frame.
    private void Update()
    {
        jumpVelocity = 2 * apexJumpHeight / timeToReachApex;
        rb2D.gravityScale = -2 * apexJumpHeight / timeToReachApex / timeToReachApex / Physics2D.gravity.y;

        inputX = Input.GetAxisRaw("ComplexHorizontal"); // Gets the input on the horizontal axis set project settings

        isGrounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.W))
        {
            jumpKeyPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            jumpKeyReleased = true;
        }

        rb2D.gravityScale = getGravityScale();

        updateAnimation();
    }

    // By default, FixedUpdate() is run every 20ms.
    private void FixedUpdate()
    {
        Vector2 velocity = rb2D.velocity;

        velocity.x = inputX * maxHorizontalSpeed;

        if (jumpKeyPressed && isGrounded)
        {
            velocity.y = jumpVelocity;
        }

        jumpKeyPressed = false;
        jumpKeyReleased = false;

        Debug.Log(velocity);
        rb2D.velocity = velocity; // Sets the velocity, moving the player.
    }

    private float getGravityScale()
    {
        if (rb2D.velocity.y < 0.0f) // fast-fall gravity
        {
            return -2 * apexJumpHeight / timeToFallToGround / timeToFallToGround / Physics2D.gravity.y;
        }
        else // regular gravity
        {
            return -2 * apexJumpHeight / timeToReachApex / timeToReachApex / Physics2D.gravity.y;
        }
    }

    // Checks if the character is standing on the ground.
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll2D.bounds.center, coll2D.bounds.size, 0f, Vector2.down, 0.1f, jumpableSurface);
    }

    private void updateAnimation()
    {
        if (inputX > 0.0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputX < 0.0f)
        {
            spriteRenderer.flipX = true;
        }
    }
}
