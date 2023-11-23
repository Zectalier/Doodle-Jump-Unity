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

    float leftx;
    float rightx;
    float moveX = 0f;

    private bool isAndroid = false;

    // Last time the player shot a projectile
    private float lastShotTime = 0f;
    
    private bool accelerationEnabled;
    
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

                    projectile.GetComponent<Projectile>().direction = (new Vector3(mousePos.x, mousePos.y, 0) - transform.position).normalized;
                    
                    // Change the animation variable isShooting to true.
                    GetComponent<Animator>().SetBool("isShooting", true);
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

                projectile.GetComponent<Projectile>().direction = (new Vector3(mousePos.x, mousePos.y, 0) - transform.position).normalized;

                // Change the animation variable isShooting to true.
                GetComponent<Animator>().SetBool("isShooting", true);

                // Set the last shot time to the current time
                lastShotTime = Time.time;
            }
        }

        // Change the animation variable isShooting to true when the last shot time is more than 0.1 seconds ago.
        if (Time.time - lastShotTime > 0.1f)
            GetComponent<Animator>().SetBool("isShooting", false);

        if (accelerationEnabled)
            moveX = Input.acceleration.x*2;
        else
            moveX = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate() {
        Vector2 velocity = m_Rigidbody.velocity;
        velocity.x = moveX * MovementSpeed;
        m_Rigidbody.velocity = velocity;

        // Make the sprite flip using the moveX (do not count 0) and keep scales
        Vector3 scale = transform.localScale;

        if (moveX < 0.2)
            scale.x = 1;
        else if (moveX > 0.2)
            scale.x = -1;

        transform.localScale = scale;

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
}
