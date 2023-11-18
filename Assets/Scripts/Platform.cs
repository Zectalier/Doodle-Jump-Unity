using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float jumpForce = 10f;
    public string platformType = "Default";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y <= 0F) //Check if object colliding is coming from below
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
        if (collision.tag == "Finish")
            Destroy(this.gameObject);
        else if (collision.tag == "Player" && platformType == "Breakable")
            if(collision.attachedRigidbody.velocity.y <= 0F)
                ApplySpecialBehaviour(platformType);
    }

    private void ApplySpecialBehaviour(string type)
    {
        switch (platformType)
        {
            case "Spring":
                Spring spr = gameObject.GetComponent<Spring>();
                spr.swapSprite();
                EdgeCollider2D edgeCollider2D = gameObject.GetComponent<EdgeCollider2D>();
                edgeCollider2D.isTrigger = true;
                break;
            case "Breakable":
                Breakable brk = gameObject.GetComponent<Breakable>();
                brk.swapSprite();
                Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
                rigidbody.bodyType = RigidbodyType2D.Dynamic;
                break;
        }
    }
}
