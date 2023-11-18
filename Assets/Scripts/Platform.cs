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
    }

    private void ApplySpecialBehaviour(string type)
    {
        switch (platformType)
        {
            case "Spring":
                Spring comp = gameObject.GetComponent<Spring>();
                comp.swapSprite();
                EdgeCollider2D edgeCollider2D = gameObject.GetComponent<EdgeCollider2D>();
                edgeCollider2D.isTrigger = true;
                break;
        }
    }
}
