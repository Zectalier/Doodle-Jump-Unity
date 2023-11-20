using UnityEngine;

public class Projectile : MonoBehaviour {
    public float lifetime = 5f;

    public float speed = 16f;

    // Variable that stores the direction of the projectile
    public Vector3 direction;

    private void Start() {
        // Start the countdown to destroy the projectile
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update() {
        // Move the projectile in the direction of the direction variable
        transform.position += direction * speed * Time.deltaTime;
    }
}
