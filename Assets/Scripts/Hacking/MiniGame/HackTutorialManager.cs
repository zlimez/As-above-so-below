using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Chronellium.SceneSystem;

public class HackTutorialManager : BaseSceneManager
{
    public string nextPart;
    public ChronelliumScene nextScene;

    public void Advance()
    {
        if (nextPart == "MainMenu")
        {
            Debug.Log("Returning to main menu");
            SceneManager.LoadSceneAsync("MainMenu");
        }
        else
        {
            SceneLoader.Instance.PrepLoadWithMaster(nextPart);
        }
    }
}
