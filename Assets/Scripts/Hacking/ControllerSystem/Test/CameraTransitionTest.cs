using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionTest : MonoBehaviour
{
    public CameraFocusable[] cameraFocusables;
    public int focusedIndex;

    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && focusedIndex < cameraFocusables.Length && focusedIndex >= 0) {
            cameraFocusables[focusedIndex].Focus();
        }
    }
}
