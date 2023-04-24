using Chronellium.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{
    public CableSeats cableSeats;
    public SpriteRenderer wormSprite;
    private bool attackFinished;
    private bool isAttacking;
    public GameObject initialWormPosition;
    public GameObject attackFinalWormPosition;
    public float attackSpeed = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(StaticEvent.Core_SwitchToOtherWorld, ResetAttack);
        EventManager.StartListening(StaticEvent.Core_ResetPuzzle, ResetAttack);
        wormSprite = GetComponent<SpriteRenderer>();
        wormSprite.enabled = false;
    }

    private void Update()
    {
        if (attackFinished)
        {
            wormSprite.enabled = false;
        }

        if (isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackFinalWormPosition.transform.position, attackSpeed);
        }
    }

    public void Attack()
    {
        wormSprite.enabled = true;
        // Move worm towards left in Update
        isAttacking = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (IsCableSeats(collision.gameObject))
        {
            // Apply force to cable car to make it shake

            if (cableSeats.isSeated)
            {
                StartCoroutine(WaitBeforeReset());
            }
        }
    }

    public void ResetAttack(object input = null)
    {
        attackFinished = false;
        wormSprite.enabled = false;
        isAttacking = false;
        transform.position = initialWormPosition.transform.position;
    }

    private bool IsCableSeats(GameObject otherObject)
    {
        return otherObject.CompareTag("CableSeats");
    }

    private IEnumerator WaitBeforeReset(object input = null)
    {
        yield return new WaitForSeconds(0.5f);
        EventManager.InvokeEvent(StaticEvent.Core_ResetPuzzle);
    }
}
