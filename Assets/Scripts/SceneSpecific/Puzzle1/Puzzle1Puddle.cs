using Chronellium.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Puddle : Interactable
{
    public GameObject normCharacter;
    public GameObject swimCharacter;
    public GameObject normCharacterAppearPosition;
    public GameObject normCharacterEndingPosition;
    public GameObject swimCharacterAppearPosition;
    public GameObject swimCharacterEndingPosition;

    public float normLerpSpeed = 100;
    public float waterLerpSpeed = 200;

    private bool isInRealWorld = true;
    private GameEvent EnterRealWorld = new GameEvent("Enter Real World");
    private GameEvent EnterSpiritWorld = new GameEvent("Enter Spirit World");

    public override void Interact()
    {
        if (isInRealWorld)
        {
            EventManager.InvokeEvent(EnterSpiritWorld);
            isInRealWorld = false;
            DropIntoWater();
        }
        else
        {
            EventManager.InvokeEvent(EnterRealWorld);
            isInRealWorld = true;
            JumpOutOfWater();
        }
    }

    private void DropIntoWater()
    {
        swimCharacter.transform.position = swimCharacterAppearPosition.transform.position;
        swimCharacter.SetActive(true);
        normCharacter.SetActive(false);
        StartCoroutine(DropIntoWaterMovement());
    }

    private void JumpOutOfWater()
    {
        normCharacter.transform.position = normCharacterAppearPosition.transform.position;
        normCharacter.SetActive(true);
        swimCharacter.SetActive(false);
        StartCoroutine(JumpOutOfWaterMovement());
    }

    public IEnumerator DropIntoWaterMovement()
    {
        while (swimCharacter.transform.position != swimCharacterEndingPosition.transform.position)
        {
            Debug.Log("Moving in water");
            swimCharacter.transform.position = Vector3.MoveTowards(swimCharacter.transform.position, swimCharacterEndingPosition.transform.position, 2f);
            yield return null;
        }
    }

    public IEnumerator JumpOutOfWaterMovement()
    {
        while (normCharacter.transform.position != normCharacterEndingPosition.transform.position)
        {
            Debug.Log("Moving in air");
            normCharacter.transform.position = Vector3.MoveTowards(normCharacter.transform.position, normCharacterEndingPosition.transform.position, 0.05f);
            yield return null;
        }
    }
}
