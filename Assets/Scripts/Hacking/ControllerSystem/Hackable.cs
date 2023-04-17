using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Chronellium.EventSystem;

// TODO: State machine (Complication is that state transition will require different arguments that is passed from environment)
// Negative actions e.g. Unbase, ControlRelinquished are triggered automatically when another is based and controlled
namespace Chronellium.Hacking {
    public class OrderedHackable<T>: IComparable<OrderedHackable<T>> where T : IComparable<T> {
        public Hackable WrappedHackable { get; private set; }
        private T orderValue;

        public OrderedHackable(Hackable wrappedHackable, Hackable basedHackable, Func<Hackable, Hackable, T> orderValueYielder = null) {
            WrappedHackable = wrappedHackable;
            orderValue = orderValueYielder(wrappedHackable, basedHackable);
        }

        public OrderedHackable(Hackable wrappedHackable, T orderValue) {
            this.WrappedHackable = wrappedHackable;
            this.orderValue = orderValue;
        }

        // Counter-clockwise from +x axis
        public static Func<Hackable, Hackable, float> AngleGetter = (Hackable adjHackable, Hackable basedHackable) => {
            Vector2 basedToAdj = (Vector2)(adjHackable.transform.position - basedHackable.transform.position);
            return Vector2.SignedAngle(Vector2.right, basedToAdj);
        };

        public int CompareTo(OrderedHackable<T> otherOrderedHackable) {
            if (otherOrderedHackable == null) return 1;
            return orderValue.CompareTo(otherOrderedHackable.orderValue);
        }
    }
    public class Hackable : MonoBehaviour
    {
        public static GameEvent generalHackFailed = new GameEvent("Hack attempt failed");
        public enum State { alien = 0, friendly = 1, based = 2, controlled = 3 }

        public struct Network {
            public static int LABEL_SIZE = 3;
            // Player's starting device network
            public static Network SUPER_NETWORK = new Network(new char[] {'X', 'X', 'X'});
            private char[] type;

            public Network(char[] type) {
                this.type = type;
            }

            public bool ContainSymbol(char checkedSymbol) {
                foreach (char symbol in type) {
                    if (symbol == checkedSymbol) return true;
                }

                return false;
            }

            public bool HasAffinity(Network otherNetwork) {
                if (IsSuper || otherNetwork.IsSuper) return true;

                int sharedSymbolCounter = 0;
                foreach (char symbol in type) {
                    if (otherNetwork.ContainSymbol(symbol)) sharedSymbolCounter++;
                }

                return sharedSymbolCounter >= LABEL_SIZE - 1;
            }

            public bool IsSuper => Enumerable.SequenceEqual(type, SUPER_NETWORK.type);
        }

        [SerializeField] private char[] networkType = new char[Network.LABEL_SIZE];
        private Network network;
        [SerializeField] private Transform networkCenter;
        public Transform NetworkCenter { get { return networkCenter; } }
        [SerializeField] private float signalRadius;
        public float SignalDiameter { get { return signalRadius * 2; } }
        public bool isStatic;
        [SerializeField] private State state;
        public State HackableState { get { return state; } }

        [SerializeField] private HackedMode[] possibleHackedModes;
        public HackedMode[] PossibleHackedModes { get { return possibleHackedModes;} }
        private Dictionary<string, HackedMode> modeMapper = new Dictionary<string, HackedMode>();
        private HackedMode associatedHackedMode = null;

        public List<Hackable> staticAdjHackables = new List<Hackable>();
        public List<Hackable> dynamicAdjHackables = new List<Hackable>();
        public List<OrderedHackable<float>> OrderedStaticAdjHackables { get; private set; }

        // Triggered in two scenarios: Focused -> Hacked (Start in alien state)
        // or: Focused (Start in friendly state)
        public event Action<Hackable> OnBased;
        public event Action OnUnbased;
        public event Action OnMadeFriendly;
        public event Action<Hackable> OnControlled;
        
        // UI related events
        // public Action OnAdjToBase;
        // public Action OnAdjToBaseLost;
        public Action OnFocused;
        public Action OnUnfocused;
        // public event Action OnUnfocused;

        void OnValidate() {
            if (networkType.Length != Network.LABEL_SIZE) {
                Debug.LogWarning("Network type should be indicated by three symbols");
                Array.Resize(ref networkType, Network.LABEL_SIZE);
            }
        }

