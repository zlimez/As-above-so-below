using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private GameObject player;
    private bool playerInRange;
    private bool isGrabbing;
    Vector3 initialDistance;
    Rigidbody rigidbody;
    float lerpSpeed = 10; // Move speed during grab

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        TryGrab();

        if (isGrabbing)
        {
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            transform.position = Vector3.Lerp(transform.position, player.transform.position + initialDistance, Time.deltaTime * lerpSpeed);
            //Vector3 newPosition = Vector3.Lerp(transform.position, player.transform.position + initialDistance, Time.deltaTime * lerpSpeed);
            //rigidbody.MovePosition(newPosition);
        }
        else
        {
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
    }

    public void TryGrab()
    {
        // InputManager.InteractButtonActivated
        if (Input.GetKeyDown(KeyCode.Return) && playerInRange)
        {
            Grab();
        }

        if (Input.GetKeyUp(KeyCode.Return) || !playerInRange)
        {
            isGrabbing = false;
        }
    }
    private void Grab()
    {
        initialDistance = transform.position - player.transform.position;
        isGrabbing = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            playerInRange = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            playerInRange = false;
        }
    }

    private bool IsPlayer(GameObject otherObject)
    {
        return otherObject.CompareTag("Player");
    }
}
