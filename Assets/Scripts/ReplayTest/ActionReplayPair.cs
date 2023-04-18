using Chronellium.EventSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionReplayPair : MonoBehaviour
{
    public GameObject spiritualObject;
    public GameObject realObject;

    private bool isInReplayMode;
    private int currentReplayIndex;
    private List<ActionReplayRecord> actionReplayRecords = new List<ActionReplayRecord>();

    private Vector3 spiritualObjectLastPosition;
    private GameEvent EnterRealWorld = new GameEvent("Enter Real World");
    private GameEvent EnterSpiritWorld = new GameEvent("Enter Spirit World");

    // Note: Assumes Real object rigidbody isKinematic is set to true.

    // Start is called before the first frame update
    void Start()
    {
        spiritualObjectLastPosition = spiritualObject.transform.position;
        EventManager.StartListening(EnterRealWorld, Replay);
        EventManager.StartListening(EnterSpiritWorld, Record);
    }

    private void Record(object input = null)
    {
        //EventManager.StopListening(EnterRealWorld, Replay);
        isInReplayMode = false;
    }

    private void Replay(object input = null)
    {
        //EventManager.StopListening(EnterRealWorld, Replay);
        isInReplayMode = true;
    }

    private void FixedUpdate()
    {
        if (isInReplayMode == false)
        {
            Vector3 positionShifted = spiritualObject.transform.position - spiritualObjectLastPosition;
            positionShifted.y = -positionShifted.y;

            actionReplayRecords.Add(new ActionReplayRecord { position = positionShifted, rotation = spiritualObject.transform.rotation });
            spiritualObjectLastPosition = spiritualObject.transform.position;
        }
        else
        {
            int nextIndex = currentReplayIndex + 1;

            if (nextIndex < actionReplayRecords.Count)
            {
                SetrealObjectTransform(nextIndex);
            }
            else
            {
                Debug.Log("Replay complete");
                actionReplayRecords.Clear();
                currentReplayIndex = 0;
            }
        }
    }

    private void SetrealObjectTransform(int index)
    {
        currentReplayIndex = index;

        ActionReplayRecord actionReplayRecord = actionReplayRecords[index];

        Debug.Log(actionReplayRecord.position);

        realObject.transform.position += actionReplayRecord.position;
        realObject.transform.rotation = actionReplayRecord.rotation;
    }
}
