using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillRotator : MonoBehaviour
{
    [SerializeField] private float peakRotationSpeed;
    [SerializeField] private float rotationAcceleration;
    // + for anticlockwise - for clockwise
    private float currRotationVel;
    private bool isStopped = true;
    public bool IsClockwise { get; private set; }
    private bool isPowerOffSeq = false;
    private IEnumerator powerDownRoutine;

    public void PowerOn() {
        this.enabled = true;
        if (powerDownRoutine != null) StopCoroutine(powerDownRoutine);
        powerDownRoutine = PowerDown();
        isPowerOffSeq = false;
        isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPowerOffSeq) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            IsClockwise = false;
            isStopped = false;
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            IsClockwise = true;
            isStopped = false;
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            isStopped = true;
        }

        if (isStopped) {
            if (currRotationVel == 0) return;
            currRotationVel = Mathf.Sign(currRotationVel) * Mathf.Max(0, Mathf.Abs(currRotationVel) - rotationAcceleration * Time.deltaTime);
        } else {
                if (IsClockwise) {
                currRotationVel -= rotationAcceleration * Time.deltaTime;
            } else {
                currRotationVel += rotationAcceleration * Time.deltaTime;
            }
        }

        currRotationVel = Mathf.Clamp(currRotationVel, -peakRotationSpeed, peakRotationSpeed);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.rotation.eulerAngles.z + currRotationVel * Time.deltaTime);
    }

    public void StartPowerDown() {
        isPowerOffSeq = true;
        isStopped = true;
        StartCoroutine(powerDownRoutine);
    }

    // When disengaged
    IEnumerator PowerDown() {
        while (Mathf.Abs(currRotationVel) > 0) {
            currRotationVel = Mathf.Sign(currRotationVel) * Mathf.Max(0, Mathf.Abs(currRotationVel) - rotationAcceleration * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.rotation.eulerAngles.z + currRotationVel * Time.deltaTime);
            yield return null;
        }
        powerDownRoutine = null;
        this.enabled = false;
    }
}
