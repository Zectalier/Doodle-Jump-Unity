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
    Collider2D m_Collider;

    public Transform background;

    float leftx; //position edge background
    float rightx;
    float moveX = 0f;

    public GameObject usedJetpack;
    public GameObject usedPropeller;

    private bool isAndroid = false;
    
    private bool accelerationEnabled;

    Animator m_Animator;

    private Transform bodyTransform;
    private Transform noseTransform;

    private bool facingDirection = false; //false for left, true for right

    private bool isPressing;
    private bool canShoot = true;

    public bool isFlying = false;

    private float flyingSpeed;
    private float flyingTimeEnd;

    private string currentGadget;

    public AudioSource shoot_audio;
    public AudioSource fall_audio;
    public AudioSource jetpack_audio;
    public AudioSource propeller_audio;
    public AudioSource jump_audio;
    // Start is called before the first frame update
    void Start() {
        gm = gameManager.GetComponent<GameManager>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();
        rightx = background.GetComponent<BoxCollider2D>().size.x / 2;
        leftx = -rightx;

        // Check if the platform is Android
        if (Application.platform == RuntimePlatform.Android)
            isAndroid = true;
        accelerationEnabled = SystemInfo.supportsAccelerometer; 

        m_Animator = GetComponent<Animator>();

        bodyTransform = this.gameObject.transform.GetChild(0); //Should be the body Transform
        noseTransform = this.gameObject.transform.GetChild(1); //Should be the nose Transform
    }

    // Update is called once per frame
    void Update() {
        moveX = Input.GetAxis("Horizontal");
        if (isAndroid)
        {
            if (Input.touchCount == 0)
                isPressing = false;
        }
        else
            if (Input.GetKey("up") == false)
                isPressing = false;

        if (isPressing == false && canShoot)
        {
            // If the platform is Android, we shoot where the player touches the screen.
            if (isAndroid)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    // Create a new Projectile and set the position to the player's position.
                    GameObject projectile = Instantiate(projectilePrefab as GameObject);
                    projectile.transform.position = transform.position;

                    // Set the direction of the projectile to where the player touched the screen.
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (mousePos.y < transform.position.y + 2)
                        mousePos.y = transform.position.y + 2;

                    Vector3 newDirection = (new Vector3(mousePos.x, mousePos.y, 0) - transform.position).normalized;
                    projectile.GetComponent<Projectile>().direction = newDirection;

                    noseTransform.up = newDirection;
                    // Change the animation trigger isShooting to true.
                    shootAnimation();
                    isPressing = true;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
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
                    isPressing = true;
                }
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

        // Make the sprite flip using the moveX (do not count 0) and keep scales
        if (velocity.x < -0.5 && facingDirection == true)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            facingDirection = false;
        }
        else if (velocity.x > 0.5 && facingDirection == false)
        {
            Vector3 newScale = gameObject.transform.localScale;
            newScale.x *= -1;
            gameObject.transform.localScale = newScale;
            facingDirection = false; facingDirection = true;
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

        //Flight logic
        if (isFlying){
            if (flyingTimeEnd > Time.time)
                velocity.y = flyingSpeed;
            else
            {
                isFlying = false;
                canShoot = true;
                m_Animator.SetBool("hasJetpack", false);
                m_Animator.SetBool("hasPropeller", false);
                loseGadget();
            }
        }

        m_Rigidbody.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Finish") {
            if (gm.getGameState() == GAME_STATE.gameOver) {
                this.gameObject.SetActive(false);
            }
            else {
                gm.setGameState(GAME_STATE.gameOver);
                fall_audio.Play();
            }
        }
        else if (collision.gameObject.tag == "Monster")
        {
            if (isFlying || flyingTimeEnd + 1 > Time.time) //give 1sec of invicibility
                collision.gameObject.GetComponent<Monster>().doDeath();
            else
            {
                m_Animator.SetBool("isDead", true);
                gm.setGameState(GAME_STATE.gameOver);
                fall_audio.Play();
                // Make the collider a trigger so the player can pass through the monster
                m_Collider.isTrigger = true;
                canShoot = false;
            }
        }
        else if (collision.gameObject.tag == "BlackHole" && !isFlying)
        {
            transform.position = collision.gameObject.transform.position;
            gm.setGameState(GAME_STATE.gameOver);
            fall_audio.Play();
            // Trigger the black hole animation and disable the rigidbody
            m_Animator.ResetTrigger("hasJumped");
            m_Animator.SetTrigger("hasBlackHoled");
            m_Rigidbody.simulated = false;
            canShoot = false;
            StartCoroutine(waitAndDestroy(m_Animator.GetCurrentAnimatorStateInfo(0).length));
        }
    }

    IEnumerator waitAndDestroy(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }

    public void shootAnimation()
    {
        m_Animator.ResetTrigger("hasJumped");
        m_Animator.SetTrigger("hasShot");
        shoot_audio.Play();
    }

    public void jumpAnimation()
    {
        if (m_Animator != null)
        {
            m_Animator.ResetTrigger("hasJumped");
            m_Animator.SetTrigger("hasJumped");
            jump_audio.Play();
        }
    }

    public void jetpackAnimation(string animTrigger)
    {
        m_Animator.ResetTrigger("hasJumped");
        m_Animator.ResetTrigger("hasShot");
        m_Animator.SetBool(animTrigger, true);
        if (animTrigger == "hasJetpack")
            jetpack_audio.Play();
        else
            propeller_audio.Play();
    }

    public void setCanShoot(bool boolean)
    {
        canShoot = boolean;
    }

    public void startFlight(float speed, float time, string animTrigger)
    {
        isFlying = true;
        flyingTimeEnd = Time.time + time;
        flyingSpeed = speed;
        canShoot = false;

        currentGadget = animTrigger;
        jetpackAnimation(animTrigger);
    }

    private void loseGadget()
    {
        if(currentGadget == "hasJetpack"){
            var pref = Instantiate(usedJetpack);
            int direction = (facingDirection)? (-1) : 1;
            pref.transform.position = transform.position + new Vector3((float)(direction * 0.41), -0.41f, 0);
            pref.GetComponent<Rigidbody2D>().velocity = new Vector2(direction, m_Rigidbody.velocity.y + 1);
        }
        else if (currentGadget == "hasPropeller")
        {
            var pref = Instantiate(usedPropeller);
            int direction = (facingDirection) ? (-1) : 1;
            pref.transform.position = transform.position + new Vector3((float)(direction * 0.41), -0.41f, 0);
            pref.GetComponent<Rigidbody2D>().velocity = new Vector2(direction, m_Rigidbody.velocity.y + 1);
        }
    }
}
