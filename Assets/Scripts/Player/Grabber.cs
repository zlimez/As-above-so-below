using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

public class Grabber : MonoBehaviour
{
    private GameObject GrabbedObject;
    [SerializeField] private Animator animator;
    
    public bool ToggleGrabState(GameObject interactedObject) {
        if (interactedObject == GrabbedObject) {
            // Let go
            animator.SetBool("isHolding", false);
            GrabbedObject = null;
            return true;
        }

        if (GrabbedObject == null) {
            // Grab
            GrabbedObject = interactedObject;
            animator.SetBool("isHolding", true);
            return true;
        }

        return false;
    }
}
