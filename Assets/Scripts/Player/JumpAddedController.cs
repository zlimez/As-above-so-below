using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAddedController : MonoBehaviour
{
    public MainCamera mainCamera;
    public Transform cameraFollowPoint;
    public SpriteRenderer playerSprite;
    public Vector3 goalVelocity;
    public float moveSpeed = 5f;
    [SerializeField]
    private float continiousJumpForce = 0.7f;
    [SerializeField]
    private float initialJumpForce = 5f;
    private float totalJumpForce = 0f;
    private float maxJumpForce = 7f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded = false;
    private bool isJumping = false;
    public float jumpInputBufferTime = 0.15f;
    private bool jumpInputBuffered = false;
    private float jumpInputBufferTimer = 0f;
    private float timeSinceLastJump = 0f;

    void OnEnable()
    {
        mainCamera.SetFollowTransform(cameraFollowPoint, mainCamera.DefaultDistance);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = playerSprite.gameObject.GetComponent<Animator>();
    }

    void LateUpdate()
    {
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

        // increase the jump impulse with longer hold 
        if (isJumping && Input.GetKey(KeyCode.Space))
        {
            if (totalJumpForce < maxJumpForce)
            {
                totalJumpForce += continiousJumpForce;
                rb.AddForce(Vector2.up * continiousJumpForce, ForceMode.Impulse);
            }
        }
        if (isGrounded && jumpInputBuffered && timeSinceLastJump > 0.1f)
        {
            Debug.Log("start jump");
            animator.SetBool("isMoving", false);
            animator.SetBool("isJumping", true);
            totalJumpForce += initialJumpForce;
            rb.AddForce(Vector2.up * initialJumpForce, ForceMode.Impulse);
            jumpInputBuffered = false;
            isGrounded = false;
            isJumping = true;

            timeSinceLastJump = 0;
        }


        if (isJumping)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            if (hInput == 0)
            {
                animator.SetBool("isMoving", false);
            }
            else
            {
                bool isRight = hInput > 0;
                playerSprite.flipX = !isRight;
                animator.SetBool("isMoving", true);
            }
        }

        // Check if grounded, because sometimes it still think that the character is jumping
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position, Vector3.down, out hitInfo, 2f);
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 2f);

        // Update the grounded flag
        isGrounded = hit;
    }

    void Update()
    {
        timeSinceLastJump += Time.deltaTime;
        if (jumpInputBuffered)
        {
            jumpInputBufferTimer -= Time.deltaTime;

            if (jumpInputBufferTimer <= 0f)
            {
                jumpInputBuffered = false;
            }
        }

        // This is to prevent the player being stuck when
        // performign jumping too close to a collider
        if (isJumping)
        {
            StartCoroutine(CheckForStuck());
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInputBuffered = true;
            jumpInputBufferTimer = jumpInputBufferTime;
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
        Debug.Log("Stop jump");
        isGrounded = true;
        isJumping = false;
        animator.SetBool("isJumping", false);
        totalJumpForce = 0f;
    }
}
