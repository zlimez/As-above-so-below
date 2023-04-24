using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSeats : Interactable
{
    public GameObject normPlayer;
    public GameObject seatPosition;
    private float playerMoveSpeed = 0.4f;

    public bool isSeated;
    public AudioSource SitDown;

    public override void Interact()
    {
        JumpAddedController playerJumpController = normPlayer.GetComponent<JumpAddedController>();
        Debug.Log("Interacting");
        if (isSeated)
        {
            isSeated = false;
            normPlayer.GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            SitDown.Play();
            isSeated = true;
            normPlayer.GetComponent<Rigidbody>().useGravity = false;
        }
        playerJumpController.jumpEnabled = !isSeated;
    }

    private void FixedUpdate()
    {
        if (isSeated)
        {
            normPlayer.transform.position = Vector3.MoveTowards(normPlayer.transform.position, seatPosition.transform.position, playerMoveSpeed);
        }
    }
}
