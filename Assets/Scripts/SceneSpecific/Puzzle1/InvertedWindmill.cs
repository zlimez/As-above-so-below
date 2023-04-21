using Chronellium.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedWindmill : Interactable
{
    public GameObject windmillBlades;
    public string choiceText1, choiceText2;
    private Choice choice1, choice2;
    private bool choice1Activated = true;
    private bool choice2Activated = true;
    private IEnumerator turnClockwise;
    private IEnumerator turnAntiClockwise;
    private float rotationZ;

    private GameEvent EnterRealWorld = new GameEvent("Enter Real World");
    private GameEvent EnterSpiritWorld = new GameEvent("Enter Spirit World");


    void Awake()
    {
        InitialiseChoice();
        turnClockwise = TurnClockwise();
        turnAntiClockwise = TurnAntiClockwise();
        rotationZ = windmillBlades.transform.rotation.z;
        EventManager.StartListening(EnterRealWorld, ResetBlades);
    }

    private void InitialiseChoice()
    {
        choice1 = new Choice(choiceText1, Choice1, choice1Activated);
        choice2 = new Choice(choiceText2, Choice2, choice2Activated);
    }

    public override void Interact()
    {
        ChoiceManager.Instance.StartChoice(choice1, choice2);
    }

    // Invoked Events
    // Change according to what is required
    public void Choice1(object o = null)
    {
        // Turn Clockwise
        StopCoroutine(turnClockwise);
        StopCoroutine(turnAntiClockwise);
        StartCoroutine(turnClockwise);
    }

    public void Choice2(object o = null)
    {
        // Turn Anti clockwise
        StopCoroutine(turnClockwise);
        StopCoroutine(turnAntiClockwise);
        StartCoroutine(turnAntiClockwise);
    }

    public IEnumerator TurnClockwise()
    {
        while (true)
        {
            rotationZ += 1;
            windmillBlades.transform.rotation = Quaternion.Euler(windmillBlades.transform.rotation.x, windmillBlades.transform.rotation.y, rotationZ);
            yield return null;
        }
    }

    public IEnumerator TurnAntiClockwise()
    {
        while (true)
        {
            rotationZ -= 1;
            windmillBlades.transform.rotation = Quaternion.Euler(windmillBlades.transform.rotation.x, windmillBlades.transform.rotation.y, rotationZ);
            yield return null;
        }
    }

    private void ResetBlades(object input = null)
    {
        StopCoroutine(turnClockwise);
        StopCoroutine(turnAntiClockwise);
        windmillBlades.transform.rotation = Quaternion.Euler(windmillBlades.transform.rotation.x, windmillBlades.transform.rotation.y, 0);
    }
}
