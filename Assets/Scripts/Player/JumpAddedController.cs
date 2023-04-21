using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAddedController : MonoBehaviour
{
    public MainCamera mainCamera;
    public Transform cameraFollowPoint;
    public SpriteRenderer playerSprite;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded = false;

    void OnEnable()
    {
        mainCamera.SetFollowTransform(cameraFollowPoint);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = playerSprite.gameObject.GetComponent<Animator>();
    }

    void LateUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            bool isRight = rb.velocity.x > 0;
            playerSprite.flipX = !isRight;
        }

        mainCamera.Move(Time.smoothDeltaTime);
    }

    void FixedUpdate()
    {
        if (!isGrounded) return;

        float hInput = Input.GetAxis("Horizontal");
        Vector2 vel = rb.velocity;
        vel.x = hInput * moveSpeed;
        rb.velocity = vel;
        if (rb.velocity.x == 0)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isMoving", false);
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
