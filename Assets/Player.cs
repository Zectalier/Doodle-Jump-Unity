using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MovementSpeed = 1.0f;
    Rigidbody2D m_Rigidbody;

    public Transform background;
    float leftx;
    float rightx;

    float moveX = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        rightx = background.GetComponent<BoxCollider2D>().size.x / 2;
        leftx = -rightx;
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector2 velocity = m_Rigidbody.velocity;
        velocity.x = moveX * MovementSpeed;
        m_Rigidbody.velocity = velocity;

        if(transform.position.x < leftx)
        {
            Vector2 newPos = new Vector2(rightx, transform.position.y);
            transform.position = newPos;
        }
        else if(transform.position.x > rightx)
        {
            Vector2 newPos = new Vector2(leftx, transform.position.y);
            transform.position = newPos;
        }
    }
}