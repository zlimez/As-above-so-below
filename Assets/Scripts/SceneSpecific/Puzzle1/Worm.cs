using Chronellium.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public bool hasAttacked;
    public CableSeats cableSeats;
    public SpriteRenderer wormSprite;
    private bool isPlayerStillInAttackRange;


    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(StaticEvent.Core_SwitchToOtherWorld, ResetAttack);
        wormSprite = GetComponent<SpriteRenderer>();
        wormSprite.enabled = false;
    }

    private void Attack()
    {
        wormSprite.enabled = true;
        Debug.Log("PlayerInRange: " + isPlayerStillInAttackRange);
        // TODO: Move worm sprite up
        if (isPlayerStillInAttackRange && cableSeats.isSeated)
        {
            // Apply force to cable car to make it shake
            StartCoroutine(WaitBeforeReset());
        }
        StartCoroutine(WaitBeforeDisappear());
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject);
        if (IsCableSeats(collision.gameObject))
        {
            isPlayerStillInAttackRange = true;
            if (!hasAttacked)
            {
                //Attack();
                StartCoroutine(WaitBeforeAttack());
                hasAttacked = true;
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

    public void ResetAttack(object input = null)
    {
        hasAttacked = false;
    }

    private bool IsCableSeats(GameObject otherObject)
    {
        return otherObject.CompareTag("CableSeats");
    }

    private IEnumerator WaitBeforeReset(object input = null)
    {
        yield return new WaitForSeconds(1);
        EventManager.InvokeEvent(StaticEvent.Core_ResetPuzzle);
    }

    private IEnumerator WaitBeforeAttack(object input = null)
    {
        yield return new WaitForSeconds(1);
        Attack();
    }

    private IEnumerator WaitBeforeDisappear(object input = null)
    {
        yield return new WaitForSeconds(1);
        wormSprite.enabled = false;
    }
}
