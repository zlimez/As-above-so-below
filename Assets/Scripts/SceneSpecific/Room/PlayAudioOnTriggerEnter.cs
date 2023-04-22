using UnityEngine;

public class PlayAudioOnTriggerEnter : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (audioSource != null && other.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }
}