using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackUIWrapper : MonoBehaviour
{
    public GameObject startButton;
    public GameObject nextButton;
    public LevelDescription levelDescriptionPanel;
    [SerializeField] private HackGameManager hackGameManager;

    void Awake() {
        levelDescriptionPanel.Populate(hackGameManager.title, hackGameManager.description, hackGameManager.hint);
    }

    void OnEnable() {
        hackGameManager.onInputsReady += EnableStartButton;
        hackGameManager.onBestOutcomeAchieved += EnableNextButton;
        startButton.GetComponent<Button>().onClick.AddListener(hackGameManager.StartMiniGame);
    }

    void OnDisable() {
        hackGameManager.onInputsReady -= EnableStartButton;
        hackGameManager.onBestOutcomeAchieved -= EnableNextButton;
        startButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void EnableStartButton() {
        startButton.SetActive(true);
    }

    private void EnableNextButton() {
        nextButton.SetActive(true);
    }
}
