using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSeats : MonoBehaviour
{
    public Windmill WindmillBlades;
    public bool isMoving;
    public bool isMovingRight;
    public bool isTurning;

    private void FixedUpdate()
    {
        if (WindmillBlades.isRotating)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isTurning)
        {
            if (isMovingRight)
            {
                if (transform.rotation.y == 180)
                {
                    isTurning = false;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 20, transform.rotation.z);
                }
            }
            else
            {
                if (transform.rotation.y == 0)
                {
                    isTurning = false;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y - 20, transform.rotation.z);
                }
            }
        }

        if (!isMoving)
        {
            return;
        }
        else
        {

        }
    }
}
