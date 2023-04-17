using System.Collections;
using System;
using UnityEngine;

public class OutputNodeView : GeneralOutputView
{
    // The parameter is the number of layers the virus have upon reaching
    public event Action<int> onVirusReachedEnd;
    protected override void Awake() {
        base.Awake();
        downstream = null;
    }

    protected override IEnumerator MoveStream(GameObject content) {
        // Any animation logic here
        yield return null;
        EndStream(content);
    }

    void EndStream(GameObject content) {
        outputNode.DetermineOutput();
        Destroy(content);
        onVirusReachedEnd?.Invoke(content.GetComponent<VirusView>().LayerCount());
    }
}
