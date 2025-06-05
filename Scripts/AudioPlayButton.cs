using UnityEngine;

public class AudioPlayButton : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
