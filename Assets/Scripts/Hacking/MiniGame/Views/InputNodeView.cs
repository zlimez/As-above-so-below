using System;
using System.Collections;
using UnityEngine;

// Allows players to choose input
public class InputNodeView : PipeView
{
    private InputNode inputNode = new InputNode();
    [SerializeField] private SpriteRenderer arrow;
    [SerializeField] private float arrowRotationInterval = 1f;
    [SerializeField] private GameObject layeredVirusPrefab;
    [SerializeField] private Transform spawnPoint;
    public VirusBase SelectedInput { get; private set; }
    public event Action<InputNodeView> onSelected;
    // Deselect is invoked by common virus base choice menu
    public Action onDeselected;
    private IEnumerator spinRoutine;

    void Awake() {
        SelectedInput = null;
        upstream = null;
    }

    void OnEnable() {
        onDeselected += StopArrowSpin;
    }

    void OnDisable() {
        onDeselected -= StopArrowSpin;
    }

    void OnMouseDown() {
        onSelected?.Invoke(this);
        spinRoutine = SpinArrow();
        StartCoroutine(spinRoutine);
    }

    // Wrapper for EndArrowSpin routine
    private void StopArrowSpin() {
        Debug.Log("Stopping spin on " + name);
        StopCoroutine(spinRoutine);
        spinRoutine = null;
        StartCoroutine(EndArrowSpin());
    }

    IEnumerator EndArrowSpin() {
        float intervalRemainder = (360 - arrow.transform.localRotation.eulerAngles.y) / 360 * arrowRotationInterval;
        float timeElapsed = 0;
        Vector3 fullRotation = new Vector3(arrow.transform.localRotation.eulerAngles.x, 360, arrow.transform.localRotation.eulerAngles.z);
        Vector3 initialRotation = arrow.transform.localRotation.eulerAngles;
        while (timeElapsed < intervalRemainder) {
            timeElapsed += Time.deltaTime;
            arrow.transform.localRotation = Quaternion.Euler(VectorUtils.CubicLerpVector(initialRotation, fullRotation, timeElapsed / intervalRemainder));
            yield return null;
        }
    }

    IEnumerator SpinArrow() {
        float timeElapsed = 0;
        Vector3 initialRotation = arrow.transform.localRotation.eulerAngles;
        Vector3 fullRotation = new Vector3(arrow.transform.localRotation.eulerAngles.x, 360, arrow.transform.localRotation.eulerAngles.z);
        while (true) {
            timeElapsed += Time.deltaTime;
            // Assumes frame rate is fast enough such that one single frame cannot cause timeElapsed to increment by equal or more than one arrow rotation interval
            timeElapsed = timeElapsed > arrowRotationInterval ? timeElapsed - arrowRotationInterval : timeElapsed;
            arrow.transform.localRotation = Quaternion.Euler(VectorUtils.CubicLerpVector(initialRotation, fullRotation, timeElapsed / arrowRotationInterval));
            yield return null;
        }
    }

    bool HasInput() { return SelectedInput != null; }

    public void ChangeSelectedInput(VirusBase selectedVirus) {
        SelectedInput = selectedVirus;
        arrow.color = selectedVirus.color;
    }

    void SetInput() {
        inputNode.CreateInput(SelectedInput);
    }

    public void StartStreamWithInput() {
        inputNode.Clear();
        // if (!HasInput()) selectedInput = menu.allVirusBases[0];
        // Debug.Log($"Starting stream at {name}");
        if (!HasInput()) Debug.LogError($"{name} started without selected input, examine hack game manager or start button");
        SetInput();
        inputNode.DetermineOutput();
        GameObject simpleVirus = Instantiate(layeredVirusPrefab, spawnPoint.position, Quaternion.identity);
        simpleVirus.GetComponent<VirusView>()?.InitVirus(inputNode.GetOutput());
        CallMoveStream(simpleVirus, this);
    }

    // Do nothing
    protected override void AbsorbFromUpstream(PipeView providedUpstream = null) {}

    public override Pipe GetPipe() { return inputNode; }

    public override float GetStreamSpeed() { return 0; }
    protected override IEnumerator MoveStream(GameObject content) {
        // Addition animation logic can be added here
        yield return null;
        downstream.CallMoveStream(content, this);
    }
}
