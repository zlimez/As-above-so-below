using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using Chronellium.EventSystem;

// FIXME: Permit 0 snapshots
// NOTE: Current implementation assumes snapshot cannot be changed when rewind ui is opened
public class RewindUi : MonoBehaviour
{
    private List<Snapshot> snapshots = new List<Snapshot>();
    private int currIndex = -1;
    private bool isOpen;
    public bool isForcedRewind;
    private Coroutine activeCoroutine;
    [SerializeField] Text description;
    [SerializeField] Image screenshot;
    [SerializeField] TMP_Text sceneName, sceneOrder;
    [SerializeField] GameObject rewindBox, noPoint, onePoint, multiplePoints, pointsForTimeline;
    [SerializeField]
    PlayableDirector moveLeft, moveRight, moveLeftEnd, moveRightEnd,
        endMoveLeft, endMoveRight, twoPointsMoveLeft, twoPointsMoveRight;
    [SerializeField] AudioClip hover, select;
    [SerializeField] SnapshotTakenHint snapshotHint;

    // for testing purpose only
    [SerializeField] Sprite dummyScreenshot;

    // TESTCODE
    void Start()
    {
        //InitialiseTesting();
    }

    void OnEnable()
    {
        EventManager.StartListening(CommonEventCollection.ForcedRewind, ForcedRewindEnabled);
        EventManager.StartListening(CoreEventCollection.SnapshotTaken, ShowHint);
    }

    void OnDisable()
    {
        EventManager.StopListening(CommonEventCollection.ForcedRewind, ForcedRewindEnabled);
        EventManager.StopListening(CoreEventCollection.SnapshotTaken, ShowHint);
    }