        void Awake() {
            possibleHackedModes = GetComponents<HackedMode>();
            OrderedStaticAdjHackables = new List<OrderedHackable<float>>();
            network = new Network(networkType);
            Array.ForEach(possibleHackedModes, hackedMode => modeMapper.Add(hackedMode.Modename, hackedMode));
        }

        void OnEnable() {
            Array.ForEach(possibleHackedModes, hackedMode => hackedMode.OnHackSucceed += MakeFriendly);
        }

        void OnDisable() {
            Array.ForEach(possibleHackedModes, hackedMode => hackedMode.OnHackSucceed -= MakeFriendly);
        }

        public void OrderStaticAdjHackables() {
            OrderedStaticAdjHackables.Clear();
            staticAdjHackables.ForEach(staticAdjHackable => OrderedStaticAdjHackables.Add(new OrderedHackable<float>(staticAdjHackable, this, OrderedHackable<float>.AngleGetter)));
            OrderedStaticAdjHackables.Sort();
            Debug.Log("Static adjacent hackables for " + name + " ordered");
        }

        public List<OrderedHackable<float>> OrderDynamicAdjHackables() {
            var orderedDynamicAdjHackables = new List<OrderedHackable<float>>();
            dynamicAdjHackables.ForEach(dynamicAdjHackable => orderedDynamicAdjHackables.Add(new OrderedHackable<float>(dynamicAdjHackable,this, OrderedHackable<float>.AngleGetter)));
            orderedDynamicAdjHackables.Sort();
            return orderedDynamicAdjHackables;
        }

        private void MakeFriendly(HackedMode modeHacked) {
            state = State.friendly;
            associatedHackedMode = modeHacked;
            OnMadeFriendly?.Invoke();
            // Can only be called once in lifetime
            OnMadeFriendly = null;
        }

        public void TakeControl() {
            if (associatedHackedMode == null) {
                Debug.LogWarning("Control can only be taken after " + name + " is hacked");
                return;
            }

            Debug.Log("Taking control of " + name);
            state = State.controlled;
            associatedHackedMode.Activate();
            OnControlled?.Invoke(this);
        }

        public void ControlRelinquished() {
            if (associatedHackedMode == null) {
                Debug.LogWarning("Control can only be relinquished after " + name + " is hacked");
                return;
            }

            Debug.Log("Relinquishing control of " + name);
            state = State.friendly;
            associatedHackedMode.Deactivate();
        }

        public bool IsFriendly => state.CompareTo(State.alien) > 0;

        public bool OverlapsWith(Hackable otherHackable) {
            return ((Vector2)(networkCenter.position - otherHackable.networkCenter.position)).sqrMagnitude <= Mathf.Pow(signalRadius + otherHackable.signalRadius, 2);
        }

        public void Unbase() {
            state = State.friendly;
            OnUnbased?.Invoke();
        } 

        public void Base(Hackable input = null) {
            if (state.CompareTo(State.based) < 0) state = State.based;
            Debug.Log("Basing on " + name);
            OnBased?.Invoke(this);
        }

        public void SetAsStartBase() {
            if (state != State.controlled) {
                Debug.LogWarning("Start base should be controlled");
            }
            
            // Manual kick start the associated mode
            if (associatedHackedMode == null && CompareTag("Player")) associatedHackedMode = possibleHackedModes[0];
            Base();
        }

        public void StartHack(Hackable sourceHackable, string hackModeSelected = null) {
            if (IsFriendly) {
                Debug.LogWarning("Hacked device cannot be hacked again");
                return;
            }

            hackModeSelected = hackModeSelected ?? possibleHackedModes[0].Modename;
            if (!modeMapper.ContainsKey(hackModeSelected)) {
                Debug.LogWarning("Invalid hack mode " + hackModeSelected + " selected for " + name);
                return;
            }

            HackedMode selectedHackedMode = modeMapper[hackModeSelected];
            if (selectedHackedMode.BeginHackAttempt(network.HasAffinity(sourceHackable.network))) {
                // After a successful hack attempt, the newly converted hackable is automatically based on
                Base();
                EventManager.InvokeEvent(selectedHackedMode.hackSucceededEvent);
            } else {
                EventManager.InvokeEvent(selectedHackedMode.hackFailedEvent);
                // A generalized version of hack fail event for listeners that do not differentiate the cause of failure
                EventManager.InvokeEvent(generalHackFailed);
            }
        }
    }
}
