using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{
    private bool isGrabbing = false;
    private Follower follower;


    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Assigning follower " + GetComponent<Follower>());
        follower = GetComponent<Follower>();
    }

    public override void Interact()
    {
        isGrabbing = !isGrabbing;
        follower.target = Player;
        follower.enabled = isGrabbing;
    }
}
