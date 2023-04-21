using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

public class SwimController : MonoBehaviour
{
    public MainCamera mainCamera;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float propulsionStrength = 3f;
    [SerializeField] private float drag = 1f;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private float RotationSharpness = 10f;
    [SerializeField] private float straightenInterval = 0.2f, straightenDelay = 0.1f;
    [SerializeField] private Transform cameraFollowPoint;
    private float idleTime = 0f;
    private IEnumerator straightenRoutine;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 currDirection;
    private bool lastFacingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = playerSprite.gameObject.GetComponent<Animator>();
    }

    void OnEnable()
    {
        mainCamera.SetFollowTransform(cameraFollowPoint);
        EventManager.StartListening(CommonEventCollection.ObjectPickedUp, 
            (o) => animator.SetBool("isHolding", true));
        EventManager.StartListening(CommonEventCollection.ObjectPutDown, 
            (o) => animator.SetBool("isHolding", false));
    }

    void OnDisable() 
    {
        EventManager.StopListening(CommonEventCollection.ObjectPickedUp, 
            (o) => animator.SetBool("isHolding", true));
        EventManager.StopListening(CommonEventCollection.ObjectPutDown, 
            (o) => animator.SetBool("isHolding", false));
    }

    void LateUpdate()
    {
        mainCamera.Move(Time.smoothDeltaTime);

        float zAngle = lastFacingRight ? Vector2.SignedAngle(Vector2.right, currDirection) : Vector2.SignedAngle(Vector2.left, currDirection);
        playerSprite.gameObject.transform.rotation = Quaternion.Slerp(playerSprite.gameObject.transform.rotation, Quaternion.Euler(0, 0, zAngle), 1 - Mathf.Exp(-RotationSharpness * Time.fixedDeltaTime));
    }

    void FixedUpdate()
    {
        // TODO: Add animator
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        currDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        if (currDirection.sqrMagnitude > 0)
        {
            if (Mathf.Abs(horizontalInput) > 0)
            {
                bool isRight = Mathf.Sign(horizontalInput) == 1;
                playerSprite.flipX = !isRight;
                lastFacingRight = isRight;
            }
        }
        else if (rb.velocity.sqrMagnitude < 0.0025)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 netForce = rb.velocity.sqrMagnitude < Mathf.Pow(maxSpeed, 2) ? currDirection * propulsionStrength - drag * rb.velocity : -drag * rb.velocity;
        rb.AddForce(netForce);
    }
}
