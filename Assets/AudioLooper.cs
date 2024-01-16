using UnityEngine;

public class AudioLooper : MonoBehaviour
{
    // Drag and drop your audio clip in the Unity Editor
    public AudioClip audioClip;

    // Set the delay between loops in seconds
    public float loopDelay = 2f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure an audio clip is assigned
        if (audioClip == null)
        {
            Debug.LogError("Audio clip not assigned!");
            return;
        }

        // Set the audio clip for the AudioSource
        audioSource.clip = audioClip;

        // Start playing the audio loop
        PlayAudioLoop();
    }

    private void PlayAudioLoop()
    {
        // Play the audio clip
        audioSource.Play();

        // Invoke the method to play the audio loop again after the delay
        Invoke("PlayAudioLoop", loopDelay);
    }

    // You can add other methods or events as needed for your game
}
