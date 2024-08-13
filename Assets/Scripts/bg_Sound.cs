using UnityEngine;

public class bg_Sound : MonoBehaviour
{
    public AudioClip backgroundMusic; 

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // If an AudioSource component is not already attached, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = backgroundMusic; // Assign the background music clip to the AudioSource
        audioSource.loop = true; // Set the music to loop
        audioSource.playOnAwake = true; // Play the music when the game starts
        audioSource.volume = 0.5f; // Set the volume (0.0 to 1.0)

        audioSource.Play(); // Start playing the background music
    }
}
