using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WindmillPlayer : Interactable
{
    [SerializeField] private Transform[] engagePts;
    [SerializeField] private float swimmerSpeed = 2.5f;
    private SwimController swimmer;
    private bool isEngaged;
    private WindmillRotator rotator;
    private IEnumerator swimToRoutine;
    private IEnumerator swimWithRoutine;

    void Awake() {
        rotator = GetComponent<WindmillRotator>();
    }

    public override void Interact()
    {
        if (isEngaged) {
            isEngaged = false;
            swimmer.Automated = false;
            if (swimToRoutine != null) {
                StopCoroutine(swimToRoutine);
                rotator.StartPowerDown();
            }
        } else {
            isEngaged = true;
            swimmer = Player.GetComponent<SwimController>();
            swimmer.Automated = true;
            swimToRoutine = MovePlayerToBlade();
            StartCoroutine(swimToRoutine);
        }
    }

    // IEnumerator MovePlayerWithBlade() {
    //     if (rotator.IsClockwise) {
    //         swimmer.transform.
    //     }
    // }

    IEnumerator MovePlayerToBlade() {
        Transform nearestEngagePt = engagePts.Aggregate(engagePts[0], (pt1, pt2) => {
            if ((pt2.position - swimmer.transform.position).sqrMagnitude < (pt1.position - swimmer.transform.position).sqrMagnitude) {
                return pt2;
            }
                return pt1;
            }
        );

        Vector3 toEngagePt = nearestEngagePt.position - swimmer.Grabpoint.position;
        Vector3 destPt = swimmer.transform.position + toEngagePt;
        float timeNeeded = toEngagePt.magnitude / swimmerSpeed;
        float timeElapsed = 0;
        bool isToRight = toEngagePt.x > 0;
        this.swimmer.SwimmerRenderer.flipX = !isToRight;
        Vector3 initPosition = swimmer.transform.position;
        while (timeElapsed < timeNeeded) {
            swimmer.transform.position = VectorUtils.CubicLerpVector(initPosition, destPt, timeElapsed / timeNeeded);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        swimmer.SwimmerAnimator.SetBool("isGrabbing", true);
        swimToRoutine = null;
        rotator.PowerOn();
    }
}
