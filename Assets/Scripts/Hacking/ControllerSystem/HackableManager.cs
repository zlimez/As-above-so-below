using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;

namespace Chronellium.Hacking {
    public class HackedBehaviour : MonoBehaviour {
        // When view is changed to hacked UI, the controlled hackable cannot be moved
        public static bool IsFrozen { get; set; }
    }

    public class HackableManager : MonoBehaviour
    {
        [SerializeField] private Hackable[] allHackables;
        // Starts wuth scientist
        [SerializeField] private Hackable controlledHackable;
        public Hackable BasedHackable { get; private set; }
        private List<Hackable> dynamicHackables = new List<Hackable>();
        private List<Hackable> staticHackables = new List<Hackable>();
        [SerializeField] private bool isInActivationProcess = true;
        public event Action OnActivated;
        public event Action OnDeactivated;

        void Awake() {
            foreach (Hackable hackable in allHackables) {
                if (hackable.isStatic) {
                    staticHackables.Add(hackable);
                } else {
                    dynamicHackables.Add(hackable);
                }
            }

            InitStaticHackables();
        }

        void OnEnable() {
            foreach (Hackable hackable in allHackables) {
                hackable.OnBased += ChangeBasedHackable;
                hackable.OnControlled += ChangeControlledHackable;
            }
            EventManager.StartListening(Hackable.generalHackFailed, DeactivateHackscape);
        }

        void OnDisable() {
            foreach (Hackable hackable in allHackables) {
                hackable.OnBased -= ChangeBasedHackable;
                hackable.OnControlled -= ChangeControlledHackable;
            }
            EventManager.StopListening(Hackable.generalHackFailed, DeactivateHackscape);
            HackedBehaviour.IsFrozen = false;
        }

        public void ChangeControlledHackable(Hackable newControlledHackable) {
            Debug.Log("Changing control to " + newControlledHackable.name);
            if (!ReferenceEquals(controlledHackable, newControlledHackable)) {
                controlledHackable.ControlRelinquished();
                controlledHackable = newControlledHackable;
            }
            DeactivateHackscape();
        }

        public void ActivateHackscape() {
            // Whenever hack view starts, base is aligned with controlled
            isInActivationProcess = true;
            BasedHackable = controlledHackable;
            BasedHackable.SetAsStartBase();
            HackedBehaviour.IsFrozen = true;
            OnActivated?.Invoke();
        }

        public void DeactivateHackscape(object input = null) {
            // FIXME: Better way To recycle resource
            BasedHackable.Unbase();
            HackedBehaviour.IsFrozen = false;
            OnDeactivated?.Invoke();
        }

        public void ChangeBasedHackable(Hackable newBasedHackable) {
            if (!isInActivationProcess) {
                // SetAsStartBase will trigger a cycle of reassignment due to OnBased event hence isInActivationProcess acts as a guard
                Debug.Log("Unbasing last " + BasedHackable.name);
                BasedHackable.Unbase();
            } else {
                isInActivationProcess = false;
            }

            BasedHackable = newBasedHackable;
            newBasedHackable.dynamicAdjHackables.Clear();
            foreach (Hackable dynamicHackable in dynamicHackables) {
                if (!ReferenceEquals(newBasedHackable, dynamicHackable) && newBasedHackable.OverlapsWith(dynamicHackable)) {
                    newBasedHackable.dynamicAdjHackables.Add(dynamicHackable);
                }
            }
            newBasedHackable.staticAdjHackables.Clear();
            if (!newBasedHackable.isStatic) {
                foreach (Hackable staticHackable in staticHackables) {
                    if ( !ReferenceEquals(newBasedHackable, staticHackable) && newBasedHackable.OverlapsWith(staticHackable)) {
                        newBasedHackable.staticAdjHackables.Add(staticHackable);
                    }
                }
                newBasedHackable.OrderStaticAdjHackables();
            }
        }

        void InitStaticHackables() {
            for (int i = 0; i < staticHackables.Count; i++) {
                for (int j = i; j < staticHackables.Count; j++) {
                    if (staticHackables[i].OverlapsWith(staticHackables[j])) {
                        staticHackables[i].staticAdjHackables.Add(staticHackables[j]);
                        staticHackables[j].staticAdjHackables.Add(staticHackables[i]);
                    }
                }
            }

            staticHackables.ForEach(staticHackable => staticHackable.OrderStaticAdjHackables());
        }
    }
}
