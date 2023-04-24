using UnityEngine;

namespace Chronellium.EventSystem
{
    public class OneTimeConvo : MonoBehaviour
    {
        private bool hasTriggered;
        public Conversation convo;
        public GameEvent optionalInvokeEvent;

        // Tag based so each entity can represent a group
        void OnTriggerEnter(Collider other)
        {
            if (hasTriggered) return;
            if (optionalInvokeEvent != null && optionalInvokeEvent.RelatedStaticEvent != StaticEvent.NoEvent) EventManager.InvokeEvent(optionalInvokeEvent);
            DialogueManager.Instance.StartConversation(convo);
            hasTriggered = true;

        }
    }
}