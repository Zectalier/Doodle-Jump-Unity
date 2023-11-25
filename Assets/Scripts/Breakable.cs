using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite sprite3;

    public float timeBetweenSwaps = 0.5f;
    public SpriteRenderer spriteRenderer;

    public AudioSource break_sound;
    public void swapSprite()
    {
        break_sound.Play();
        StartCoroutine(spriteSwapping());
    }

    IEnumerator spriteSwapping()
    {
        spriteRenderer.sprite = sprite1;
        yield return new WaitForSeconds(timeBetweenSwaps);
        spriteRenderer.sprite = sprite2;
        yield return new WaitForSeconds(timeBetweenSwaps);
        spriteRenderer.sprite = sprite3;
    }
}
