using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Chronellium.EventSystem;

public class Room2DoorLock : Interactable
{
    private Func<bool> isDoorUnlocked = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level4Events_CandyLockUnlocked);
    private Func<bool> isCandyChosen = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnCandyChosen);
    public Conversation isDoorLockedConvo, isDoorUnlockedConvo;

    private void Start()
    {
        if (!isCandyChosen())
        {
            DisableHint();
        }
    }

    public override void Interact()
    {
        if (isCandyChosen())
        {
            if (isDoorUnlocked())
            {
                DialogueManager.Instance.StartConversation(isDoorUnlockedConvo);
                return;
            }

            DialogueManager.Instance.StartConversation(isDoorLockedConvo);
            EventLedger.Instance.RecordEvent(StaticEvent.Level4Events_CandyLockUnlocked);
        }
        else
        {
            // Does nothing if Ginger is chosen. Interact with door instead.
            return;
        }
    }

}
