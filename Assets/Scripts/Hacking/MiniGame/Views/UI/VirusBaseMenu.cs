using System;
using UnityEngine.Events;
using UnityEngine;

public class VirusBaseMenu : MonoBehaviour
{
    // Need to manually match number of choices with number of available viruses. Can be made dynamic.
    public VirusBase[] allVirusBases;
    public VirusBaseChoice[] virusBaseChoices;
    private VirusBase selectedChoice;
    [NonSerialized] public UnityEvent<VirusBase> onChoiceChanged = new UnityEvent<VirusBase>();
    public event Action onInitialized;
    public InputNodeView bindedNode = null;
    private Animator animator;
    private bool isOpen = false;

    void Awake() {
        animator = GetComponent<Animator>();
        for (int i = 0; i < allVirusBases.Length; i++) {
            virusBaseChoices[i].virusChoice = allVirusBases[i];
            virusBaseChoices[i].colorIndicator.color = allVirusBases[i].color;
        }
    }

    void OnEnable() {
        foreach (VirusBaseChoice choice in virusBaseChoices) {
            choice.onChoiceSelected += SetSelectedChoice;
        }
    }

    void OnDisable() {
        foreach (VirusBaseChoice choice in virusBaseChoices) {
            choice.onChoiceSelected -= SetSelectedChoice;
        }
        onChoiceChanged.RemoveAllListeners();
    }

    void SetSelectedChoice(VirusBase selectedVirus) {
        if (selectedChoice == selectedVirus) return;
        if (bindedNode.SelectedInput == null) onInitialized?.Invoke();

        selectedChoice = selectedVirus;
        onChoiceChanged?.Invoke(selectedChoice);
        CloseMenu();
    }

    public void OpenMenu(InputNodeView triggeringNode) {
        if (bindedNode != null) ResetBindedNode();

        // gameObject.SetActive(true);
        if (!isOpen) {
            animator.SetTrigger("openTrigger");
            isOpen = true;
        }
        selectedChoice = null;
        bindedNode = triggeringNode;
        onChoiceChanged.RemoveAllListeners();
        onChoiceChanged.AddListener(bindedNode.ChangeSelectedInput);
    }

    public void CloseMenu() {
        animator.SetTrigger("closeTrigger");
        isOpen = false;
        ResetBindedNode();
    }

    private void ResetBindedNode() {
        bindedNode.onDeselected?.Invoke();
        bindedNode = null;
    }
}
