using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chronellium.EventSystem;

public class SnapshotTakenHint : MonoBehaviour
{
    public bool isActive;
    public GameObject hintBox;
    public Transform hintBoxTransform;
    public Text nameHolder;
    public Image image;
    private Animator animator;
    private string snapshotName;
    private Sprite snapshotSprite;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Show(Snapshot snapshot)
    {
        snapshotName = snapshot.SceneName;
        snapshotSprite = snapshot.Screenshot;
        StartCoroutine(Display());
    }

    IEnumerator Display()
    {
        nameHolder.text = snapshotName;
        image.sprite = snapshotSprite;
        hintBox.SetActive(true);
        animator.CrossFade("Window In", 0.1f);

        yield return new WaitForSeconds(2f);
        animator.CrossFade("Window Out", 0.1f);
        yield return new WaitForSeconds(0.5f);

        isActive = false;
        snapshotName = "";
        image.sprite = null;
        hintBox.SetActive(false);
    }
}
