using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Chronellium.EventSystem;


public class TutorialController : MonoBehaviour
{
    [SerializeField] Sprite[] tutorialSprite;
    [SerializeField, TextArea(3, 5)] string[] tutorialText;
    [SerializeField] Image image;
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject tutorialHolder;
    [SerializeField] AudioClip hover;
    [SerializeField] AudioSource audio;
    [SerializeField] GameObject left, right;
    enum Tutorial {rewind, inventory, skip};
    [SerializeField] Tutorial tutorial;
    float audioVolume;
    int currIndex, length;
    bool inTutorial;

    // Start is called before the first frame update
    void Start()
    {
        if (EventLedger.Instance.HasEventOccurredInLoopedPast(StaticEvent.CommonEvents_RewindTutorialTriggered) &&
            EventLedger.Instance.HasEventOccurredInLoopedPast(StaticEvent.CommonEvents_InventoryTutorialTriggered))
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inTutorial)
            return;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ReadNext();
            return;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ReadLast();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    void ReadNext()
    {
        currIndex++;

        if (currIndex == length)
        {
            currIndex = length - 1;
            return;
        }

        right.SetActive(currIndex != length - 1);
        left.SetActive(currIndex != 0);

        image.sprite = tutorialSprite[currIndex];
        text.text = tutorialText[currIndex];
    }

    void ReadLast()
    {
        currIndex--;

        if (currIndex < 0)
        {
            currIndex = 0;
            return;
        }
        right.SetActive(currIndex != length - 1);
        left.SetActive(currIndex != 0);

        image.sprite = tutorialSprite[currIndex];
        text.text = tutorialText[currIndex];
    }

    void OnEnable()
    {
        switch (tutorial)
        {
            case Tutorial.rewind: 
            EventManager.StartListening(StaticEvent.CommonEvents_RewindTutorialTriggered, TutorialStart);
            break;
            case Tutorial.inventory: 
            EventManager.StartListening(StaticEvent.CommonEvents_InventoryTutorialTriggered, TutorialStart);
            break;
            case Tutorial.skip: 
            EventManager.StartListening(StaticEvent.CommonEvents_SkipTutorialTriggered, TutorialStart);
            break;
        }
    }

    void OnDisable()
    {
        switch (tutorial)
        {
            case Tutorial.rewind: 
            EventManager.StopListening(StaticEvent.CommonEvents_RewindTutorialTriggered, TutorialStart);
            break;
            case Tutorial.inventory: 
            EventManager.StopListening(StaticEvent.CommonEvents_InventoryTutorialTriggered, TutorialStart);
            break;
            case Tutorial.skip: 
            EventManager.StopListening(StaticEvent.CommonEvents_SkipTutorialTriggered, TutorialStart);
            break;
        }
    }

    public void TutorialStart(object o = null)
    {
        StartCoroutine(WaitForDialog());
    }

    void Open()
    {

        audioVolume = audio.volume;
        StartCoroutine(AudioFadeOut(audioVolume));

        AudioManager.Instance.StartPlayingUiAudio(hover);

        UiStatus.OpenUI();
        tutorialHolder.SetActive(true);

        currIndex = -1;
        inTutorial = true;
        length = tutorialText.Length;
        ReadNext();
    }

    void Close()
    {
        StartCoroutine(AudioFadeIn(audioVolume));
        AudioManager.Instance.StartPlayingUiAudio(hover);
        UiStatus.CloseUI();
        tutorialHolder.SetActive(false);


        inTutorial = false;
        image.sprite = null;
        text.text = "";
        currIndex = -1;
        length = -1;
    }

    IEnumerator WaitForDialog()
    {
        while (DialogueManager.Instance.InDialogue)
        {
            yield return new WaitForSeconds(0.1f);
        }

        Open();
    }

    IEnumerator AudioFadeOut(float audioVolume)
    {
        float currentTime = 0;

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;

            audio.volume = Mathf.Lerp(audioVolume, 0, currentTime / 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator AudioFadeIn(float audioVolume)
    {
        float currentTime = 0;

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;

            audio.volume = Mathf.Lerp(0, audioVolume, currentTime / 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
