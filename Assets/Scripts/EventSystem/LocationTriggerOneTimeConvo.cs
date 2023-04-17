using UnityEngine;

namespace Chronellium.EventSystem
{
    public class LocationTriggerOneTimeConvo : MonoBehaviour
    {
        private bool hasTriggered;
        public Conversation convo;
        public GameEvent optionalInvokeEvent;

        // Tag based so each entity can represent a group
        void OnTriggerEnter(Collider other)
        {
            if (hasTriggered) return;
            if (optionalInvokeEvent != null ) EventManager.InvokeEvent(optionalInvokeEvent);
            DialogueManager.Instance.StartConversation(convo);
            hasTriggered = true;
        }
    }
}