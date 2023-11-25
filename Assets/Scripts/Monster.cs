using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public AudioSource monster_audio;
    public AudioSource monster_death_audio;

    private void Start()
    {
        monster_audio.Play();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Projectile")
            doDeath();
    }

    public void doDeath() // RIP ;(
    {
        monster_audio.Stop();
        monster_death_audio.Play();
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        Destroy(this.gameObject, 5f);   
    }
}
