using UnityEngine;
using Chronellium.EventSystem;
using UnityEngine.SceneManagement;

public class Puzzle1 : MonoBehaviour
{
    private void Start()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPuzzle();
        }
        EventManager.StartListening(StaticEvent.Core_ResetPuzzle, ResetPuzzle);
    }

    private void ResetPuzzle(object input = null)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
