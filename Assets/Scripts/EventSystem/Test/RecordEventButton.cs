using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Chronellium.EventSystem
{
    public class RecordEventButton : MonoBehaviour
    {
        // public TMP_InputField inputField;
        [Tooltip("Press M to submit")] public string eventName;
        public bool isAdd = true;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                Record();
            }
        }

        public void Record()
        {
            if (eventName.Trim() == "") return;
            GameEvent gameEvent = new GameEvent(eventName.Trim());

            if (isAdd)
            {
                EventLedger.Instance.RecordEvent(gameEvent);
            }
            else
            {
                EventLedger.Instance.RemoveEvent(gameEvent);
            }
            eventName = "";
        }
    }
}