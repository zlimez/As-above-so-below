using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tuples;
using DeepBreath.Environment;

public class Recorder<T> : MonoBehaviour
{
    private float firstFrameTimestamp = -1;
    private Queue<T> keyFrames = new Queue<T>();

    public void StartRecord() {
        if (firstFrameTimestamp == -1) {
            firstFrameTimestamp = StateManager.TimeSinceOtherWorldEntered;
        }
    }

    public void StopRecord() {

    }

    // IEnumerator Record() {

    // }
}