    void Initialise()
    {
        // reset points
        pointsForTimeline.SetActive(false);
        noPoint.SetActive(false);
        onePoint.SetActive(false);
        multiplePoints.SetActive(false);
        snapshots = new List<Snapshot>();

        foreach (Snapshot snapshot in SnapshotManager.Instance.Snapshots)
        {
            snapshots.Insert(0, snapshot);
        }

        currIndex = snapshots.Count - 1;

        if (snapshots.Count == 1)
        {
            onePoint.SetActive(true);
        }
        else if (snapshots.Count > 1)
        {
            multiplePoints.SetActive(true);
        }
        else
        {
            // no snapshot in the system
            noPoint.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Is forced rewind: " + isForcedRewind);
        // Debug.Log("UiStatus: " + UIStatus.isOpen);
        if (UiStatus.IsDisabled()) return;

        if (isForcedRewind)
        {
            // no action allowed in forced rewind
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!rewindBox.activeInHierarchy && !UiStatus.IsOpen)
            {
                Open();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (rewindBox.activeInHierarchy)
            {
                Close();
                return;
            }
        }

        if (isOpen)
        {
            if (currIndex == -1)
            {
                // navigation and selection not allowed
                // when there is no snapshot in the system
                return;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                LeftSelect();
                return;

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RightSelect();
                return;
            }

            if (Input.GetButtonDown("Submit"))
            {
                Select();
                return;
            }
        }
    }

    void Select()
    {
        AudioManager.Instance.StartPlayingUiAudio(select);
        SnapshotManager.Instance.LoadSnapshot(snapshots.Count - 1 - currIndex);
        isForcedRewind = false;
        Close();
    }

    void LeftSelect()
    {
        if (currIndex == 0)
        {
            // no longer to navigate to the left as the current
            // snapshot is the first snapshot
            return;
        }

        AudioManager.Instance.StartPlayingUiAudio(hover);

        currIndex--;
        // Debug.Log($"Move to the left {currIndex} with snapshots count {snapshots.Count}");

        bool isToEnd = currIndex == 0;

        bool isFromEnd = currIndex == snapshots.Count - 2;

        UpdateUi("left", snapshots.Count > 2, isToEnd, isFromEnd);
        UpdatePanels();
    }

    void RightSelect()
    {
        if (currIndex == snapshots.Count - 1)
        {
            // no longer to navigate to the right as the current
            // snapshot is the last snapshot
            return;
        }

        AudioManager.Instance.StartPlayingUiAudio(hover);

        currIndex++;
        // Debug.Log($"Move to the right {currIndex} with snapshots count {snapshots.Count}");

        bool isToEnd = currIndex == snapshots.Count - 1;

        bool isFromEnd = currIndex == 1;

        UpdateUi("right", snapshots.Count > 2, isToEnd, isFromEnd);
        UpdatePanels();
    }

    void UpdateUi(string direction, bool largerThanTwo, bool isToEnd, bool isFromEnd)
    {
        // activate different timeline for different Ui updates
        // according to how many snapshots there are in total
        // and the direction of navigation

        onePoint.SetActive(false);
        multiplePoints.SetActive(false);
        ResetTimeline();

        if (!largerThanTwo)
        {
            if (direction == "left")
            {
                twoPointsMoveLeft.Play();
                return;
            }
            else if (direction == "right")
            {
                twoPointsMoveRight.Play();
                return;
            }

            //  Debug.Log("direction parameter should either be left or right");
            return;
        }

        if (direction == "left")
        {
            if (isToEnd)
            {
                moveLeftEnd.Play();
                return;
            }
            else if (isFromEnd)
            {
                endMoveLeft.Play();
                return;
            }
            moveLeft.Play();
            return;
        }

        if (direction == "right")
        {
            if (isToEnd)
            {
                moveRightEnd.Play();
                return;
            }
            else if (isFromEnd)
            {
                endMoveRight.Play();
                return;
            }
            moveRight.Play();
            return;
        }

        // Debug.Log("direction parameter should either be left or right");

    }

    void UpdatePanels()
    {
        if (currIndex == -1) return;
        description.text = snapshots[currIndex].Description;
        sceneName.text = snapshots[currIndex].SceneName;
        screenshot.sprite = snapshots[currIndex].Screenshot;

        string currOrder = (currIndex + 1).ToString();
        string count = snapshots.Count.ToString();
        sceneOrder.text = currOrder + "/" + count;
    }

    void ResetTimeline()
    {
        moveLeft.Stop();
        moveLeft.time = 0;

        moveRight.Stop();
        moveRight.time = 0;

        moveLeftEnd.Stop();
        moveLeftEnd.time = 0;

        moveRightEnd.Stop();
        moveRightEnd.time = 0;

        endMoveLeft.Stop();
        endMoveLeft.time = 0;

        endMoveRight.Stop();
        endMoveRight.time = 0;

        twoPointsMoveLeft.Stop();
        twoPointsMoveLeft.time = 0;

        twoPointsMoveRight.Stop();
        twoPointsMoveRight.time = 0;
    }

    void Open()
    {
        AudioManager.Instance.StartPlayingUiAudio(hover);
        isOpen = true;
        UiStatus.OpenUI();
        rewindBox.SetActive(true);
        Initialise();
        UpdatePanels();
    }

    void Close()
    {
        AudioManager.Instance.StartPlayingUiAudio(hover);
        isOpen = false;
        UiStatus.CloseUI();
        snapshots.Clear();
        rewindBox.SetActive(false);
    }

    void ForcedRewindEnabled(object o = null)
    {
        isForcedRewind = true;
        Open();
        StartCoroutine(ForcedRewind());
    }

    void ShowHint(object o = null)
    {
        snapshotHint.Show(SnapshotManager.Instance.Snapshots.Peek());
    }

    IEnumerator ForcedRewind()
    {
        // navigates to left or right randomly for 
        // the number of times equal to the size 
        // of the snapshots then select
        yield return new WaitForSeconds(0.5f);
        // allowing at most rewinding to two point away
        for (int i = 0; i < 1; i++)
        {
            int random = Random.Range(0, 2);
            Debug.Log("random: " + random);
            if (random < 1)
            {
                RightSelect();
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            else
            {
                LeftSelect();
                yield return new WaitForSeconds(0.5f);
                continue;
            }
        }
        Select();
        isForcedRewind = false;
    }

    // For testing only. Should be removed in the 
    // final version.
    // FIXME: No dummy snapshots
    void InitialiseTesting()
    {
        if (snapshots.Count != 0)
            return;

        string dummyScene = "CityFloor";
        string dummyDescription = "Hector travelled back in time to when the time machine is first invented";
        SnapshotManager.Instance.TestTakeSnapshot(dummyScene, dummyDescription, dummyScreenshot);
        SnapshotManager.Instance.TestTakeSnapshot(dummyScene, dummyDescription, dummyScreenshot);
        SnapshotManager.Instance.TestTakeSnapshot(dummyScene, dummyDescription, dummyScreenshot);

    }
}
