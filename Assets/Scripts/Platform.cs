using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 10f;
    public string platformType = "Default";

    public AudioSource jump_audio;
    public AudioSource spring_audio;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0F) //Check if object colliding is coming from below
        {
            // check if the contact point is below the bottom of the collider
            ContactPoint2D contact = collision.GetContact(0);
            float contact_y = contact.point.y;
            float collider_y = collision.collider.bounds.center.y - collision.collider.bounds.extents.y;
            if (contact_y <= collider_y){
                Rigidbody2D rb = collision.collider.attachedRigidbody;
                if (rb != null)
                {
                    Vector2 velocity = rb.velocity;
                    velocity.y = jumpForce;
                    rb.velocity = velocity;
                    ApplySpecialBehaviour(platformType);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish") //If the platform touch the finish trigger box, destroy this
            Destroy(this.gameObject,0.5f);
        else if (collision.tag == "Player" && platformType == "Breakable") //If the trigger is the player, apply special behaviour
            if(collision.attachedRigidbody.velocity.y <= 0F)
                ApplySpecialBehaviour(platformType);
    }

    //Apply special behaviour if needed (ie. breakable platform needs to break when hit)
    private void ApplySpecialBehaviour(string type)
    {
        switch (platformType)
        {
            case "Spring":
                Spring spr = gameObject.GetComponent<Spring>();
                spr.swapSprite();
                EdgeCollider2D edgeCollider2D = gameObject.GetComponent<EdgeCollider2D>();
                edgeCollider2D.isTrigger = true;
                GameObject.Find("Doodler").GetComponent<Player>().jumpAnimation();
                break;
            case "Breakable":
                Breakable brk = gameObject.GetComponent<Breakable>();
                brk.swapSprite();
                Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
                rigidbody.bodyType = RigidbodyType2D.Dynamic;
                break;
            case "OneTimeUse":
                GameObject.Find("Doodler").GetComponent<Player>().jumpAnimation();
                Destroy(this.gameObject);
                break;
            default:
                GameObject.Find("Doodler").GetComponent<Player>().jumpAnimation();
                break;
        }
    }
}
