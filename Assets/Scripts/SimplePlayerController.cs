using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float maxHorizontalSpeed = 7.5f;
    [SerializeField] float jumpVelocity = 7.5f;
    [SerializeField] float variableJumpHeightMultiplier = 0.5f;
    [SerializeField] LayerMask jumpableSurface;

    [Header("Debug Info")]
    [SerializeField] bool isGrounded;
    [SerializeField] float inputX;
    [SerializeField] bool jumpKeyPressed;
    [SerializeField] bool jumpKeyReleased;

    Rigidbody2D rb2D;
    Collider2D coll2D;
    SpriteRenderer spriteRenderer;

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
        inputX = Input.GetAxisRaw("SimpleHorizontal"); // Gets the input on the horizontal axis set project settings

        isGrounded = IsGrounded();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpKeyPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            jumpKeyReleased = true;
        }

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
        if (jumpKeyReleased && velocity.y > 0.0f)
        {
            velocity.y = rb2D.velocity.y * variableJumpHeightMultiplier;
        }

        jumpKeyPressed = false;
        jumpKeyReleased = false;

        Debug.Log(velocity);
        rb2D.velocity = velocity; // Sets the velocity, moving the player.
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
