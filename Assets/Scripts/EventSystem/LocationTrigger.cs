using UnityEngine;
using System.Collections.Generic;
using Tuples;

namespace Chronellium.EventSystem
{
    public class LocationTrigger : MonoBehaviour
    {
        [SerializeField] private Pair<string, GameEvent>[] targetEntities;
        [SerializeField] private bool shouldRecordEvents;
        private Dictionary<string, GameEvent> entityEventTable = new Dictionary<string, GameEvent>();
        private ActivationClauses activationClauses;

        void Awake()
        {
            activationClauses = GetComponent<ActivationClauses>();

            foreach (Pair<string, GameEvent> entityEvent in targetEntities)
            {
                entityEventTable.Add(entityEvent.head, entityEvent.tail);
            }
        }

        // Tag based so each entity can represent a group
        void OnTriggerEnter(Collider other)
        {
            if (activationClauses == null || activationClauses.IsSatisfied())
            {
                if (entityEventTable.ContainsKey(other.tag))
                {
                    if (shouldRecordEvents)
                    {
                        EventLedger.Instance.RecordEvent(entityEventTable[other.tag]);
                    }
                    EventManager.InvokeEvent(entityEventTable[other.tag]);
                }
            }
        }
    }
}