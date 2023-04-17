using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Chronellium.EventSystem;

// TODO: Snapshot choice
public class MurderCutsceneChoiceHelper : MonoBehaviour
{
    public PlayableDirector timeline;
    public string choiceText1, choiceText2;
    private Choice choice1, choice2;
    private bool choice1Activated = true;
    private bool choice2Activated = false;
    private bool finished = false;

    void Awake()
    {
        InitialiseChoice();
    }

    private void InitialiseChoice()
    {
        choice1 = new Choice(choiceText1, Choice1, choice1Activated);
        choice2 = new Choice(choiceText2, Choice2, choice2Activated);
    }

    public void BeginChoices()
    {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
        ChoiceManager.Instance.StartChoice(choice1, choice2);
        StartCoroutine(WaitUntilChoiceStart());
        finished = true;
    }

    // Invoked Events
    // Change according to what is required
    private void Choice1(object o = null)
    {
        EventLedger.Instance.RecordEvent(StaticEvent.BrothelRewindEvents_FirstIterationCompleted);
        // Entrance snapshot is the latest one at this point of time, hence offset 0
        SnapshotManager.Instance.LoadSnapshot(0);
    }

    private void Choice2(object o = null) { }

    private void Update()
    {
        if (finished && !ChoiceManager.Instance.InChoice && !DialogueManager.Instance.InDialogue)
        {
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
            finished = false;
        }
    }

    IEnumerator WaitUntilChoiceStart()
    {
        while (!ChoiceManager.Instance.InChoice)
        {
            yield return null;
        }
    }
}
