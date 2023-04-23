using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSeatsOW : MonoBehaviour
{
    public Windmill windmillOW;
    public bool isMoving;
    public bool isMovingRight;
    public Waypoints waypoints;

    private IEnumerator moveRight;
    private IEnumerator moveLeft;
    public float moveSpeed = 0.3f;

    private void Start()
    {
        moveRight = MoveRight();
        moveLeft = MoveLeft();
    }

    private void FixedUpdate()
    {
        if (windmillOW.isRotating)
        {
            Debug.Log("Windmill rotating");
            isMoving = true;
            if (windmillOW.isRotatingClockwise)
            {
                isMovingRight = true;
            }
            else
            {
                isMovingRight = false;
            }
        }
        else
        {
            isMoving = false;
        }

        if (!isMoving)
        {
            return;
        }
        else
        {
            if (isMovingRight)
            {
                StopCoroutine(moveRight);
                StopCoroutine(moveLeft);
                StartCoroutine(moveRight);
            }
            else
            {
                StopCoroutine(moveRight);
                StopCoroutine(moveLeft);
                StartCoroutine(moveLeft);
            }
        }
    }

    public IEnumerator MoveRight()
    {
        while (!waypoints.reachedEnd)
        {
            if (transform.position != waypoints.currentWaypoint.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints.currentWaypoint.position, moveSpeed);
            }
            else
            {
                waypoints.GetNextWaypoint();
                transform.position = Vector3.MoveTowards(transform.position, waypoints.currentWaypoint.position, moveSpeed);
            }
            yield return null;
        }
    }

    public IEnumerator MoveLeft()
    {
        while (!waypoints.reachedStart)
        {
            if (transform.position != waypoints.currentWaypoint.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints.currentWaypoint.position, moveSpeed);
            }
            else
            {
                waypoints.GetPrevWaypoint();
                transform.position = Vector3.MoveTowards(transform.position, waypoints.currentWaypoint.position, moveSpeed);
            }
            yield return null;
        }
    }
}
