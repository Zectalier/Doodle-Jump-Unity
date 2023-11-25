using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public float lifetime = 5f;
    // Start is called before the first frame update
    private void Start()
    {
        // Start the countdown to destroy the projectile
        Destroy(gameObject, lifetime);
    }
}
