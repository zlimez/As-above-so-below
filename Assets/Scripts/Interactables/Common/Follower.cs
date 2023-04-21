using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject target;
    private Vector3 offsetToTarget;
    private Rigidbody rb;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable() {
        offsetToTarget = target.transform.position - transform.position;
    }

    void Update() {
        rb.MovePosition(target.transform.position - offsetToTarget);
    }
}
