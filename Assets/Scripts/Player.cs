using DoodleJump;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public GameObject gameManager;

    public GameObject projectilePrefab;

    GameManager gm;
    public float MovementSpeed = 1.0f;
    Rigidbody2D m_Rigidbody;

    public Transform background;

    float leftx; //position edge background
    float rightx;
    float moveX = 0f;

    private bool isAndroid = false;

    // Last time the player shot a projectile
    private float lastShotTime = 0f;
    
    private bool accelerationEnabled;

    Animator m_Animator;

    private Transform bodyTransform;
    private Transform noseTransform;
    private Quaternion noseRotation;

    private bool facingDirection = false; //false for left, true for right

    // Start is called before the first frame update
    void Start() {
        gm = gameManager.GetComponent<GameManager>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        rightx = background.GetComponent<BoxCollider2D>().size.x / 2;
        leftx = -rightx;

        // Check if the platform is Android
        if (Application.platform == RuntimePlatform.Android)
            isAndroid = true;
        accelerationEnabled = SystemInfo.supportsAccelerometer;

        // Set the last shot time to the current time
        lastShotTime = Time.time;

        m_Animator = GetComponent<Animator>();

        bodyTransform = this.gameObject.transform.GetChild(0); //Should be the body Transform
        noseTransform = this.gameObject.transform.GetChild(1); //Should be the nose Transform
    }

    // Update is called once per frame
    void Update() {
        moveX = Input.GetAxis("Horizontal");

        // If the platform is Android, we shoot where the player touches the screen.
        if (isAndroid) {
            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved) {
                    // Create a new Projectile and set the position to the player's position.
                    GameObject projectile = Instantiate(projectilePrefab as GameObject);
                    projectile.transform.position = transform.position;

                    // Set the direction of the projectile to where the player touched the screen.
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if(mousePos.y < transform.position.y + 2)
                        mousePos.y = transform.position.y + 2;

                    Vector3 newDirection = (new Vector3(mousePos.x, mousePos.y, 0) - transform.position).normalized;
                    projectile.GetComponent<Projectile>().direction = newDirection;

                    noseTransform.up = newDirection;
                    // Change the animation trigger isShooting to true.
                    shootAnimation();
                }
            }
        } else {
            if (Input.GetMouseButtonDown(0)) {
                // Create a new Projectile and set the position to the player's position.
                GameObject projectile = Instantiate(projectilePrefab as GameObject);
                projectile.transform.position = transform.position;

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (mousePos.y < transform.position.y + 2)
                    mousePos.y = transform.position.y + 2;

                Vector3 newDirection = (new Vector3(mousePos.x, mousePos.y, 0) - transform.position).normalized;
                projectile.GetComponent<Projectile>().direction = newDirection;

                noseTransform.up = newDirection;
                // Change the animation trigger isShooting to true.
                shootAnimation();

                // Set the last shot time to the current time
                lastShotTime = Time.time;
            }
            else if (Input.GetKey("up"))
            {
                // Create a new Projectile and set the position to the player's position.
                GameObject projectile = Instantiate(projectilePrefab as GameObject);
                projectile.transform.position = transform.position;

                Vector3 newDirection = Vector3.up;
                projectile.GetComponent<Projectile>().direction = newDirection;

                noseTransform.up = newDirection;
                // Change the animation trigger isShooting to true.
                shootAnimation();

                // Set the last shot time to the current time
                lastShotTime = Time.time;
            }
        }


        if (accelerationEnabled)
            moveX = Mathf.Clamp(Input.acceleration.x*2,-1,1);
        else
            moveX = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        Vector2 velocity = m_Rigidbody.velocity;
        velocity.x = moveX * MovementSpeed;
        m_Rigidbody.velocity = velocity;

        // Make the sprite flip using the moveX (do not count 0) and keep scales
        if (velocity.x < -0.5 && facingDirection == true)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            bodyTransform.localScale = scale;
            facingDirection = false;
        }
        else if (velocity.x > 0.5 && facingDirection == false)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            bodyTransform.localScale = scale;
            facingDirection = true;
        }


        //Teleportation logic
        if (transform.position.x < leftx) {
            Vector2 newPos = new Vector2(rightx, transform.position.y);
            transform.position = newPos;
        } else if (transform.position.x > rightx)
        {
            Vector2 newPos = new Vector2(leftx, transform.position.y);
            transform.position = newPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Finish") {
            if (gm.getGameState() == GAME_STATE.gameOver) {
                this.gameObject.SetActive(false);
            }
            else {
                gm.setGameState(GAME_STATE.gameOver);
            }
            
        }
    }

    private void shootAnimation()
    {
        m_Animator.ResetTrigger("hasJumped");
        m_Animator.SetTrigger("hasShot");
    }

    public void jumpAnimation()
    {
        m_Animator.ResetTrigger("hasJumped");
        m_Animator.SetTrigger("hasJumped");
    }
}
