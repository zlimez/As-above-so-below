using Chronellium.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public bool hasAttacked;
    public CableSeats cableSeats;
    private GameEvent EnterRealWorld = new GameEvent("Enter Real World");
    private GameEvent EnterSpiritWorld = new GameEvent("Enter Spirit World");
    private SpriteRenderer wormSprite;
    private bool isPlayerStillInAttackRange;


    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EnterSpiritWorld, ResetAttack);
        wormSprite = GetComponent<SpriteRenderer>();
        wormSprite.enabled = false;
    }

    private void Attack()
    {
        wormSprite.enabled = true;
        // Move worm up
        if (isPlayerStillInAttackRange && cableSeats.isSeated)
        {
            // Apply force to cable car to make it shake
            // Teleport player back to start
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (IsCableSeats(collision.gameObject))
        {
            isPlayerStillInAttackRange = true;
            if (!hasAttacked)
            {
                Attack();
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (IsCableSeats(collision.gameObject))
        {
            isPlayerStillInAttackRange = false;
        }
    }

    private void ResetAttack(object input = null)
    {
        hasAttacked = false;
    }

    private bool IsCableSeats(GameObject otherObject)
    {
        return otherObject.CompareTag("CableSeats");
    }
}
