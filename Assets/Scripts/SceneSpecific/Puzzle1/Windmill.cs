using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    public GameObject WindmillBlades;
    public bool isRotating;
    public bool isRotatingClockwise;
    public float prevRotationZ;

    private void FixedUpdate()
    {
        float currentRotationZ = transform.rotation.z;
        if (currentRotationZ == prevRotationZ)
        {
            isRotating = false;
            return;
        }
        else if (currentRotationZ > prevRotationZ)
        {
            isRotating = true;
            prevRotationZ = currentRotationZ;
            isRotatingClockwise = true;
        }
        else
        {
            isRotating = true;
            prevRotationZ = currentRotationZ;
            isRotatingClockwise = false;
        }
    }
}
