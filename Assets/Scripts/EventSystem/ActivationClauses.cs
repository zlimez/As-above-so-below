using System;
using UnityEngine;
using Tuples;

namespace Chronellium.EventSystem
{
    public class ActivationClauses : MonoBehaviour
    {
        // NOTE: Per row it is an "and" relation, between rows it is an "or" relation to determine whether the clause as a whole is satisified
        // NOTE: Per triplet, if first bool is true, check event occurred, else check event never occur
        // NOTE: if second bool true, the event is checked against looped past, else conventional past
        [SerializeField] private Clause[] activationRules;
        private Clauses activationClauses;
        public string identifier;

        void Awake()
        {
            activationClauses = new Clauses(activationRules);
        }

        public bool IsSatisfied()
        {
            return activationClauses.IsSatisfied();
        }

        [Serializable]
        public struct Clauses
        {
            private Clause[] activationRules;

            public Clauses(Clause[] activationRules)
            {
                this.activationRules = activationRules;
            }

            public bool IsSatisfied()
            {
                foreach (Clause activationRule in activationRules)
                {
                    if (activationRule.IsSatisfied())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [Serializable]
        public struct Clause
        {
            public Triplet<GameEvent, bool, bool>[] eventElements;

            public Clause(Triplet<GameEvent, bool, bool>[] eventElements)
            {
                this.eventElements = eventElements;
            }

            public bool IsSatisfied()
            {
                bool ruleSatisfied = true;
                foreach (Triplet<GameEvent, bool, bool> eventElement in eventElements)
                {
                    bool condition = eventElement.Item3
                        ? EventLedger.Instance.HasEventOccurredInLoopedPast(eventElement.Item1)
                        : EventLedger.Instance.HasEventOccurredInPast(eventElement.Item1);

                    if (!eventElement.Item2) condition = !condition;

                    // Debug.Log($"{eventElement.Item1.eventName} condition {condition}");
                    ruleSatisfied = ruleSatisfied && condition;
                }
                return ruleSatisfied;
            }
        }
    }
}