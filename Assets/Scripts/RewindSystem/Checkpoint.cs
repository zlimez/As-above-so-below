using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

[RequireComponent(typeof(ActivationClauses))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameEvent snapshotEvent;
    [SerializeField] private string shotDescription;
    [SerializeField] private Sprite screenshot;
    private ActivationClauses activationClauses;

    void Awake()
    {
        activationClauses = GetComponent<ActivationClauses>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && activationClauses.IsSatisfied())
        {
            EventLedger.Instance.RecordEvent(snapshotEvent);
            SnapshotManager.Instance.TakeSnapshot(shotDescription, screenshot);
        }
    }
}
