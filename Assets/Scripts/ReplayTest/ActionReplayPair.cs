using Chronellium.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReplayPair : MonoBehaviour
{
    public GameObject spiritualObject;
    public GameObject realObject;

    private bool isInReplayMode;
    private Queue<ActionReplayRecord> actionReplayRecords = new Queue<ActionReplayRecord>();

    private Vector3 spiritualObjectLastPosition;

    void OnEnable() {
        EventManager.StartListening(StaticEvent.Core_SwitchToRealWorld, Replay);
        EventManager.StartListening(StaticEvent.Core_SwitchToOtherWorld, Record);
    }

    void OnDisable() {
        EventManager.StopListening(StaticEvent.Core_SwitchToRealWorld, Replay);
        EventManager.StopListening(StaticEvent.Core_SwitchToOtherWorld, Record);
    }

    // Note: Assumes Real object rigidbody isKinematic is set to true.
    void Start()
    {
        spiritualObjectLastPosition = spiritualObject.transform.position;
    }

    private void Record(object input = null)
    {
        isInReplayMode = false;
    }

    private void Replay(object input = null)
    {
        isInReplayMode = true;
    }

    private void FixedUpdate()
    {
        if (isInReplayMode == false)
        {
            Vector3 positionShifted = spiritualObject.transform.position - spiritualObjectLastPosition;

            actionReplayRecords.Enqueue(new ActionReplayRecord { deltaPosition = positionShifted, rotation = spiritualObject.transform.rotation });
            spiritualObjectLastPosition = spiritualObject.transform.position;
        }
        else
        {

            if (actionReplayRecords.Count > 0)
            {
                SetrealObjectTransform();
            } else
            {
                Debug.Log("Replay complete");
            }
        }
    }

    private void SetrealObjectTransform()
    {
        ActionReplayRecord actionReplayRecord = actionReplayRecords.Dequeue();

        realObject.transform.position += actionReplayRecord.deltaPosition;
        realObject.transform.rotation = actionReplayRecord.rotation;
    }
}
