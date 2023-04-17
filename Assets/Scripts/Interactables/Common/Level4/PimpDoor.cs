using System.Collections;
using System.Collections.Generic;
using Chronellium.EventSystem;
using Chronellium.SceneSystem;
using System;

public class PimpDoor : Interactable
{
    private Func<bool> isHectorBookedCandyOrHectorBookedGinger = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnCandyChosen) || EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnGingerChosen);
    private Func<bool> candyReassuredConvoCompletedAndNotHectorKnockedOnPimpDoor = () => EventLedger.Instance.HasEventOccurredInLoopedPast(DynamicEvent.Convo_CandyReassured) && !EventLedger.Instance.HasEventOccurredInLoopedPast(DynamicEvent.Choice_HectorKnockedOnPimpDoor);
    private Func<bool> candyReassuredConvoCompletedAndHectorKnockedOnPimpDoor = () => EventLedger.Instance.HasEventOccurredInLoopedPast(DynamicEvent.Convo_CandyReassured) && EventLedger.Instance.HasEventOccurredInLoopedPast(DynamicEvent.Choice_HectorKnockedOnPimpDoor);
    private Func<bool> hasHectorExploredGingerRoom = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level2Events_ExploredGingersRoom);

    public string knockChoiceText, noKnockChoiceText;
    public Choice knockChoice, noKnockChoice;
    public Conversation wrongRoomConvo, preChoiceConvo, knockConvo, noKnockConvo, afterKnockConvo;
    void Awake()
    {
        InitializeChoices();
    }

    private void InitializeChoices()
    {
        knockChoice = new Choice(knockChoiceText, OnKnockChoiceSelected);
        noKnockChoice = new Choice(noKnockChoiceText, OnNoKnockChoiceSelected);
    }

    public override void Interact()
    {
        if (hasHectorExploredGingerRoom())
        {
            SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelPimpRoom);
            return;
        }

        if (candyReassuredConvoCompletedAndNotHectorKnockedOnPimpDoor())
        {
            DialogueManager.Instance.StartConversation(preChoiceConvo);
            ChoiceManager.Instance.StartChoice(knockChoice, noKnockChoice);
            return;
        }

        if (candyReassuredConvoCompletedAndHectorKnockedOnPimpDoor())
        {
            DialogueManager.Instance.StartConversation(afterKnockConvo);
            return;
        }

        if (isHectorBookedCandyOrHectorBookedGinger())
        {
            DialogueManager.Instance.StartConversation(wrongRoomConvo);
            return;
        }
    }

    public void OnKnockChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(knockConvo);
    }
    public void OnNoKnockChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(noKnockConvo);
    }
}