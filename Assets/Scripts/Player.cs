using DoodleJump;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject gameManager;
    GameManager gm;
    public float MovementSpeed = 1.0f;
    Rigidbody2D m_Rigidbody;

    public Transform background;

    float leftx;
    float rightx;
    float moveX = 0f;
    // Start is called before the first frame update
    void Start()
    {
        gm = gameManager.GetComponent<GameManager>();
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

        // Make the sprite flip using the moveX (do not count 0) and keep scales
        Vector3 scale = transform.localScale;

        if (moveX < 0)
            scale.x = 1;
        else if (moveX > 0)
            scale.x = -1;

        transform.localScale = scale;

        if (transform.position.x < leftx)
        {
            Vector2 newPos = new Vector2(rightx, transform.position.y);
            transform.position = newPos;
        }
        else if (transform.position.x > rightx)
        {
            Vector2 newPos = new Vector2(leftx, transform.position.y);
            transform.position = newPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            if (gm.getGameState() == GAME_STATE.gameOver){
                this.gameObject.SetActive(false);
            }
            else{
                gm.setGameState(GAME_STATE.gameOver);
            }
            
        }
    }
}
