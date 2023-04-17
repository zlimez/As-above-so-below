using System.Collections;
using Chronellium.EventSystem;
using UnityEngine;

public class Ramen : Interactable
{
    private GameEvent ramenHouseInquiry = new GameEvent("Hector inquires customers at ramen house");
    [SerializeField] Conversation ramenHouseInquiry1, ramenHouseInquiry2;

    void Update()
    {
        TryInteract();
    }

    public override void Interact()
    {
        if (!EventLedger.Instance.HasEventOccurredInPast(ramenHouseInquiry))
        {
            DialogueManager.Instance.StartConversation(ramenHouseInquiry1);
            EventLedger.Instance.RecordEvent(ramenHouseInquiry);
        }
        else
        {
            DialogueManager.Instance.StartConversation(ramenHouseInquiry2);
        }
    }
}
