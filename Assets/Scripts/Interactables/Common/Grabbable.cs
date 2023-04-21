using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeepBreath.Environment;

public class Grabbable : Interactable
{
    private bool isGrabbed = false;
    private Follower follower;


    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Assigning follower " + GetComponent<Follower>());
        follower = GetComponent<Follower>();
    }

    public override void Interact()
    {
        isGrabbed = !isGrabbed;
        follower.target = Player;
        follower.enabled = isGrabbed;
        
        if (StateManager.realm == Realm.otherWorld) {
            Player.GetComponent<SwimController>().IsRotationFrozen = isGrabbed;
        }
    }
}
