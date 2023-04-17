using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Chronellium.EventSystem;

namespace Chronellium.Hacking {
    // Represent a possible state after a hackable is hacked
    public class HackedMode : MonoBehaviour
    {
        public static string hackFailedPrefix = "Hack attempt failed on";
        public static string hackSucceededPrefix = "Hack attempt succeeded on";
        [SerializeField] private string modeName;
        // Between 0 and 1
        [SerializeField] private float successRate = 1;
        [SerializeField] private HackedBehaviour hackedBehaviour;
        [SerializeField] private HackedBehaviour alertedBehaviour;
        [SerializeField] private bool unlocked = false;
        public event Action<HackedMode> OnHackSucceed;

        [NonSerialized] public GameEvent hackFailedEvent;
        [NonSerialized] public GameEvent hackSucceededEvent;

        public string Modename => modeName;

        void Awake() {
            hackFailedEvent = new GameEvent($"{HackedMode.hackFailedPrefix} {name} with mode {modeName}");
            hackSucceededEvent = new GameEvent($"{HackedMode.hackSucceededPrefix} {name} with mode {modeName}");
        }

        public bool BeginHackAttempt(bool hasAffinity) {
            if (!hasAffinity) {
                if (alertedBehaviour != null) alertedBehaviour.enabled = true;
                return false;
            }

            System.Random rand = new System.Random();
            if (rand.NextDouble() <= successRate) {
                OnHackSucceed?.Invoke(this);
                unlocked = true;
                return true;
            }
            if (alertedBehaviour != null) alertedBehaviour.enabled = true;
            return false;
        }

        public void Activate() {
            if (unlocked && hackedBehaviour != null) hackedBehaviour.enabled = true;
        }

        public void Deactivate() {
            Debug.Log("Deactivating hacked mode " + modeName + " from " + name);
            if (hackedBehaviour != null) hackedBehaviour.enabled = false;
        }
    }
}