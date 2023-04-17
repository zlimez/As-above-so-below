using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Chronellium.SceneSystem;
using Chronellium.EventSystem;


public class Room2Door : Interactable
{
    private Func<bool> isHectorBookedCandy = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnCandyChosen);
    private Func<bool> isHectorBookedGinger = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnGingerChosen);
    private Func<bool> isHectorLockedGinger = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level2Events_GingerLockBroken);
    private Func<bool> isCandyLetHectorIn = () => EventLedger.Instance.HasEventOccurredInPast(DynamicEvent.Convo_CallCandyRealNameConvoCompleted);
    private Func<bool> isDoorUnlocked = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level4Events_CandyLockUnlocked);
    public string talkToCandyChoiceText, leaveChoiceText, askCandyDirectlyChoiceText, offerCandyMoneyChoiceText, callCandyRealNameChoiceText, pretendStaffChoiceText;
    public Choice talkToCandyChoice, leaveChoice, askCandyDirectlyChoice, offerCandyMoneyChoice, callCandyRealNameChoice, pretendStaffChoice;
    public Conversation isDoorLockedConvo, wrongRoomConvo, thinkConvo, askCandyDirectlyConvo, offerCandyMoneyConvo, callCandyRealNameConvo, pretendStaffConvo;
    [SerializeField] AudioClip doorAudio;
    void Awake()
    {
        InitializeChoices();
    }

    private void InitializeChoices()
    {
        talkToCandyChoice = new Choice(talkToCandyChoiceText, OnTalkToCandyChoiceSelected);
        leaveChoice = new Choice(leaveChoiceText, OnLeaveChoiceSelected);
        askCandyDirectlyChoice = new Choice(askCandyDirectlyChoiceText, OnAskCandyDirectlyChoiceSelected);
        offerCandyMoneyChoice = new Choice(offerCandyMoneyChoiceText, OnOfferCandyMoneyChoiceSelected);
        callCandyRealNameChoice = new Choice(callCandyRealNameChoiceText, OnCallCandyRealNameChoiceSelected);
        pretendStaffChoice = new Choice(pretendStaffChoiceText, OnPretendStaffChoiceSelected);
    }

    public override void Interact()
    {
        if (isCandyLetHectorIn()) // Avoid retriggering the dialogue if you go back in
        {
            AudioManager.Instance.StartPlayingSoundEffectAudio(doorAudio);
            SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelRoom2);
            return;
        }

        if (isHectorLockedGinger() || isHectorBookedGinger())
        {
            DialogueManager.Instance.StartConversation(wrongRoomConvo);
            ChoiceManager.Instance.StartChoice(talkToCandyChoice, leaveChoice);
            return;
        }

        if (isHectorBookedCandy())
        {
            if (!isDoorUnlocked())
            {
                DialogueManager.Instance.StartConversation(isDoorLockedConvo);
                return;
            }

            AudioManager.Instance.StartPlayingSoundEffectAudio(doorAudio);
            SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelRoom2);
            return;
        }
    }

    void OnEnable()
    {
        EventManager.StartListening(DynamicEvent.Convo_CallCandyRealNameConvoCompleted, OnCallCandyRealNameConvoCompleted);
    }
    void OnDisable()
    {
        EventManager.StopListening(DynamicEvent.Convo_CallCandyRealNameConvoCompleted, OnCallCandyRealNameConvoCompleted);
    }
    private void OnCallCandyRealNameConvoCompleted(object input = null)
    {
        AudioManager.Instance.StartPlayingSoundEffectAudio(doorAudio);
        SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelRoom2);
    }
    public void OnTalkToCandyChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(thinkConvo);
        ChoiceManager.Instance.StartChoice(askCandyDirectlyChoice, offerCandyMoneyChoice, callCandyRealNameChoice, pretendStaffChoice);
    }
    public void OnLeaveChoiceSelected(object o = null)
    {
    }
    public void OnAskCandyDirectlyChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(askCandyDirectlyConvo);
    }
    public void OnOfferCandyMoneyChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(offerCandyMoneyConvo);
    }
    public void OnCallCandyRealNameChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(callCandyRealNameConvo);
    }
    public void OnPretendStaffChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(pretendStaffConvo);
    }
}