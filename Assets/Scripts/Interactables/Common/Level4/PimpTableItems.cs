using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;
using System;
using Chronellium.SceneSystem;

public class PimpTableItems : Interactable
{
    private Func<bool> isHectorLockedGinger = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level2Events_GingerLockBroken);
    private Func<bool> hasHectorTalkedToPimp = () => EventLedger.Instance.HasEventOccurredInPast(DynamicEvent.Convo_MeetingPimpConvo);
    public string inspectComputerChoiceText, inspectGlassChoiceText;
    public Choice inspectComputerChoice, inspectGlassChoice, yellowsChoice, otherItemsChoice;
    public Conversation inspectComputerConvo, inspectGlassConvo, yellowsConvo, otherItemsConvo;
    // For convenience, instead of checking event ledger
    [SerializeField] private bool isTeaDrugged = false;

    [SerializeField] private Item teaDrug;

    void Awake()
    {
        InitializeChoices();
    }

    private void InitializeChoices()
    {
        inspectComputerChoice = new Choice(inspectComputerChoiceText, OnInspectComputerChoiceSelected);
        inspectGlassChoice = new Choice(inspectGlassChoiceText, OnInspectGlassChoiceSelected);
    }

    public override void Interact()
    {
        if (!hasHectorTalkedToPimp())
        {
            SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.MeetingPimp);
            return;
        }

        if (isHectorLockedGinger() && !isTeaDrugged)
        {
            ChoiceManager.Instance.StartChoice(inspectComputerChoice, inspectGlassChoice);
            return;
        }
    }

    public void OnInspectComputerChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(inspectComputerConvo);
    }

    public void OnInspectGlassChoiceSelected(object o = null)
    {
        DialogueManager.Instance.StartConversation(inspectGlassConvo);
        EventManager.StartListening(DynamicEvent.Convo_GlassInspectRemark, DrawInventory);
    }

    public void DrawInventory(object o = null)
    {
        EventManager.InvokeEvent(CommonEventCollection.OpenInventory);
        InventoryUI.Instance.StartItemSelect(OnSelectTeaSubstance);
    }

    private bool OnSelectTeaSubstance(Item item)
    {
        if (item == null)
        {
            DialogueManager.Instance.StartConversation(otherItemsConvo);
            return false;
        }

        //InventoryUI.Instance.HidePanel();
        if (item.Equals(teaDrug))
        {
            InventoryUI.Instance.StopItemSelect();
            DialogueManager.Instance.StartConversation(yellowsConvo);
            EventLedger.Instance.RecordEvent(StaticEvent.Level4Events_TeaDrugged);
            isTeaDrugged = true;
            return true;
        }
        else
        {
            DialogueManager.Instance.StartConversation(otherItemsConvo);
            return false;
        }
    }
}