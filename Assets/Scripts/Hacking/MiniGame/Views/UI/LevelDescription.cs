using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelDescription : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Text description;
    public Text hint;
    private Animator animator;
    private bool isOpen = true;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isOpen) {
                Close();
            } else {
                Open();
            }
        }
    }

    public void Populate(string title, string description, string hint) {
        this.title.SetText(title);
        this.description.text = description;
        this.hint.text = hint;
    }

    public void Open() {
        animator.SetTrigger("openTrigger");
        isOpen = true;
    }

    public void Close() {
        animator.SetTrigger("closeTrigger");
        isOpen = false;
    }
}
