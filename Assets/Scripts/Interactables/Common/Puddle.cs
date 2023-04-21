using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

namespace DeepBreath.Environment {
    public enum Realm { otherWorld, realWorld }
    public static class StateManager {
        public static Realm realm  = Realm.realWorld;
        public static void SwitchRealm() {
            if (realm == Realm.realWorld) {
                realm = Realm.otherWorld;
                EventManager.InvokeEvent(StaticEvent.Core_SwitchToOtherWorld);
            } else {
                realm = Realm.realWorld;
                EventManager.InvokeEvent(StaticEvent.Core_SwitchToRealWorld);
            }
        }
    }

    public class Puddle : Interactable
    {
        public override void Interact()
        {
            StateManager.SwitchRealm();
        }
    }
}
