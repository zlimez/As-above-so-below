using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

// TODO: Snapshot choice
public class PimpTortureCutsceneChoiceHelper : MonoBehaviour
{
    public PlayableDirector timeline;
    // public string choiceText1, choiceText2;
    private Choice[] rewindChoices;
    private bool finished = false;

    void Awake()
    {
        InitialiseChoice();
    }

    private void InitialiseChoice()
    {
        int offset = 0;
        rewindChoices = new Choice[SnapshotManager.Instance.Snapshots.Count];
        foreach (Snapshot snapshot in SnapshotManager.Instance.Snapshots)
        {
            rewindChoices[offset] = RewindChoiceFactory(snapshot, offset);
        }
    }

    public void BeginChoices()
    {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
        ChoiceManager.Instance.StartChoice(rewindChoices);
        StartCoroutine(WaitUntilChoiceStart(() => { finished = true; }));
    }

    private Choice RewindChoiceFactory(Snapshot snapshot, int offset)
    {
        UnityAction<object> rewinder = (object input) =>
        {
            SnapshotManager.Instance.LoadSnapshot(offset);
        };

        return new Choice(snapshot.Description, rewinder, true);
    }

    private void Update()
    {
        if (finished && !ChoiceManager.Instance.InChoice && !DialogueManager.Instance.InDialogue)
        {
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
            finished = false;
        }
    }

    IEnumerator WaitUntilChoiceStart(System.Action whenDone)
    {
        while (!ChoiceManager.Instance.InChoice)
        {
            yield return null;
        }
        whenDone();
    }
}
