using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeepBreath.ReplaySystem;

public class ResetPuzzle : MonoBehaviour
{
    public GameObject normPlayer;
    public GameObject cableSeatsInitialPosition;
    public GameObject normPlayerInitialPosition;
    public ActionReplayPair windmillActionReplayPair;
    public CableSeats cableSeats;
    public GameObject windmillBlades;
    public Windmill windmill;
    public Worm worm;

    public void ResetPuzzle1()
    {
        //StopAllCoroutines();
        windmillActionReplayPair.ResetReplayRecords();
        windmillBlades.transform.eulerAngles = Vector3.zero;
        windmill.prevRotationZ = 0;
        windmill.isRotating = false;
        cableSeats.StopAllCoroutines();
        cableSeats.isMoving = false;
        cableSeats.transform.position = cableSeatsInitialPosition.transform.position;
        cableSeats.StopAllCoroutines();
        // Teleport player back to start
        cableSeats.isSeated = false;
        normPlayer.transform.position = normPlayerInitialPosition.transform.position;
        Rigidbody rb = normPlayer.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        worm.wormSprite.enabled = false;
        worm.ResetAttack();
    }
}
