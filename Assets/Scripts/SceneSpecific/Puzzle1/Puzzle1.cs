using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1 : MonoBehaviour
{
    public Conversation startingConvo;
    private void Start()
    {
        DialogueManager.Instance.StartConversation(startingConvo);
    }
}
