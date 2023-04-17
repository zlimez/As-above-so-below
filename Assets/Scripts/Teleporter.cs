using UnityEngine;
using KinematicCharacterController;
using Chronellium.EventSystem;

public class Teleporter : Interactable
{
    public Transform teleportPosition;
    [SerializeField] private string yesTeleport, noTeleport;
    [SerializeField] private Conversation teleporterBlock;
    private Choice doTeleport, notTeleport;
    private ActivationClauses activationClauses;

    void Awake()
    {
        activationClauses = GetComponent<ActivationClauses>();
        doTeleport = new Choice(yesTeleport, PrepToTeleport, true);
        notTeleport = new Choice(noTeleport, NoTeleport, true);
    }

    public override void Interact()
    {
        if (activationClauses == null || activationClauses.IsSatisfied())
        {
            ChoiceManager.Instance.StartChoice(doTeleport, notTeleport);
        }
        else
        {
            DialogueManager.Instance.StartConversation(teleporterBlock);
        }
    }

    private void PrepToTeleport(object input = null)
    {
        EventManager.StartListening(CommonEventCollection.CurtainFullyDrawn, Teleport);
        EventManager.InvokeEvent(CommonEventCollection.PrepToTeleport);
    }

    private void Teleport(object input = null)
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        Debug.Log("teleport position: " + teleportPosition.position);
        player.GetComponent<KinematicCharacterMotor>().SetPosition(teleportPosition.position, true);
        Debug.Log("player positon: " + player.transform.position);
        EventManager.StopListening(CommonEventCollection.CurtainFullyDrawn, Teleport);
    }

    private void NoTeleport(object input = null) { }
}