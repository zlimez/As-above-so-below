using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.Hacking;

public class LeftRightMovement : HackedBehaviour
{
    public MainCamera mainCamera;
    public float speed = 5.0f; // speed of movement
    private Rigidbody rb; // rigidbody component of the GameObject

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        mainCamera.Move(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (HackedBehaviour.IsFrozen) return;
        float horizontalInput = Input.GetAxis("Horizontal"); // get input from the horizontal axis (A/D keys or left/right arrow keys)

        Vector3 movement = transform.right * horizontalInput * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
