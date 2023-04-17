using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VirusBaseChoice : MonoBehaviour
{
    public VirusBase virusChoice;
    public TextMeshProUGUI virusText;
    public Image colorIndicator;
    public Button button;
    public event Action<VirusBase> onChoiceSelected;

    void Start() {
        virusText.text = virusChoice.prettyName;
        button.onClick.AddListener(ReactToClick);
    }

    void ReactToClick() {
        onChoiceSelected?.Invoke(virusChoice);
    }
}
