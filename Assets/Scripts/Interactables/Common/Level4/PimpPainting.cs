using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Chronellium.SceneSystem;
using Chronellium.EventSystem;
public class PimpPainting : Interactable
{
    private Func<bool> hasHectorLockedGinger = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level2Events_GingerLockBroken);
    private Func<bool> hasHectorSpikedDrink = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level4Events_TeaDrugged);
    private Func<bool> hasHectorFoundItemBefore = () => EventLedger.Instance.HasEventOccurredInLoopedPast(DynamicEvent.Convo_FoundItemConvoCompleted);
    private Func<bool> hasHectorExploredGingerRoom = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level2Events_ExploredGingersRoom);
    private Func<bool> hasHectorTalkedToPimp = () => EventLedger.Instance.HasEventOccurredInPast(DynamicEvent.Convo_MeetingPimpConvo);

    public Conversation foundItemConvo, commentConvo, pimpReturnedConvo, pimpItemTakenConvo, foundItemBeforeConvo;
    public Item CherryItem;

    public override void Interact()
    {
        if (Inventory.Instance.Contains(CherryItem))
        {
            DialogueManager.Instance.StartConversation(pimpItemTakenConvo);
            return;
        }

        if (hasHectorSpikedDrink())
        {
            DialogueManager.Instance.StartConversation(commentConvo);
            return;
        }

        if (hasHectorTalkedToPimp())
        {
            DialogueManager.Instance.StartConversation(
                hasHectorFoundItemBefore() ? foundItemBeforeConvo : foundItemConvo,
                OnFoundItemConvoCompleted
            );
            return;
        }

        DialogueManager.Instance.StartConversation(commentConvo);
    }

    void OnEnable()
    {
        EventManager.StartListening(DynamicEvent.Convo_PimpReturnedConvoCompleted, OnPimpReturnedConvoCompleted);
    }

    void OnDisable()
    {
        EventManager.StopListening(DynamicEvent.Convo_PimpReturnedConvoCompleted, OnPimpReturnedConvoCompleted);
    }

    private void OnFoundItemConvoCompleted()
    {
        Inventory.Instance.AddTo(true, CherryItem, 1);
    }

    private void OnPimpReturnedConvoCompleted(object input = null)
    {
        SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.PimpTorture);
    }
}