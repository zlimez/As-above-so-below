using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionReplayPairTest : MonoBehaviour
{
    public GameObject initialObject;
    public GameObject copyObject;

    private bool isInReplayMode;
    private int currentReplayIndex;
    private Rigidbody rigidbody;
    private List<ActionReplayRecord> actionReplayRecords = new List<ActionReplayRecord>();

    private Vector3 initialObjectLastPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = copyObject.GetComponent<Rigidbody>();
        initialObjectLastPosition = initialObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isInReplayMode = !isInReplayMode;

            /*
            if (isInReplayMode)
            {
                //SetCopyObjectTransform(0);
                //rigidbody.isKinematic = true;
            }
            else
            {
                //SetCopyObjectTransform(actionReplayRecords.Count - 1);
                //rigidbody.isKinematic = false;
            }
            */
        }
    }

    private void FixedUpdate()
    {
        if (isInReplayMode == false)
        {
            actionReplayRecords.Add(new ActionReplayRecord { position = initialObject.transform.position - initialObjectLastPosition, rotation = initialObject.transform.rotation });
            initialObjectLastPosition = initialObject.transform.position;
        }
        else
        {
            int nextIndex = currentReplayIndex + 1;

            if (nextIndex < actionReplayRecords.Count)
            {
                SetCopyObjectTransform(nextIndex);
            }
            else
            {
                Debug.Log("Replay complete");
                actionReplayRecords.Clear();
                currentReplayIndex = 0;
            }
        }
    }

    private void SetCopyObjectTransform(int index)
    {
        currentReplayIndex = index;

        ActionReplayRecord actionReplayRecord = actionReplayRecords[index];

        Debug.Log(actionReplayRecord.position);

        copyObject.transform.position += actionReplayRecord.position;
        copyObject.transform.rotation = actionReplayRecord.rotation;
    }
}
