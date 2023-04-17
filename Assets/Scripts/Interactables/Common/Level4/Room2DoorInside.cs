using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Chronellium.SceneSystem;
using Chronellium.EventSystem;

public class Room2DoorInside : Interactable
{
    private Func<bool> isHectorBookedCandyOrHectorBookedGinger = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnCandyChosen) || EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnGingerChosen);
    private Func<bool> isHectorLockedGingerOrHectorSpikedPimp = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level2Events_GingerLockBroken) || EventLedger.Instance.HasEventOccurredInPast(StaticEvent.Level4Events_TeaDrugged);
    private Func<bool> isHectorBookedCandy = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnCandyChosen);
    private Func<bool> isHectorBookedGinger = () => EventLedger.Instance.HasEventOccurredInPast(StaticEvent.LobbyEvents_OnGingerChosen);
    [SerializeField] AudioClip doorAudio;

    public override void Interact()
    {
        if (isHectorLockedGingerOrHectorSpikedPimp())
        {
            AudioManager.Instance.StartPlayingSoundEffectAudio(doorAudio);
            SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelPimpLevel4);
            return;
        }
        if (isHectorBookedCandyOrHectorBookedGinger())
        {
            // Can refactor this to just 
            //  - BookedGinger => Murder2
            //  - BookedCandy  => Murder
            // However, keeping the current to make it clearer chronologically

            if (EventLedger.Instance.HasEventOccurredInLoopedPast(StaticEvent.BrothelRewindEvents_FirstIterationCompleted))
            {
                if (isHectorBookedCandy())
                {
                    SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.Murder);
                    return;
                }
                if (isHectorBookedGinger())
                {
                    SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.Murder2);
                    return;
                }
            }
            EventLedger.Instance.RecordEvent(StaticEvent.BrothelRewindEvents_FirstIterationCompleted);
            SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.Murder);
            return;
        }
    }
}