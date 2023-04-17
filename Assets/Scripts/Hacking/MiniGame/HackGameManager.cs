using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Chronellium.MiniGames;

public class HackGameManager:  MiniGameUnitBase
{
    public InputNodeView[] allInputNodes;
    public OutputNodeView[] allOutputNodes;
    public OptionalOutputNodeView[] allOptionalOutputNodes;
    public bool gameInProgess;

    public VirusBaseMenu virusInputMenu;

    [SerializeField] private int inputProvided = 0;
    [SerializeField] private int endPointCount;
    [SerializeField] private int endPointReached = 0;
    [SerializeField] private int outputDestroyed = 0;

    // NOTE: For tutorial purposes only
    private int maxOutputDestroyable = 0;
    public event Action onBestOutcomeAchieved;

    public event Action onInputsReady;

    void Awake()
    {
        this.gameMode = MiniGameMode.HACKING;
        endPointCount = allInputNodes.Length;
        maxOutputDestroyable = Mathf.Min(allOutputNodes.Length + allOptionalOutputNodes.Length, allInputNodes.Length); 
    }

    void OnEnable() {
        foreach (InputNodeView inputNodeView in allInputNodes) {
            inputNodeView.onSelected += OpenVirusInputMenu;
        }

        foreach (OutputNodeView outputNodeView in allOutputNodes) {
            outputNodeView.onVirusReachedEnd += CheckGameEnded;
            outputNodeView.RegisterDestroyResponse(DefaultCheckGameEnded);
            outputNodeView.RegisterDestroyResponse(IncrementDestroyCount);
        }

        foreach (OptionalOutputNodeView optionalOutputNodeView in allOptionalOutputNodes) {
            optionalOutputNodeView.RegisterDestroyResponse(DefaultCheckGameEnded);
            optionalOutputNodeView.RegisterDestroyResponse(IncrementDestroyCount);
        }

        virusInputMenu.onInitialized += IncrementInputCount;
    }

    void OnDisable() {
        foreach (InputNodeView inputNodeView in allInputNodes) {
            inputNodeView.onSelected -= OpenVirusInputMenu;
        }

        foreach (OutputNodeView outputNodeView in allOutputNodes) {
            outputNodeView.onVirusReachedEnd -= CheckGameEnded;
            outputNodeView.UnregisterDestroyResponse(DefaultCheckGameEnded);
            outputNodeView.UnregisterDestroyResponse(IncrementDestroyCount);
        }

        foreach (OptionalOutputNodeView optionalOutputNodeView in allOptionalOutputNodes) {
            optionalOutputNodeView.UnregisterDestroyResponse(DefaultCheckGameEnded);
            optionalOutputNodeView.UnregisterDestroyResponse(IncrementDestroyCount);
        }

        virusInputMenu.onInitialized -= IncrementInputCount;
    }

    // NOTE: Currently clearing input to null is not supported hence decrementInputCOunt not needed
    private void IncrementInputCount() {
        inputProvided += 1;
        if (inputProvided == allInputNodes.Length) onInputsReady?.Invoke();
    }

    private void OpenVirusInputMenu(InputNodeView triggeringNode) {
        virusInputMenu.OpenMenu(triggeringNode);
    }

    public override void StartMiniGame()
    {
        if (gameInProgess) return;
        endPointReached = 0;
        gameInProgess = true;

        foreach (InputNodeView inputNodeView in allInputNodes)
        {
            inputNodeView.StartStreamWithInput();
        }
    }

    // Game considered ended when all output node destroy incoming virus,
    // since they are located at the end points of the graph.
    private void CheckGameEnded(int layersRemoved)
    {
        endPointReached += layersRemoved;
        gameInProgess = endPointReached != endPointCount;

        if (!gameInProgess)
        {
            if (outputDestroyed == maxOutputDestroyable) {
                onBestOutcomeAchieved?.Invoke();
                // Assuming the game is successful when all layers are removed
                MiniGameSolvedSuccessfully();
            }
        }
    }

    // Considers only the output node destroy case
    private void DefaultCheckGameEnded() {
        CheckGameEnded(1);
    }

    // NOTE: For tutorial
    private void IncrementDestroyCount() {
        outputDestroyed += 1;
    }

    public override void PauseMiniGame() {
        Time.timeScale = 0;
    }

    public override void ResumeMiniGame() {
        Time.timeScale = 1;
    }
}
