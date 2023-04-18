using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABKaspo
{
    public class FPSCamera : MonoBehaviour
    {
        //publics
        [Header("Principal Settings")]
        public Transform PlayerBody;
        [Header("Mouse")]
        public float mouseSensivity = 80.0f;
        //Private
        float mouseX;
        float mouseY;
        float xRotation;
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }


        void Update()
        {
            //Data
            mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;
            //Rotation
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            PlayerBody.Rotate(Vector3.up * mouseX);

        }
    }
}