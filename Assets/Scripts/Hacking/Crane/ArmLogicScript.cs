using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmLogicScript : MonoBehaviour
{

    [SerializeField]
    private float INDICATOR_MOVE_SPEED = 4.0f;
    [SerializeField]
    private float INDICATOR_MOVE_TIGHTNESS = 0.01f;

    // Bounding box to constrain the grab range
    public Vector3 boundingBoxCenter;
    public Vector3 boundingBoxSize;

    public GameObject armSegmentUpper;
    public GrabZoneCollisionDetection grabZoneCollider;
    public GameObject rangeIndicator;
    public GameObject armSegmentMid;
    private Bounds bounds; // The Bounds object

    public GameObject head;
    public GameObject headSprite;
    public Transform grabPosTransform;
    public GameObject headFollowPoint;
    public GameObject grabIndicator;

    public Vector3 startPosition;
    public Vector3 startRotation;
    public bool isGrabbing;

    private Vector3 grabPosLerped;
    private Vector3 grabPos;
    private GameObject curGrabbedObject;

    // Use this for initialization
    void Start()
    {
        startPosition = armSegmentMid.transform.position;
        startRotation = new Vector3(0.0f, 0.0f, 0.0f);

        grabPos = grabPosTransform.position;
        grabPosLerped = grabPos;

        SetNewArmLenghts();
        isGrabbing = false;

        boundingBoxCenter.z = transform.position.z;
    }

    // When you select the object, a red border will show the bounds
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
    void Update()
    {
        // Set ui hints, This is the border around the crane
        UpdateUI();

        // Grab / ungrab using Enter
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isGrabbing)
            {
                StartGrab();
            }
            else
            {
                StartUngrab();
            }
        }

        // Move the indicator according to keyboard input
        Vector3 moveDir = new Vector3();
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += Vector3.right;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDir += Vector3.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDir = Vector3.down;
        }

        // Set grab Pos This is where the head of the crane should be 
        grabPos += moveDir.normalized * Time.deltaTime * INDICATOR_MOVE_SPEED;

        // Otherwise if using mouse to move crane 
        if (Input.GetButton("Fire1") && !PointerOverUI())
        {
            Ray grabPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
            grabPos = new Vector3(grabPoint.origin.x, grabPoint.origin.y, 0.0f);
        }

        // Constrain the grab position within the bounds
        grabPos = bounds.ClosestPoint(grabPos);

        // Lerp the position to smooth our the transition
        grabPosLerped = Vector3.Lerp(grabPosLerped, grabPos, INDICATOR_MOVE_TIGHTNESS);

        // Calculate the transform of the numerous arms using IK given the goal position
        MoveSegmentsToTarget();

        // Move the claw sprite to follow the tip of the crane 
        head.transform.position = headFollowPoint.transform.position;

        // Move the currently grab object
        if (isGrabbing)
        {
            curGrabbedObject.transform.position = head.transform.position;
        }

        grabIndicator.transform.position = armSegmentUpper.GetComponent<SegmentScript>().endPoint + new Vector3(0.0f, -1.2f, 0.0f);
        grabIndicator.transform.position = grabPos; // debug
    }

    private void StartUngrab()
    {
        curGrabbedObject.GetComponent<Grabbable>().OnUngrab();
        curGrabbedObject = null;
        isGrabbing = false;
        headSprite.GetComponent<Grabbable>().OnUngrab();
    }

    private void StartGrab()
    {
        // Invokes the Ongrab() of the thing we are trying to grab
        List<Collider> colList = grabZoneCollider.GetComponent<GrabZoneCollisionDetection>().triggeringColliders;
        if (colList == null || colList.Count == 0)
        {
            return;
        }

        Collider col = colList[0];
        {
            // Debug.Log("Invoking Grab() in: " + col.name);
            curGrabbedObject = col.gameObject;
            Grabbable curGrabbedObjectGrabable = col.gameObject.GetComponent<Grabbable>();

            if (curGrabbedObjectGrabable)
            {
                curGrabbedObjectGrabable.OnGrab();
            }
        }

        // Invokes the Ongrab() of the claw/head of the crane. This makes it open/close
        headSprite.GetComponent<Grabbable>().OnGrab();

        isGrabbing = true;
    }

    // Updates the the bounding box and hints around the crane
    private void UpdateUI()
    {
        bounds = new Bounds(boundingBoxCenter, boundingBoxSize);
        rangeIndicator.GetComponent<SpriteRenderer>().size = boundingBoxSize + new Vector3(2.0f, 2.0f, 0); // The Vector magic number here is  the size of the selection box that we move around
        rangeIndicator.transform.position = new Vector3(bounds.center.x, bounds.center.y, rangeIndicator.transform.position.z);
    }

    public bool PointerOverUI()
    {

        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    // Basically FABRIK algorithim where you go from the lowest segment and push it towards the start of 
    private void MoveSegmentsToTarget()
    {

        Vector3 grabPosOffset = grabPosLerped + new Vector3(0.0f, 1.2f, 0.0f);
        armSegmentUpper.GetComponent<SegmentScript>().MoveSegment(grabPosOffset);
        armSegmentUpper.GetComponent<SegmentScript>().SetSegmentEndPoint(startPosition.z);

        // From the highest segment For each lower segment, link it  to the one above
        armSegmentMid.GetComponent<SegmentScript>().MoveSegment(armSegmentUpper.GetComponent<SegmentScript>().startPoint);
        armSegmentMid.GetComponent<SegmentScript>().SetSegmentEndPoint(startPosition.z);

        armSegmentMid.GetComponent<SegmentScript>().startPoint = startPosition;

        // From the lowest segment For each upper segment, link it  to the one below
        armSegmentMid.GetComponent<SegmentScript>().SetSegmentEndPoint(startPosition.z);
        armSegmentUpper.GetComponent<SegmentScript>().startPoint = armSegmentMid.GetComponent<SegmentScript>().endPoint;


        // drawing
        armSegmentMid.transform.position = armSegmentMid.GetComponent<SegmentScript>().startPoint;
        armSegmentMid.GetComponent<SegmentScript>().UpdateSegmentAngle();

        armSegmentUpper.transform.position = armSegmentUpper.GetComponent<SegmentScript>().startPoint;
        armSegmentUpper.GetComponent<SegmentScript>().UpdateSegmentAngle();
    }

    public void SetNewArmLenghts()
    {

        armSegmentUpper.GetComponent<SegmentScript>().SetArmLength(1.5f);
        armSegmentUpper.GetComponent<SegmentScript>().SetSegmentEndPoint(startPosition.z);

        armSegmentMid.GetComponent<SegmentScript>().SetArmLength(1.5f);
        armSegmentMid.GetComponent<SegmentScript>().SetSegmentEndPoint(startPosition.z);
    }
}
