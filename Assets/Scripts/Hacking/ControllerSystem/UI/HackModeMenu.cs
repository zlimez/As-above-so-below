using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Chronellium.Hacking.UI {
    public class HackModeMenu : MonoBehaviour
    {
        public GameObject choicePrefab;
        private Hackable bindedHackable;
        private Hackable basedHackable;
        private List<GameObject> hackModeChoices = new List<GameObject>();
        [SerializeField] private string selectedModename;
        private Animator animator;

        void Awake() {
            animator = GetComponent<Animator>();
        }
        
        public void OpenMenu(Hackable triggerHackable, Hackable sourceHackable) {
            selectedModename = "";
            hackModeChoices.ForEach(hackModeChoice => Destroy(hackModeChoice));
            hackModeChoices.Clear();
            bindedHackable = triggerHackable;
            basedHackable = sourceHackable;
            foreach (var possibleHackedMode in bindedHackable.PossibleHackedModes) {
                // TODO: Check instantiate transform
                GameObject modeChoiceObject = Instantiate(choicePrefab, transform);
                modeChoiceObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                HackModeChoice modeChoice = modeChoiceObject.GetComponent<HackModeChoice>();
                modeChoice.modenameText.SetText(possibleHackedMode.Modename);
                modeChoice.onModeSelected += SetSelectedModename;
                hackModeChoices.Add(modeChoiceObject);
            }
            animator.SetTrigger("openTrigger");
        }

        public void CloseMenu() {
            animator.SetTrigger("closeTrigger");
            Debug.Log("Selected hack mode is " + selectedModename);
            bindedHackable.StartHack(basedHackable, selectedModename);
            bindedHackable = null;
            basedHackable = null;
        }

        private void SetSelectedModename(string newSelectedModename) {
            selectedModename = newSelectedModename;
            CloseMenu();
        }
    }
}
