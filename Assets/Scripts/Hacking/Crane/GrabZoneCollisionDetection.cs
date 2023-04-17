using System.Collections.Generic;
using UnityEngine;

public class GrabZoneCollisionDetection : MonoBehaviour
{
    public List<Collider> triggeringColliders = new List<Collider>(); // List to store triggering colliders

    private void OnTriggerEnter(Collider other)
    {
        // Add the triggering collider to the list
        triggeringColliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the triggering collider from the list
        triggeringColliders.Remove(other);
    }
}
