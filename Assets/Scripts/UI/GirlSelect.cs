using UnityEngine;
using System.Collections;
using DigitalRuby.Tween;
using Chronellium.SceneSystem;
using Chronellium.EventSystem;


// This script controls the UI for the girl select screen in the brothel
// Not designed to be extensible, hardcoded to select between the 
// first two objects in Gradients and CardSelect borders.
// In this case between Ginger and Candy
// After you select with "Enter", the other gradients fade out

public class GirlSelect : MonoBehaviour
{
    [SerializeField]
    GameObject[] cardSelectBorders;

    [SerializeField]
    GameObject[] gradients;

    [SerializeField]
    GameObject overlay;
    [SerializeField]
    float fadeOutTimeSeconds = 1.0f;
    [SerializeField] private Conversation wrongGirlPrompt;
    [SerializeField] AudioClip hover, select;

    private int index; // Hard coded the selected girl. 0 -> Ginger, 1 -> Candy
    private bool hasSelected;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;

        foreach (GameObject item in cardSelectBorders)
        {
            item.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (hasSelected) return;

        if (InputManager.InteractButtonActivated)
        {
            InputManager.InteractButtonActivated = false;
            if (index == 0 && !EventLedger.Instance.HasEventOccurredInLoopedPast(StaticEvent.BrothelRewindEvents_FirstIterationCompleted))
            {
                DialogueManager.Instance.StartConversation(wrongGirlPrompt);
                return;
            }

            AudioManager.Instance.StartPlayingUiAudio(select);
            StartCoroutine(Selected());
            hasSelected = true;
        }

        cardSelectBorders[index].SetActive(false);

        if (Input.GetKeyDown("left") || Input.GetKeyDown(KeyCode.A))
        {
            AudioManager.Instance.StartPlayingUiAudio(hover);
            index--;
        }
        if (Input.GetKeyDown("right") || Input.GetKeyDown(KeyCode.D))
        {
            AudioManager.Instance.StartPlayingUiAudio(hover);
            index++;
        }

        index = Mathf.Clamp(index, 0, cardSelectBorders.Length - 1);
        cardSelectBorders[index].SetActive(true);
    }

    // This method Fades out a sprite using Tweens
    private void FadeOut(GameObject sprite)
    {
        SpriteRenderer spriteRenderer = sprite.GetComponent<SpriteRenderer>();
        System.Action<ITween<Color>> updateColor = (t) =>
        {
            spriteRenderer.color = t.CurrentValue;
        };

        Color endColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        sprite.gameObject.Tween(sprite.GetInstanceID(), spriteRenderer.color, endColor, fadeOutTimeSeconds, TweenScaleFunctions.QuadraticEaseOut, updateColor);
    }

    IEnumerator Selected()
    {
        // Fade out every gradient except the one currently selected
        for (int i = 0; i < gradients.Length; i++)
        {
            if (i != index)
            {
                FadeOut(gradients[i]);
            }
        }

        yield return new WaitForSeconds(fadeOutTimeSeconds);

        // Transition out of this scene back to the main game
        // Set Ledger variable for the choice made
        if (index == 0)
        {
            EventLedger.Instance.RecordEvent(StaticEvent.LobbyEvents_OnGingerChosen);
            EventManager.QueueEvent(StaticEvent.LobbyEvents_OnGingerChosen);
        }
        else if (index == 1)
        {
            EventLedger.Instance.RecordEvent(StaticEvent.LobbyEvents_OnCandyChosen);
            EventManager.QueueEvent(StaticEvent.LobbyEvents_OnCandyChosen);
        }

        SceneLoader.Instance.PrepLoadWithMaster(ChronelliumScene.BrothelLobby);
    }
}