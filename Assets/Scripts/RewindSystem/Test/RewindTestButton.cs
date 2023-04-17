using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewindTestButton : MonoBehaviour
{
    [Tooltip("Press N to submit")] public int rewindSteps;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            RewindBy();
            return;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            SnapshotManager.Instance.TakeSnapshot("Testing");
        }
    }

    public void RewindBy()
    {
        SnapshotManager.Instance.LoadSnapshot(rewindSteps);
    }
}
