using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAddedController : MonoBehaviour
{
    public MainCamera mainCamera;
    public Transform cameraFollowPoint;
    public SpriteRenderer playerSprite;
    public float moveSpeed = 5f;
    private float jumpForce = 0.7f;
    private float totalJumpForce = 0f;
    private float maxJumpForce = 7f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded = false;
    private bool isJumping = false;

    void OnEnable() {
        mainCamera.SetFollowTransform(cameraFollowPoint, mainCamera.DefaultDistance);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = playerSprite.gameObject.GetComponent<Animator>();
    }

    void LateUpdate() {
        mainCamera.Move(Time.smoothDeltaTime);
    }

    void FixedUpdate()
    {
        // Allow moving while jumping
        //if (!isGrounded) return;

        float hInput = Input.GetAxis("Horizontal");
        Vector2 vel = rb.velocity;
        if (isJumping)
        {
            // restrict the horizontal velocity when jumping
            vel.x = 0.4f * hInput * moveSpeed;
        }
        else
        {
            vel.x = hInput * moveSpeed;
        }
        rb.velocity = vel;
        if (hInput == 0) {
            animator.SetBool("isMoving", false);
        } else {
            bool isRight = hInput > 0;
            playerSprite.flipX = !isRight;
            animator.SetBool("isMoving", true);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isJumping", true);
            totalJumpForce += jumpForce;
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = true;
        }

    }

    void Update() 
    {
        // This is to prevent the player being stuck when
        // performign jumping too close to a collider
        if (isJumping)
        {
            StartCoroutine(CheckForStuck());
        }

        // increase the jump impulse with longer hold 
        if (isJumping && Input.GetKey(KeyCode.Space))
        {
            if (totalJumpForce < maxJumpForce)
            {
                totalJumpForce += jumpForce; 
                rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            }
        }

            
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("Player collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            StopJumping();
        }
    }

    IEnumerator CheckForStuck()
    {
        float yPos = this.transform.position.y;
        yield return new WaitForSeconds(Time.smoothDeltaTime * 2);
        if (this.transform.position.y == yPos)
        {
            // Player is stucked
            StopJumping();
        }
    }

    void StopJumping()
    {
        isGrounded = true;
        isJumping = false;
        animator.SetBool("isJumping", false);
        totalJumpForce = 0f;
    }
}
