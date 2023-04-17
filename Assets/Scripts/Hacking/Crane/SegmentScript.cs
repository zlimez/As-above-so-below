using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentScript : MonoBehaviour
{

    private Transform arm;
    private Transform joint;

    public float armLength;
    public float angle;
    public Vector3 startPoint;
    public Vector3 endPoint;

    // Use this for initialization
    void Start()
    {
        // Debug.Log("Hello! I am " + name);
        arm = this.transform.Find("Arm");
        joint = this.transform.Find("Joint");

        startPoint = this.transform.position;
        endPoint = new Vector3();
        angle = this.transform.rotation.z;

        SetSegmentEndPoint();
    }


    public void MoveSegment(Vector3 target)
    {

        Vector3 vectorToTarget = target - startPoint;
        angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90.0f;

        //Debug.Log("1:" + vectorToTarget);

        vectorToTarget.Normalize();
        //Debug.Log("2:" + vectorToTarget + " " + vectorToTarget.magnitude);

        vectorToTarget = Vector3.ClampMagnitude(vectorToTarget, armLength);
        //Debug.Log("3:" + vectorToTarget + " " + vectorToTarget.magnitude);

        vectorToTarget.Scale(new Vector3(-1.0f, -1.0f));
        //Debug.Log("4:" + vectorToTarget + " " + vectorToTarget.magnitude);

        startPoint = target + vectorToTarget;
    }

    public void SetArmLength(float al)
    {
        armLength = al;
    }

    public void SetSegmentEndPoint(float z = 0.0f)
    {

        float xTemp = armLength * (float)(Mathf.Sin(angle * Mathf.PI / 180f));
        float yTemp = armLength * (float)(Mathf.Cos(angle * Mathf.PI / 180f));

        endPoint.Set(
            startPoint.x - xTemp,
            startPoint.y + yTemp,
            z
            );
        /*
        if (this.name == "Segment 0") {
            GameObject.Find("Dot").transform.position = startPoint;
        }
        */
    }

    public void UpdateSegmentAngle()
    {

        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.rotation = q;
    }

    public void SetArmBeginning()
    {

        arm.transform.localPosition = new Vector2(arm.transform.localPosition.x, (arm.transform.localScale.y / 2.0f));
    }

}
