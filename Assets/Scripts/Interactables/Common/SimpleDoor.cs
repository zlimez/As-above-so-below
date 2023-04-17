using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;
using Chronellium.SceneSystem;

public class SimpleDoor : Interactable
{
    [SerializeField] AudioClip doorAudio;
    public Conversation cannotEnterConversation;
    public ChronelliumScene scene;
    public ActivationClauses conditionalEntry;
    public GameEvent leavingEvent;

    // Update is called once per frame

    void EnterRoom(object input = null)
    {
        AudioManager.Instance.StartPlayingSoundEffectAudio(doorAudio);
        SceneLoader.Instance.PrepLoadWithMaster(scene);
        if (leavingEvent != null) {
            EventLedger.Instance.RecordEvent(leavingEvent);
        }
    }


    public override void Interact()
    {
        // Amend later if we want to allow Hector to sneak into Aka's place even while the police 
        // is outside at the car, and essentially turn on the security so he can't get back in?
        if (!conditionalEntry || conditionalEntry.IsSatisfied())
        {
            EnterRoom();
        }
        else 
        {
            DialogueManager.Instance.StartConversation(cannotEnterConversation);
        }
    }
}
