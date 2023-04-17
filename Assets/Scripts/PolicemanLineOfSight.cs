using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronellium.EventSystem;
using Chronellium.SceneSystem;

public class PolicemanLineOfSight : MonoBehaviour
{
    public bool playerInSightRange;
    public SpriteRenderer spriteRenderer;
    public Conversation caughtPlayerConvo;
    public ChronelliumScene CaughtCutscene;
    private bool IsPlayer(GameObject otherObject) => otherObject.CompareTag("Player");
    private void OnTriggerEnter(Collider collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            if (collision.gameObject.GetComponent<Hideable>().isHidden)
            {
                EventManager.StartListening(StaticEvent.AkaMeeting_StopsHiding, caughtPlayer);
            }
            else
            {
                caughtPlayer();
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            playerInSightRange = false;
        }
        EventManager.StopListeningAll(StaticEvent.AkaMeeting_StopsHiding);
    }

    private void caughtPlayer(object input = null)
    {
        playerInSightRange = true;
        DialogueManager.Instance.StartConversation(caughtPlayerConvo, () => SceneLoader.Instance.PrepLoadWithMaster(CaughtCutscene));
    }
}
