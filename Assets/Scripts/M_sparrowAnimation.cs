using System.Collections;

using UnityEngine;

public class M_sparrowAnimation : MonoBehaviour
{
    public Sprite[] animationSprites; // Array to hold animation sprites
    public float animationSpeed = 0.1f; // Speed at which to change sprites
    private SpriteRenderer spriteRenderer;

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

    private void Update()
    {
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
}
