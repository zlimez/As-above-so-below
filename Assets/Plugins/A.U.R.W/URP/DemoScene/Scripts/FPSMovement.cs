using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABKaspo
{
    public class FPSMovement : MonoBehaviour
    {
        public CharacterController characterController;
        public float walk = 10.0f;
        public float run = 15.0f;
        public float jumpHeight = 3.0f;
        public float graviyForce = 9.8f;
        public Transform groundCheck;
        public float sphereRadius = 0.3f;
        public LayerMask groundMask;
        bool isPause = false;
        bool isGrounded;
        Vector3 velocity;
        void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            //Waln&Run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                characterController.Move(move * run * Time.deltaTime);
            }
            else
            {
                characterController.Move(move * walk * Time.deltaTime);
            }
            //gravity
            velocity.y -= graviyForce * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            //gravityFix
            isGrounded = Physics.CheckSphere(groundCheck.position, sphereRadius, groundMask);
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            //jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * graviyForce);
            }
        }
    }
}