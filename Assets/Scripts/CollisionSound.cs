using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip hitSound; // Reference to the hit sound audio clip
    public AudioClip missedSound; // Reference to the missed sound audio clip

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // If an AudioSource component is not already attached, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "player"
        if (collision.gameObject.CompareTag("Target"))
        {
            PlaySound(hitSound);
        }
        else
        {
            PlaySound(missedSound);
        }
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
