using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusable : MonoBehaviour
{
    public MainCamera mainCamera;
    
    void Awake() {
        mainCamera = FindObjectOfType<MainCamera>();
    }

    public void Focus() {
        mainCamera.SetFollowTransform(transform);
    }
}
