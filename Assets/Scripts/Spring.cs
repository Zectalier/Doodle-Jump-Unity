using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public Sprite usedSpringSprite;
    public SpriteRenderer spriteRenderer;

    public AudioSource spring_audio;
    public void swapSprite()
    {
        spriteRenderer.sprite = usedSpringSprite;
        transform.position += new Vector3(0, 0.16f, 0);
        spring_audio.Play();
    }
}
