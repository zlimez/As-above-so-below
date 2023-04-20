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
    private bool isGrounded = false;

    void OnEnable() {
        mainCamera.SetFollowTransform(cameraFollowPoint);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void LateUpdate() {
        if (rb.velocity.sqrMagnitude > 0) {
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

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
