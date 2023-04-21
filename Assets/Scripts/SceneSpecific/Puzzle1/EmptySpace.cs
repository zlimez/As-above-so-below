using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptySpace : MonoBehaviour
{
    public ResetPuzzle resetPuzzle;

    private void OnTriggerEnter(Collider collision)
    {
        if (IsPlayer(collision.gameObject))
        {
            resetPuzzle.ResetPuzzle1();
        }
    }

    private bool IsPlayer(GameObject otherObject)
    {
        return otherObject.CompareTag("Player");
    }
}
