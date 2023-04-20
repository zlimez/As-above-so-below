using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSeats : Interactable
{
    public Windmill windmillBlades;
    public bool isMoving;
    public bool isMovingRight;
    public bool isTurning;
    public Waypoints waypoints;
    public GameObject player;

    private IEnumerator moveRight;
    private IEnumerator moveLeft;
    float lerpSpeed = 10; // Move speed

    private bool isSeated;

    private void Start()
    {
        moveRight = MoveRight();
        moveLeft = MoveLeft();
    }

    public override void Interact()
    {
        Debug.Log("Interacting");
        if (isSeated)
        {
            isSeated = false;
            player.GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            isSeated = true;
            player.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void FixedUpdate()
    {
        if (isSeated)
        {
            Debug.Log("Set player transform");
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }

        if (windmillBlades.isRotating)
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
                transform.position = Vector3.Lerp(transform.position, waypoints.currentWaypoint.position, Time.deltaTime * lerpSpeed);
            }
            else
            {
                waypoints.GetNextWaypoint();
                transform.position = Vector3.Lerp(transform.position, waypoints.currentWaypoint.position, Time.deltaTime * lerpSpeed);
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
                transform.position = Vector3.Lerp(transform.position, waypoints.currentWaypoint.position, Time.deltaTime * lerpSpeed);
            }
            else
            {
                waypoints.GetPrevWaypoint();
                transform.position = Vector3.Lerp(transform.position, waypoints.currentWaypoint.position, Time.deltaTime * lerpSpeed);
            }
            yield return null;
        }
    }
}
