using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayableManager : MonoBehaviour
{
    public static ReplayableManager Instance;
    [SerializeField] private float fps;

    void Awake() {
        if (Instance == null) Instance = this;
    }
}
