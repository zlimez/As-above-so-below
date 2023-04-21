using System.Collections;
using Chronellium.EventSystem;
using UnityEngine;

public class Newspapers : Interactable
{
    private GameEvent testFirstTimeEvent = new GameEvent("Jonas Has looked at the newspapers");
    [SerializeField] Conversation inquiry1, inquiry2, inquiry3, inquiry4;
    private int dialogId = 0;

    void Update()
    {
        TryInteract();
    }

    public override void Interact()
    {

        switch (dialogId)
        {
            case 0:
                DialogueManager.Instance.StartConversation(inquiry1);
                break;
            case 1:
                DialogueManager.Instance.StartConversation(inquiry2);
                break;
            case 2:
                DialogueManager.Instance.StartConversation(inquiry3);
                break;
            case 3:
                DialogueManager.Instance.StartConversation(inquiry4);
                return;
                break;
            default:
                break;
        }
        dialogId++;
    }
}
