using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PimpDoorInside : Interactable
{
    public Conversation exitRoomConvo;

    public override void Interact()
    {
        DialogueManager.Instance.StartConversation(exitRoomConvo);
        return;
    }
}