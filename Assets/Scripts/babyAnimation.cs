using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BabyAnimation : MonoBehaviour
{
    public Sprite[] animationSprites; // Array to hold animation sprites
    public float animationSpeed = 0.1f; // Speed at which to change sprites
    public AudioClip crySound; // Reference to the cry sound audio clip

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private int currentSpriteIndex = 0;
    private float timer = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // If an AudioSource component is not already attached, add one
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (spriteRenderer != null && animationSprites.Length > 0)
        {
            spriteRenderer.sprite = animationSprites[0];
        }

        PlayCrySound();
    }

    private void Update()
    {
        AnimateSprite();

        // Check if the game is paused or over and handle the cry sound accordingly
        if (IsGamePaused() || IsGameOver())
        {
            StopCrySound();
        }
        else if (!audioSource.isPlaying)
        {
            PlayCrySound();
        }
    }

    private void AnimateSprite()
    {
        if (animationSprites.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= animationSpeed)
        {
            timer = 0f;
            currentSpriteIndex = (currentSpriteIndex + 1) % animationSprites.Length;
            spriteRenderer.sprite = animationSprites[currentSpriteIndex];
        }
    }

    private void PlayCrySound()
    {
        if (crySound != null && !audioSource.isPlaying)
        {
            audioSource.clip = crySound;
            audioSource.Play();
        }
    }

    private void StopCrySound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private bool IsGamePaused()
    {
        // Check if the game is paused
        return Time.timeScale == 0;
    }

    private bool IsGameOver()
    {
        // Implement your game over logic here, for example:
        // return GameManager.Instance.IsGameOver;
        // For demonstration purposes, we'll just return false.
        return false;
    }
}
