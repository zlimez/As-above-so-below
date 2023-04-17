using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.SceneSystem;
using Chronellium.EventSystem;
using UnityEngine.Playables;

public class PimpTortureSpike : MonoBehaviour
{
    public Conversation convo;
    public SceneTransition sceneTransition;
    public PlayableDirector timeline;
    private bool finished;

    // This event is triggered in PimpTortureConvo2
    // private GameEvent StaticEvent.Level4Events_CheckDrinkSpike = new GameEvent("Check drink spike");

    void Awake()
    {
        InitializeEvents();
    }

    private void Update()
    {
        if (finished && !DialogueManager.Instance.InDialogue && !ChoiceManager.Instance.InChoice)
        {
            timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
            finished = false;
        }
    }

    private void InitializeEvents()
    {
        EventManager.StartListening(StaticEvent.Level4Events_CheckDrinkSpike, CheckSpike);
    }

    private void CheckSpike(object input = null)
    {
        if (!EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level4Events_TeaDrugged))
        {
            sceneTransition.SceneTransit(ChronelliumScene.PimpTorture2);
        }
        else
        {
            StartCoroutine(WaitUntilConvoEnd(() => {
                timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
                DialogueManager.Instance.StartConversation(convo);
                finished = true;
            }));
        }
    }

    IEnumerator WaitUntilConvoEnd(System.Action whenDone)
    {
        while (DialogueManager.Instance.InDialogue)
        {
            yield return null;
        }
        whenDone();
    }
}
