using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

public class RewindTutorialPrompt : MonoBehaviour
{
    [SerializeField] Conversation rewindTutorial;

    void OnTriggerEnter(Collider collider)
    {
        if (EventLedger.Instance.HasEventOccurredInLoopedPast(StaticEvent.CommonEvents_RewindTutorialTriggered))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }
        if (collider.CompareTag("Player"))
        {
            DialogueManager.Instance.StartConversation(rewindTutorial);
            StartCoroutine(WaitForDialog());
            return;
        }
    }

    IEnumerator WaitForDialog()
    {
        while (DialogueManager.Instance.InDialogue)
        {
            yield return new WaitForSeconds(0.1f);
        }
        EventLedger.Instance.RecordEvent(StaticEvent.CommonEvents_RewindTutorialTriggered, false);
        Destroy(this);
    }
}
