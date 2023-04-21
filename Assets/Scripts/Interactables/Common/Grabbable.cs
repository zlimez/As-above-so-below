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
        follower = GetComponent<Follower>();
    }

    public override void Interact()
    {
        if (!Player.GetComponent<Grabber>().ToggleGrabState(gameObject)) return;

        isGrabbed = !isGrabbed;
        follower.target = Player;
        follower.enabled = isGrabbed;
        
        if (StateManager.realm == Realm.otherWorld) {
            Player.GetComponent<SwimController>().IsRotationFrozen = isGrabbed;
        }
    }
}
