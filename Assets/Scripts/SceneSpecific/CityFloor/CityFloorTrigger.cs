using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

public class CityFloorTrigger : MonoBehaviour
{
    private GameEvent onCityFloorStart = new GameEvent("Hector first arrived at city floor");
    [SerializeField] Conversation convo;

    void OnTriggerEnter(Collider collider)
    {
        if (EventLedger.Instance.HasEventOccurredInLoopedPast(onCityFloorStart))
        {
            Destroy(gameObject);
            return;
        }
        // NOTE: Should technically check against past not looped past, however since the player cannot rewind past first entering 
        if (collider.CompareTag("Player"))
        {
            EventLedger.Instance.RecordEvent(onCityFloorStart);
            DialogueManager.Instance.StartConversation(convo);
        }
    }
}
