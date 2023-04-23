using UnityEngine;

using Chronellium.EventSystem;
public class WoodFallTrigger : MonoBehaviour
{
    private bool hasTriggered;
    public Rigidbody rigidbody;

    // Tag based so each entity can represent a group
    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        rigidbody.useGravity = true;

        hasTriggered = true;

    }
}