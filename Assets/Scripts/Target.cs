using UnityEngine;

public class Target : MonoBehaviour
{
    public delegate void TargetHitHandler(Target target);
    public event TargetHitHandler OnTargetHitEvent;
    public event TargetHitHandler OnTargetReachMidpointEvent;

    public Sprite[] animationSprites; // Array to hold animation sprites
    public float animationSpeed = 0.1f; // Speed at which to change sprites
    public AudioClip hitSound; // Reference to the hit sound effect
    public ParticleSystem hitParticles; // Reference to the smoke particle system
    private SpriteRenderer spriteRenderer;
    private Vector3 midPoint;
    public float speed = 1f;

    private int currentSpriteIndex = 0;
    private float timer = 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (spriteRenderer != null && animationSprites.Length > 0)
        {
            spriteRenderer.sprite = animationSprites[0];
        }
    }

    public void SetMidPoint(Vector3 position)
    {
        midPoint = position;
    }

    private void Update()
    {
        // Move the target towards the midpoint
        transform.position = Vector3.MoveTowards(transform.position, midPoint, speed * Time.deltaTime);

        // Check if the target has reached the midpoint
        if (transform.position == midPoint)
        {
            OnTargetReachMidpointEvent?.Invoke(this);
        }

        // Animate the target sprite
        AnimateSprite();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            HandleHit();
            Destroy(gameObject);
        }
    }

    public void HandleHit()
    {
        // Play the hit sound
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        // Emit smoke particles
        if (hitParticles != null)
        {
            // Instantiate the particle system
            ParticleSystem particles = Instantiate(hitParticles, transform.position, Quaternion.identity);

            // Adjust the main module for the particle system
            var main = particles.main;
            main.duration = 0.5f; // Set this to a short duration
            main.startLifetime = 0.3f; // Ensure this is consistent to avoid double bursts
            main.loop = false; // Disable looping to ensure it plays only once

            particles.Play();

            // Destroy the particle system after it finishes playing
            Destroy(particles.gameObject, main.duration + main.startLifetime.constantMax);
        }

        OnTargetHitEvent?.Invoke(this);
    }



}
