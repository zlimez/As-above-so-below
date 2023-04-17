using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Chronellium.Hacking.UI {
    public class HackModeChoice : MonoBehaviour
    {
        public TextMeshProUGUI modenameText;
        public Button button;
        public event Action<string> onModeSelected;

        void Start() {
            button.onClick.AddListener(OnSelected);
        }

        void OnSelected() {
            onModeSelected?.Invoke(modenameText.text);
        }
    }
}
