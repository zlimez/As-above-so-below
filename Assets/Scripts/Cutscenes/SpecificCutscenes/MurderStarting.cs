using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

public class MurderStarting : MonoBehaviour
{
    private Func<bool> isMurderSecondTime = () => EventLedger.Instance.HasEventOccurredInLoopedPast(DynamicEvent.Convo_MurderFirstTime);
    private Func<bool> isMurderThirdTime = () => EventLedger.Instance.HasEventOccurredInLoopedPast(DynamicEvent.Convo_MurderSecondTime);

    public CutsceneDialogue cutsceneDialogue;

    public void ChooseStarting()
    {
        // CutsceneDialogue will have the first three convos in this order:
        // 1. First Murder
        // 2. Second time going through Murder
        // 3. Third or more time going through Murder
        // Will choose what is the starting convo for murder cutscene based on
        // how many times the player has tried this.

        if (isMurderThirdTime())
        {
            cutsceneDialogue.SkipCutscene();
            cutsceneDialogue.SkipCutscene();
            cutsceneDialogue.StartCutscene();
            return;
        }

        if (isMurderSecondTime())
        {
            cutsceneDialogue.SkipCutscene();
            cutsceneDialogue.StartCutscene();
            cutsceneDialogue.SkipCutscene();
            return;
        }

        // First Time
        cutsceneDialogue.StartCutscene();
        cutsceneDialogue.SkipCutscene();
        cutsceneDialogue.SkipCutscene();
        return;
    }
}
