using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

public class DoorBreakEvent : MonoBehaviour
{
    public void UpdateLedger()
    {
        EventLedger.Instance.RecordEvent(StaticEvent.Level2Events_OnDoorBashed);
    }
}
