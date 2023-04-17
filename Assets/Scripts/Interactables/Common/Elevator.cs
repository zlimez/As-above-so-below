using System;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;
using Chronellium.SceneSystem;

// TODO: Config elevator criteria for each level
public class Elevator : Interactable
{
    private string choiceText1 = "Level 4";
    private string choiceText2 = "Level 3";
    private string choiceText3 = "Level 2";
    private string choiceText4 = "Level 1";
    private Choice choice1, choice2, choice3, choice4, choice5;
    [SerializeField] private Conversation goLevel4, preventLeavingConvo;
    [SerializeField] AudioClip elevatorAudio;
    private Func<bool> hasHectorExploredGingerRoom = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level2Events_ExploredGingersRoom);

    public int currentLevel;

    void Awake()
    {
        InitializeChoice();
    }

    private void InitializeChoice()
    {
        choice1 = new Choice(choiceText1, Choice1);
        choice2 = new Choice(choiceText2, Choice2);
        choice3 = new Choice(choiceText3, Choice3);
        choice4 = new Choice(choiceText4, Choice4);
        choice5 = new Choice("Go back", Choice5);
    }

    void Update()
    {
        TryInteract();
    }

    public override void Interact()
    {
        if (currentLevel == 4 && hasHectorExploredGingerRoom())
        {
            DialogueManager.Instance.StartConversation(preventLeavingConvo);
            return;
        }

        switch (currentLevel)
        {
            case 1:
                ChoiceManager.Instance.StartChoice(choice1, choice2, choice3, choice5);
                break;
            case 2:
                ChoiceManager.Instance.StartChoice(choice1, choice2, choice4, choice5);
                break;
            case 3:
                ChoiceManager.Instance.StartChoice(choice1, choice3, choice4, choice5);
                break;
            default:
                ChoiceManager.Instance.StartChoice(choice2, choice3, choice4, choice5);
                break;
        }
        return;
    }

    public void Choice1(object o = null)
    {
        AudioManager.Instance.StartPlayingSoundEffectAudio(elevatorAudio);
        EventLedger.Instance.RecordEvent(StaticEvent.CommonEvents_ElevatorTaken);
        SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelPimpLevel4);
    }

    public void Choice2(object o = null)
    {
        if (EventLedger.Instance.HasEventOccurredInLoopedPast(StaticEvent.BrothelRewindEvents_FirstIterationCompleted))
        {
            AudioManager.Instance.StartPlayingSoundEffectAudio(elevatorAudio);
            EventLedger.Instance.RecordEvent(StaticEvent.CommonEvents_ElevatorTaken);
            SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelLevel3);
        }
        else
        {
            DialogueManager.Instance.StartConversation(goLevel4);
        }
    }

    public void Choice3(object o = null)
    {
        if (EventLedger.Instance.HasEventOccurredInLoopedPast(StaticEvent.BrothelRewindEvents_FirstIterationCompleted))
        {
            AudioManager.Instance.StartPlayingSoundEffectAudio(elevatorAudio);
            EventLedger.Instance.RecordEvent(StaticEvent.CommonEvents_ElevatorTaken);
            SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelLevel2);
        }
        else
        {
            DialogueManager.Instance.StartConversation(goLevel4);
        }
    }

    public void Choice4(object o = null)
    {
        //TODO Change scene to correct place
        AudioManager.Instance.StartPlayingSoundEffectAudio(elevatorAudio);
        EventLedger.Instance.RecordEvent(StaticEvent.CommonEvents_ElevatorTaken);
        SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelLobby);
    }

    public void Choice5(object o = null)
    {
        return;
    }
}
